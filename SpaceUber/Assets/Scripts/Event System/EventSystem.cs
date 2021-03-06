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
	private Tick tick;
	private AdditiveSceneManager asm;
	private EventCanvas eventCanvas;
    private EventPromptButton eventPromptButton;
	private CampaignManager campMan;

	private int maxEvents = 0;
	private List<GameObject> storyEvents = new List<GameObject>();
	private List<GameObject> randomEvents = new List<GameObject>();

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

    /// <summary>
    /// The percentage chance of rolling an event per failure
    /// </summary>
    [HideInInspector] public float chanceOfEvent;

    private bool skippedToEvent;
    private bool nextEventLockedIn;
    private float eventRollCounter;
    private float timeBeforeEventCounter;
    private Coroutine travelCoroutine;

    public bool NextEventLockedIn => nextEventLockedIn;

	public bool eventActive { get; private set; } = false;

	// loaded in from Interface_EventTimer
	private GameObject sonarObjects; // event timer UI
	private EventWarning eventWarning; // warning
	private EventSonar sonar; // sonar

	private Job currentJob;

	private string lastEventTitle;

	[HideInInspector] public bool chatting = false; //Whether or not the player is talking to a character
	[HideInInspector] public bool mutiny;

	[SerializeField, Tooltip("The maximum cooldown for a character chat in ticks.")] public int chatCooldown = 3;

	private void Awake()
	{
		//Singleton pattern
		if (instance) { Destroy(gameObject); }
		else { instance = this; }

		ship = FindObjectOfType<ShipStats>();
		tick = FindObjectOfType<Tick>();
		asm = FindObjectOfType<AdditiveSceneManager>();
		campMan = GetComponent<CampaignManager>();
	}

	private void SetUpEventTimer()
	{
		eventWarning = FindObjectOfType<EventWarning>();
		sonar = FindObjectOfType<EventSonar>();
		sonarObjects = sonar.transform.parent.gameObject; // event timer UI

		//set sonar stuff
		sonar.SetSpinRate( eventChanceFreq );
		sonarObjects.SetActive(false);

		if(eventWarning != null)
		{
			eventWarning.DeactivateWarning();
		}
	}

    /// <summary>
    /// Plays job intro
    /// </summary>
    public IEnumerator PlayIntro()
    {
	    yield return new WaitUntil(() => SceneManager.GetSceneByName("Interface_Runtime").isLoaded);
	    SetUpEventTimer();

		chatting = false;
		while(currentJob == null)
        {
			yield return null;
        }

		//check for an introduction "event"
		GameObject intro = (from introEvent in currentJob.introEvents 
			let requirements = introEvent.GetComponent<InkDriverBase>().requiredStats 
			where HasRequiredStats(requirements) select introEvent).FirstOrDefault();

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
        travelCoroutine = StartCoroutine(Travel());
    }

	private IEnumerator Travel()
	{
		tick.DaysSince = 0; // reset days since
		campMan.cateringToTheRich.SaveEventChoices();

		// loops once per event
		while (GameManager.instance.currentGameState == InGameStates.Events)
		{
			// wait till any active event is cleared before starting event timer for next event
			yield return new WaitWhile((() => eventActive));
			tick.StartTickUpdate();
			sonarObjects.SetActive(true);
			sonar.ResetSonar();
			chanceOfEvent = startingEventChance;

			//start with one big chunk of time
			while (timeBeforeEventCounter <= timeBeforeEventRoll)
			{
				if (!mutiny) // don't increment timer during mutiny
				{
					// count up during the grace period
					timeBeforeEventCounter += Time.deltaTime;
				}

				yield return new WaitForEndOfFrame();
			}

            asm.LoadSceneMerged("Event_Prompt");
            yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_Prompt").isLoaded);
            eventPromptButton = FindObjectOfType<EventPromptButton>();
            eventPromptButton.eventButton.onClick.AddListener(SkipToEvent);

            // roll for next event unless skipped to it
            while (!skippedToEvent && eventRollCounter <= eventChanceFreq)
            {
				if(!mutiny) // don't increment timer during mutiny
				{
					// count up for every roll
					eventRollCounter += Time.deltaTime;
					// if reached next roll
					if (eventRollCounter >= eventChanceFreq)
					{
						if (WillRunEvent(chanceOfEvent))
						{
							nextEventLockedIn = true;
							//Activate the warning for the next event now that one has been picked
							if (eventWarning != null)
							{
								eventWarning.ActivateWarning();
							}
							break;
						}

						chanceOfEvent += chanceIncreasePerFreq;
						eventRollCounter = 0; // reset roll counter
					}
				}
					
				yield return new WaitForEndOfFrame();
            }

            // once event rolled or skipped

            tick.StopTickUpdate();
            FindObjectOfType<CrewManagement>().TurnOffPanel();

			//wait until done with minigame and/or character event
			yield return new WaitUntil(() => !OverclockController.instance.overclocking && !chatting);

            //If event button was not clicked ahead of time
            if (nextEventLockedIn && SceneManager.GetSceneByName("Event_Prompt").isLoaded)
            {
                eventPromptButton.backDrop.SetActive(true);
            }

            // wait for player to go click button to go to event
            yield return new WaitUntil((() => skippedToEvent));

            // wait for event to conclude
            yield return new WaitWhile((() => eventActive));
		}

		sonarObjects.SetActive(false);
        tick.StopTickUpdate();
	}

	/// <summary>
	/// Called by the go to event button to spawn a random/story event
	/// </summary>
	/// <returns>Returns true when complete</returns>
	private void SkipToEvent()
	{
		if (skippedToEvent) return;

	    skippedToEvent = true;
	    asm.UnloadScene("Event_Prompt");

	    //get rid of and reset sonar objects
	    eventWarning.DeactivateWarning();
	    sonarObjects.SetActive(false);

	    //if it's an even-numbered event, do a story
	    if (overallEventIndex % 2 == 1 && overallEventIndex != 0)
	    {
		    StartCoroutine(StartStoryEvent());
	    }
	    else
	    {
		    StartCoroutine(StartRandomEvent());
	    }
    }

	/// <summary>
	/// Starts the next story event
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartStoryEvent()
	{
		// Load Event_General Scene for upcoming event
		asm.LoadSceneMerged("Event_General");
		yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_General").isLoaded);

		GameObject newEvent = FindNextStoryEvent();
		CreateEvent(newEvent);
		overallEventIndex++;
	}

	/// <summary>
	/// Starts random story event
	/// </summary>
	/// <returns></returns>
	private IEnumerator StartRandomEvent()
	{
		GameObject newEvent = RandomizeEvent();

		// make sure there is a random event
		while (newEvent == null)
		{
			Debug.LogWarning("Didn't find a random event, trying again.");
			newEvent = RandomizeEvent();
		}

		// Load Event_CharacterFocused Scene for upcoming event
		asm.LoadSceneMerged("Event_CharacterFocused");
		yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_CharacterFocused").isLoaded);

		CreateEvent(newEvent);
		randomEventIndex++;
		overallEventIndex++;
	}

	/// <summary>
	/// Generates a new character event. Does nothing
	/// </summary>
	/// <param name="possibleEvents"></param>
	/// <returns></returns>
	public IEnumerator StartNewCharacterEvent(List<GameObject> possibleEvents)
    {
	    //TODO: Change Character Events to not pause Tick
		chatting = true;
		tick.StopTickUpdate();
		FindObjectOfType<CrewManagement>().TurnOffPanel();
		GameObject newEvent = FindNextCharacterEvent(possibleEvents);

		if (newEvent != null)
        {
			asm.LoadSceneMerged("Event_CharacterFocused");
			print("Starting a new character event");
			yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_CharacterFocused").isLoaded);
			CreateEvent(newEvent);
        }
    }

	public void CreateMutinyEvent(GameObject newEvent)
	{
		mutiny = true;
		tick.StopTickUpdate();
		sonarObjects.SetActive(false);
		FindObjectOfType<CrewManagement>().TurnOffPanel();

		// set event variables
		//InkDriverBase mutinyEvent = newEvent.GetComponent<InkDriverBase>();
		//int mutinyCost = MoraleManager.instance.GetMutinyCost();
		//mutinyEvent.nextChoices[0].choiceRequirements[0].requiredAmount = mutinyCost;
		//mutinyEvent.nextChoices[0].outcomes[0].amount = -mutinyCost;

		StartCoroutine(SetupMutinyEvent(newEvent));
	}

	private IEnumerator SetupMutinyEvent(GameObject newEvent)
	{
		//wait until there is no longer an overclock microgame happening
		yield return new WaitUntil(() => !OverclockController.instance.overclocking);

		asm.LoadSceneMerged("Event_CharacterFocused");
		yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_CharacterFocused").isLoaded);
		CreateEvent(newEvent);
	}

	/// <summary>
	/// Spawns the event (Gameobject prefab) chosen in Travel().
	/// Assigns the proper canvas to the created InkDriverBase script
	/// </summary>
	/// <param name="newEvent"></param>
	private void CreateEvent(GameObject newEvent)
	{
		eventCanvas = FindObjectOfType<EventCanvas>();
		eventInstance = Instantiate(newEvent, eventCanvas.canvas.transform);

		if (eventInstance.TryGetComponent(out InkDriverBase inkDriver))
		{
			inkDriver.AssignStatusFromEventSystem(eventCanvas.titleBox, eventCanvas.textBox,eventCanvas.choiceResultsBox,
				eventCanvas.backgroundImage, eventCanvas.buttonGroup, ship, campMan);
		}

        eventActive = true;
        //Does not increment overall event index because intro event does not increment it

        AnalyticsManager.OnEventStarted(inkDriver, nextEventLockedIn);
	}

    /// <summary>
    /// Ends the event that is currently running.
    /// If the max number of events has been reached, go to the ending
    /// </summary>
    public void ConcludeEvent()
	{
		bool isRegularEvent = true;

		InkDriverBase concludedEvent = eventInstance.GetComponent<InkDriverBase>();
		concludedEvent.ClearUI();

		if(concludedEvent.isCharacterEvent)
		{
			isRegularEvent = false;
			chatting = false;
			tick.DaysSinceChat = 0;
			eventInstance.GetComponent<CharacterEvent>().EndCharacterEvent();
		}
		else if (concludedEvent.isMutinyEvent)
		{
			isRegularEvent = false;
			mutiny = false;
		}

		AnalyticsManager.OnEventComplete(concludedEvent);
		Destroy(eventInstance);

		//Go back to travel scene
		asm.UnloadScene("Event_General");
		asm.UnloadScene("Event_CharacterFocused");
		AudioManager.instance.PlayMusicWithTransition("General Theme");

		//reset for next event
		eventActive = false;
		sonarObjects.SetActive(true);
		tick.StartTickUpdate();

		//set up for the next regular event
		if (isRegularEvent)
		{
			sonar.ResetSonar();
			tick.DaysSince = 0; // reset days since
			skippedToEvent = false;
			nextEventLockedIn = false;
			eventRollCounter = 0;
			timeBeforeEventCounter = 0;
		}

		if (overallEventIndex >= maxEvents) //Potentially end the job entirely if this is meant to be the final event
		{
			ClearEventSystemAtEndOfJob();
			ship.CashPayout();
			GameManager.instance.ChangeInGameState(InGameStates.CrewPayment);
		}
	}

	private void ClearEventSystemAtEndOfJob()
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
		chatting = false;
		mutiny = false;
		skippedToEvent = false;
		nextEventLockedIn = false;
		eventActive = false;
		eventRollCounter = 0;
		timeBeforeEventCounter = 0;
		tick.DaysSinceChat = 0;
		if (travelCoroutine != null) StopCoroutine(travelCoroutine);
		tick.StopTickUpdate();
	}

	public void ResetJob()
	{
		overallEventIndex = 0;
		storyEventIndex = 0;
		randomEventIndex = 0;
		eventInstance = null;
		lastEventTitle = "";
		chatting = false;
		mutiny = false;
		skippedToEvent = false;
		nextEventLockedIn = false;
		eventActive = false;
		eventRollCounter = 0;
		timeBeforeEventCounter = 0;
		tick.DaysSinceChat = 0;
		if(travelCoroutine != null) StopCoroutine(travelCoroutine);
	}

	/// <summary>
	/// Decides whether or not an event will run.
	/// </summary>
	/// <param name="chances">The number of times that the event chance has failed. The higher it is, the more likely an event</param>
	/// <returns></returns>
	private bool WillRunEvent(float chances)
	{
        if(sonar.gameObject.activeSelf)
        {
            sonar.ShowNextDot();
        }
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

	private bool HasPossibleCharacterEvent(List<GameObject> possibleEvents)
    {
		foreach (GameObject charEvent in possibleEvents)
		{
			CharacterEvent eventDriver = charEvent.GetComponent<CharacterEvent>();
			List<Requirements> requirements = eventDriver.requiredStats;

			if (HasRequiredStats(requirements))
			{
				return true; //end function as soon as one is found
			}
		}
		return false;
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
		List<GameObject> goodEvents = new List<GameObject>(); //add all events that are possible to this list

		foreach ( GameObject charEvent in possibleEvents)
        {
			CharacterEvent eventDriver = charEvent.GetComponent<CharacterEvent>();
			List<Requirements> requirements = eventDriver.requiredStats;

			if (HasRequiredStats(requirements))
            {
				goodEvents.Add(charEvent);
            }
		}

		if(goodEvents.Count > 0)
        {
			int chosen = Random.Range(0, goodEvents.Count);
			possibleEvents.Remove(goodEvents[chosen]); //remove this one from the list
			return goodEvents[chosen];
        }
		else
        {
			print("Could not get an event");
			return null;
        }
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

	public bool CanChat(List<GameObject> checkEvents)
	{
		//If chat has cooleddown
		if (tick.DaysSinceChat < chatCooldown)
		{
			print("Not ready to chat");
			return false;
		}

		//if no possible events are found
		if (!HasPossibleCharacterEvent(checkEvents))
		{
			print("No events available");
			return false;
		}

		return true;
	}
}
