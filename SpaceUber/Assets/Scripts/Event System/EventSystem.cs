/*
 * EventSystem.cs
 * Author(s): Greg Brandt, Scott Acker, Steven Drovie
 * Created on: 9/17/2020 (en-US)
 * Description: Controls how and when events spawn. Events are sorted into random and story lists
 * Random events are chosen randomly, but story events are played in order.
 * There is a random chance for an event to play every x seconds, with the chance increasing each time
 * When the number of events played reaches the maxEvents number, the job ends
 */

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Ink.Parsed;
using TMPro;

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	private ShipStats ship;
	private AdditiveSceneManager asm;
	private EventCanvas eventCanvas;
	private CampaignManager campMan;
	
	private int maxEvents = 0;
	private List<GameObject> storyEvents = new List<GameObject>();
	private List<GameObject> randomEvents = new List<GameObject>();
	
	[SerializeField, Tooltip("All possible character events. Temporary")]
	private List<GameObject> allCharacterEvents;

	//how many events (story and random) have occurred
	private int overallEventIndex = 0;
	// How many story events have occurred. Tells the code which story event to play
	private int storyEventIndex = 0;
	//how many random events have passed. Tells code how many events to ignore at the start of the list
	private int randomEventIndex = 0;

	/// <summary>
	/// The specific event being played right now
	/// </summary>
	GameObject eventInstance;

	[Tooltip("How many seconds it will take to attempt an event roll")]
	[SerializeField] private float eventChanceFreq = 10;

	[Tooltip("How many seconds before the first event roll")]
	[SerializeField] private float timeBeforeEventRoll = 40;

	[Tooltip("How much the percentage chance of rolling an event will increase per failure")]
	[SerializeField] private float chanceIncreasePerFreq = 20;

	[Tooltip("Initial percentage chance of rolling an event")]
	[SerializeField] private float startingEventChance = 5;

	
	private bool isTraveling = false;
	public bool eventActive { get; private set; } = false;

	[SerializeField] private GameObject sonarObjects;
	[SerializeField] private EventWarning eventWarning;
	[SerializeField] private EventSonar sonar;
	private Job currentJob;

	private string lastEventTitle;

	private bool chatting = false; //Whether or not the player is talking to a character
	
	[SerializeField, Tooltip("The maximum cooldown for a character chat in ticks.")] public int chatCooldown;
	private int daysSinceChat;

	private void Awake()
	{
		//Singleton pattern
		if (instance) { Destroy(gameObject); }
		else { instance = this; }

		ship = FindObjectOfType<ShipStats>();
		asm = FindObjectOfType<AdditiveSceneManager>();
		campMan = GetComponent<CampaignManager>();
		
		if(eventWarning != null)
		{
			eventWarning.DeactivateWarning();
		}

		//set sonar stuff
		sonar.SetSpinRate( eventChanceFreq );
		sonarObjects.SetActive(false);
		chatting = false;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
			StartCoroutine(StartNewCharacterEvent(allCharacterEvents));
        }
    }

	public bool CanChat(List<GameObject> checkEvents)
    {
		//If chat has cooleddown
		if(daysSinceChat < chatCooldown)
        {
			print("Cooldown active");
			return false;
        } 
		
		//if no possible events are found
		if(FindNextCharacterEvent(checkEvents) == null)
        {
			print("Could not manage an event");
			return false;
        }

		return true;
    }

    /// <summary>
    /// Plays job intro
    /// </summary>
    public IEnumerator PlayIntro()
    {
		chatting = false;
		while(currentJob == null)
        {
			yield return null;
        }

		//check for an introduction "event"
		GameObject intro = null;
		foreach (var introEvent in currentJob.introEvents)
		{
			List<Requirements> requirements = introEvent.GetComponent<InkDriverBase>().requiredStats;
			if (HasRequiredStats(requirements))
			{
				intro = introEvent;
				break;
			}
		}

		if (intro != null)
        {
			// Load Event_General Scene for upcoming event
			asm.LoadSceneMerged("Event_General");
			yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_General").isLoaded);

			eventCanvas = FindObjectOfType<EventCanvas>();

			CreateEvent(intro);
        }
		else
        {
			print("Found nothing in currentJob");
        }

        //Go to the travel coroutine
        StartCoroutine(EventSystem.instance.Travel());
    }

	public IEnumerator Travel()
	{
		ship.ResetDaysSince();
		campMan.cateringToTheRich.SaveEventChoices();

		//For the intro event
		yield return new WaitWhile((() => eventActive));

		float chanceOfEvent = startingEventChance;
		while (GameManager.instance.currentGameState == InGameStates.Events) //game state while events are possible
		{
			//pause this while chatting with folks
			while(chatting)
            {
				yield return new WaitForSeconds(.2f);
            }
			daysSinceChat += 1;

            ship.StartTickEvents();
			sonarObjects.SetActive(true);
			sonar.ResetSonar();

			yield return new WaitForSeconds(timeBeforeEventRoll); //start with one big chunk of time

			//A check to be sure that the current game state is not the event scenes
			if (GameManager.instance.currentGameState != InGameStates.Events)
            {
                break;
            }

            #region Start sonar/event chance loop
            //run random chances for event to take place in a loop
            while (!WillRunEvent(chanceOfEvent))
			{				
				isTraveling = true;
				chanceOfEvent+= chanceIncreasePerFreq;
				yield return new WaitForSeconds(eventChanceFreq);
			}
			//Once again make sure that this is not an event scene
            if(GameManager.instance.currentGameState != InGameStates.Events)
            {
                break;
            }
            #endregion

            //Activate the warning for the next event now that one has been picked
            if (eventWarning != null)
			{
				eventWarning.ActivateWarning();
			}
			ship.PauseTickEvents();

			//wait until there is no longer an overclock microgame happening
			yield return new WaitUntil(() => !OverclockController.instance.overclocking);
			
			//get rid of and reset sonar objects
			eventWarning.DeactivateWarning();
			sonarObjects.SetActive(false);
            
			#region Spawn an event
            if (overallEventIndex % 2 == 1 && overallEventIndex != 0) //if it's an even-numbered event, do a story 
			{
				// Load Event_General Scene for upcoming event
				asm.LoadSceneMerged("Event_General");
				yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_General").isLoaded);

				eventCanvas = FindObjectOfType<EventCanvas>();

				GameObject newEvent = FindNextStoryEvent();
				CreateEvent(newEvent);
				overallEventIndex++;

				yield return new WaitWhile((() => eventActive));
			}
			else if (!eventActive && randomEventIndex < randomEvents.Count) //Pick a random event
			{
				GameObject newEvent = RandomizeEvent();

				if (newEvent != null) //check to be sure a random event was still chosen
				{
					// Load Event_CharacterFocused Scene for upcoming event 
					asm.LoadSceneMerged("Event_CharacterFocused");
					yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_CharacterFocused").isLoaded);

					eventCanvas = FindObjectOfType<EventCanvas>();

					CreateEvent(newEvent);
					randomEventIndex++;
					overallEventIndex++;

					yield return new WaitWhile((() => eventActive));
				}
			}
			#endregion

			//set up the sonar for the next event
			sonarObjects.SetActive(true);
			sonar.ResetSonar();
			ship.UnpauseTickEvents();
		}
		isTraveling = false;
		sonarObjects.SetActive(false);
        ship.StopTickEvents();
	}

	/// <summary>
	/// Generates a new character event. Does nothing
	/// </summary>
	/// <param name="possibleEvents"></param>
	/// <returns></returns>
	public IEnumerator StartNewCharacterEvent(List<GameObject> possibleEvents)
    {
		chatting = true;
		GameObject newEvent = FindNextCharacterEvent(possibleEvents);

		asm.LoadSceneMerged("Event_CharacterFocused");
		print("Starting a new character event");

		yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_CharacterFocused").isLoaded);

		eventCanvas = FindObjectOfType<EventCanvas>();

		if (newEvent != null)
        {
			CreateEvent(newEvent);
        }
    }

	/// <summary>
	/// Spawns the event (Gameobject prefab) chosen in Travel().
	/// Assigns the proper canvas to the created InkDriverBase script
	/// </summary>
	/// <param name="newEvent"></param>
	private void CreateEvent(GameObject newEvent)
	{
		eventInstance = Instantiate(newEvent, eventCanvas.canvas.transform);

		if (eventInstance.TryGetComponent(out InkDriverBase inkDriver))
		{
			inkDriver.AssignStatusFromEventSystem(eventCanvas.titleBox, eventCanvas.textBox,eventCanvas.choiceResultsBox,
				eventCanvas.backgroundImage, eventCanvas.buttonGroup, ship, campMan);
			
		}

		eventActive = true;
		//Does not increment overall event index because intro event does not increment it
	}

	/// <summary>
	/// Ends the event that is currently running.
	/// If the max number of events has been reached, go to the ending
	/// </summary>
	public void ConcludeEvent()
	{
		ship.ResetDaysSince();
		eventInstance.GetComponent<InkDriverBase>().ClearUI();

		//in case a random event isn't chosen
		if (eventInstance != null)
		{
			if(eventInstance.GetComponent<InkDriverBase>().isCharacterEvent)
            {
				chatting = false;
				daysSinceChat = 0;
				eventInstance.GetComponent<CharacterEvent>().EndCharacterEvent();
				//TODO: Find a way to overclock the connected room
			}
			else
            {
				//Potentially end the job entirely if this is meant to be the final event
				if (overallEventIndex >= maxEvents)
				{
					ClearEventSystem();
					ship.CashPayout();
					GameManager.instance.ChangeInGameState(InGameStates.CrewPayment);
				}
			}
			Destroy(eventInstance);
		}

		//Go back to travel scene
		asm.UnloadScene("Event_General");
		asm.UnloadScene("Event_CharacterFocused");
		AudioManager.instance.PlayMusicWithTransition("General Theme");


		eventActive = false;
	}

	private void ClearEventSystem()
	{
		storyEvents.Clear();
		randomEvents.Clear();
		currentJob = null;
		maxEvents = 0;
		overallEventIndex = 0;
		storyEventIndex = 0;
		randomEventIndex = 0;
		eventInstance = null;
		lastEventTitle = "";
	}

	public void ResetJob()
	{
		overallEventIndex = 0;
		storyEventIndex = 0;
		randomEventIndex = 0;
		eventInstance = null;
		lastEventTitle = "";
	}

	/// <summary>
	/// Decides whether or not an event will run. 
	/// </summary>
	/// <param name="chances">The number of times that the event chance has failed. The higher it is, the more likely an event</param>
	/// <returns></returns>
	private bool WillRunEvent(float chances)
	{
		sonar.ShowNextDot();
		float rng = Random.Range(0, 101);

		return rng <= chances;
	}

	/// <summary>
	/// Finds the next story event by cycling through all possible versions of the next story event. 
	/// </summary>
	/// <returns></returns>
	private GameObject FindNextStoryEvent()
    {
		// search for the next event with the correct variation
		foreach (var storyEvent 
			in from storyEvent in storyEvents 
			let story = storyEvent.GetComponent<InkDriverBase>() let requirements = story.requiredStats 
			where story.storyIndex == storyEventIndex && HasRequiredStats(requirements) select storyEvent)
		{
			++storyEventIndex;
			return storyEvent;
		}

		Debug.LogWarning("Couldn't find next story event");
		return null;
    }

	/// <summary>
	/// Returns the next chosen event that can be played from the possible list supplied.
	/// If null, do not allow players to go into the event screen. Instead, inform them that no new events 
	/// are available.
	/// </summary>
	/// <param name="possibleEvents"></param>
	/// <returns></returns>
	private GameObject FindNextCharacterEvent(List<GameObject> possibleEvents)
    {
        //if charEvent matches requirements, pick this one and remove it from the group
        
		//foreach (var charEvent
		//in from charEvent in possibleEvents
		//   let eventDriver = charEvent.GetComponent<CharacterEvent>()
		//   let requirements = eventDriver.requiredStats
		//   where HasRequiredStats(requirements) && eventDriver.playedOnce == false //meets requirements and has never been played before
		//   select charEvent)
		//{
		//	GameObject chosen = charEvent;
		//	charEvent.GetComponent<CharacterEvent>().playedOnce = true;
		//	return chosen;

		//}

		foreach( GameObject charEvent in possibleEvents)
        {
			CharacterEvent eventDriver = charEvent.GetComponent<CharacterEvent>();
			List<Requirements> requirements = eventDriver.requiredStats;

			eventDriver.playedOnce = false; //DELETE Later

			if(HasRequiredStats(requirements) && eventDriver.playedOnce == false)
            {
				return charEvent;
            }
		}

		return null;

	}

	/// <summary>
	/// Picks a random event to spawn. The random numbers include the eventIndex to the count of storyEvents list
	/// When an event is picked, it is moved to the start of the list and cannot be picked again.
	/// If an event has requirements that are not met, the code  will shuffle until it finds an event that can be run, or
	/// there are no more events
	/// </summary>
	/// <returns></returns>
	private GameObject RandomizeEvent()
	{
		GameObject thisEvent = randomEvents[randomEventIndex];

		if (randomEventIndex != randomEvents.Count)
		{
			int eventNum = Random.Range(randomEventIndex, randomEvents.Count);
			thisEvent = randomEvents[eventNum];
			List<Requirements> requirements = thisEvent.GetComponent<InkDriverBase>().requiredStats;

			//if the event chosen has requirements that are not met
			if (!HasRequiredStats(requirements))
			{
				//copies the current index
				int newIndex = randomEventIndex;
				List<GameObject> newRandomEvents = randomEvents;

				//Copies the list to shuffle until it finds a new event to do or runs out of ideas
				while (!HasRequiredStats(requirements) && newIndex != randomEvents.Count)
				{
					//choose an event to check
					int newNum = Random.Range(newIndex, newRandomEvents.Count);

					thisEvent = newRandomEvents[newNum];

					//insert this event at the beginning of the list so it cannot be picked again
					newRandomEvents.RemoveAt(eventNum);
					newRandomEvents.Insert(0, thisEvent);

					newIndex++;
				}

				return null;
			}

			randomEvents.RemoveAt(eventNum);
			randomEvents.Insert(0, thisEvent);
		}

		return thisEvent;
	}


	/// <summary>
	/// Runs through the list of requirements for a job and determines if each and every one is met
	/// </summary>
	/// <param name="selectedRequirements"></param>
	/// <returns></returns>
	private bool HasRequiredStats(List<Requirements> selectedRequirements)
	{
		bool result = true;

		foreach (Requirements required in selectedRequirements)
		{
			//break the loop the second that one requirement doesn't match
			if (!required.MatchesRequirements(ship, campMan))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	/// <summary>
	/// Takes the events supplied in newJob and applies them to the event lists here
	/// </summary>
	/// <param name="newJob"></param>
	public void TakeStoryJobEvents(Job newJob)
	{
		storyEvents.AddRange(newJob.storyEvents);
		randomEvents.AddRange(newJob.randomEvents);
		currentJob = newJob;
		maxEvents += newJob.maxStoryEvents;
		maxEvents += newJob.maxRandomEvents;
	}

	public void TakeSideJobEvents(List<Job> sideJobs)
    {
		foreach(Job newJob in sideJobs)
        {
	        randomEvents.AddRange(newJob.randomEvents);
	        maxEvents += newJob.maxRandomEvents;
        }
    }
}
