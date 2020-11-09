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
using UnityEngine.SceneManagement;
using Ink.Parsed;

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

	//how many events (story and random) have occurred
	private int overallEventIndex = 0;
	// How many story events have occurred. Tells the code which story event to play
	private int storyEventIndex = 0;
	//how many random events have passed. Tells code how many events to ignore at the start of the list
	private int randomEventIndex = 0;

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

	[SerializeField] private GameObject eventWarning;
	[SerializeField] private EventSonar sonar;
	private Job currentJob;

	private string lastEventTitle;

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
			eventWarning.SetActive(false);
		}
		sonar.HideSonar();
	}

	public IEnumerator Travel()
	{
		//set up the sonar
		sonar.ShowSonar();
		sonar.ResetSonar();

		float chanceOfEvent = startingEventChance;
		while (GameManager.instance.currentGameState == InGameStates.Events)
		{
            ship.StartTickEvents();
            
			yield return new WaitForSeconds(timeBeforeEventRoll); //start with one big chunk of time
            if(GameManager.instance.currentGameState != InGameStates.Events)
            {
                break;
            }
            
			//run random chances for event to take place
			while (!WillRunEvent(chanceOfEvent))
			{				
				isTraveling = true;
				chanceOfEvent+= chanceIncreasePerFreq;
				yield return new WaitForSeconds(eventChanceFreq);
			}
            if(GameManager.instance.currentGameState != InGameStates.Events)
            {
                break;
            }

			//Activate the warning for the next event now
			if (eventWarning != null)
			{
				eventWarning.SetActive(true);
			}
			ship.PauseTickEvents();

			//wait until there is no longer an overclock microgame happening
			yield return new WaitUntil(() => !OverclockController.instance.overclocking);
			
			//turn off the event warning because an event is about to begin
			if (eventWarning != null)
			{
				eventWarning.SetActive(false);
			}
			sonar.HideSonar();

			

			//Time to decide on an event
			//story events happen every other time 
			if (overallEventIndex % 2 == 1 && overallEventIndex != 0) //if it's an even-numbered event, do a story 
			{
				// Load Event_General Scene for upcoming event
				asm.LoadSceneMerged("Event_General");
				yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_General").isLoaded);

				eventCanvas = FindObjectOfType<EventCanvas>();

				GameObject newEvent = FindNextStoryEvent();
				CreateEvent(newEvent);
				storyEventIndex++;

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
                    
					yield return new WaitWhile((() => eventActive));
				}
				else
				{
					ConcludeEvent();
				}
			}
            
			//set up the sonar for the next event
			sonar.ShowSonar();
			sonar.ResetSonar();
            ship.UnpauseTickEvents();
		}
		isTraveling = false;
		sonar.HideSonar();
        ship.StopTickEvents();
	}

	/// <summary>
	/// Spawns the event (Gameobject prefab) chosen in Travel().
	/// Assigns the proper canvas to the created InkDriverBase script
	/// </summary>
	/// <param name="newEvent"></param>
	public void CreateEvent(GameObject newEvent)
	{
		eventInstance = Instantiate(newEvent, eventCanvas.canvas.transform);

		if (eventInstance.TryGetComponent(out InkDriverBase inkDriver))
		{
			inkDriver.AssignStatusFromEventSystem(eventCanvas.titleBox, eventCanvas.textBox,
				eventCanvas.backgroundImage, eventCanvas.buttonGroup, ship, campMan);
			
		}

		eventActive = true;
		overallEventIndex++;
	}

	/// <summary>
	/// Ends the event that is currently running.
	/// If the max number of events has been reached, go to the ending
	/// </summary>
	public void ConcludeEvent()
	{
		eventInstance.GetComponent<InkDriverBase>().ClearUI();

		//in case a random event isn't chosen
		if (eventInstance != null)
		{
			Destroy(eventInstance);
		}

		//Go back to travel scene
		asm.UnloadScene("Event_General");
		asm.UnloadScene("Event_CharacterFocused");
		AudioManager.instance.PlayMusicWithTransition("General Theme");

		//Potentially end the job entirely
		if (overallEventIndex >= maxEvents)
		{
			ClearEventSystem();
			ship.CashPayout();
			GameManager.instance.ChangeInGameState(InGameStates.CrewPayment);
		}

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

	/// <summary>
	/// Decides whether or not an event will run. 
	/// </summary>
	/// <param name="chances">The number of times that the event chance has failed. The higher it is, the more likely an event</param>
	/// <returns></returns>
	private bool WillRunEvent(float chances)
	{
		sonar.ShowNextDot();
		float rng = Random.Range(0, 101);

		if (rng <= chances)
		{
			return true; 
		}
		else
		{ return false; }


	}

	/// <summary>
	/// Finds the next story event by cycling through all possible versions of the next story event. 
	/// </summary>
	/// <returns></returns>
	private GameObject FindNextStoryEvent()
    {
		GameObject result = storyEvents[storyEventIndex];
		bool found = false;
		
		// search for the next event with the correct variation
		for (int i = storyEventIndex; i < storyEvents.Count; i++)
		{
			if(!found)
			{
				GameObject thisEvent = storyEvents[i];
				List<Requirements> requirements = thisEvent.GetComponent<InkDriverBase>().requiredStats;

				//grabs first eight letters of ink file name to check the naming code
				string thisEventTitle = thisEvent.GetComponent<InkDriverBase>().inkJSONAsset.name.Substring(0, 8); 

				if (lastEventTitle != thisEventTitle) //if this is not a variation of the same event as the last one, check requirements
				{
					if (HasRequiredStats(requirements))
					{
						result = thisEvent;
						found = true;
					}
				}
				else //if it's the same event as the last one we played, just keep going.
				{
					++storyEventIndex;
				}
			}
		}

		lastEventTitle = result.GetComponent<InkDriverBase>().inkJSONAsset.name.Substring(0, 8);
		return result;
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
		maxEvents += newJob.storyEvents.Count;
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
