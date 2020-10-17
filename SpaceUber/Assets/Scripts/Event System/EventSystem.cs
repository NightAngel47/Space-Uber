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

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	private ShipStats ship;
	private AdditiveSceneManager asm;

	//how many events will happen in this journey
	public int maxEvents = 3;
	[SerializeField] private List<GameObject> storyEvents;
	[SerializeField] private List<GameObject> randomEvents;

	private EventCanvas eventCanvas;

	GameObject eventInstance;
	
	//time inbetween possible event triggers
	float travelTicTime = 5;
	bool isTraveling = false;
	bool eventActive = false;

	//how many events (story and random) have occurred
	private int overallEventIndex = 0;
	// How many story events have occurred. Tells the code which story event to play
	private int storyEventIndex = 0;
	//how many random events have passed. Tells code how many events to ignore at the start of the list
	private int randomEventIndex = 0;

	public bool doingTasks = false;
	public GameObject eventWarning;

	private void Awake()
	{
		//Singleton pattern
		if(instance) { Destroy(gameObject); }
		else { instance = this; }

		ship = FindObjectOfType<ShipStats>();
		asm = FindObjectOfType<AdditiveSceneManager>();

		eventWarning.SetActive(false);
	}

	public IEnumerator Travel()
	{
		while (GameManager.currentGameState == InGameStates.Events)
		{
			yield return new WaitForSeconds(20); //start with one big chunk of time

			//run random chances for event to take place
			int chanceOfEvent = 1;
			while (!WillRunEvent(chanceOfEvent))
			{
				print("Did not pick an event");
				isTraveling = true;
				++chanceOfEvent;
				yield return new WaitForSeconds(travelTicTime);
			}

			eventWarning.SetActive(true);
			yield return new WaitUntil(() => !doingTasks);
			eventWarning.SetActive(false);

			// Load Event_General Scene for upcoming event
			asm.LoadSceneMerged("Event_General");
			yield return new WaitUntil(() => SceneManager.GetSceneByName("Event_General").isLoaded);

			eventCanvas = FindObjectOfType<EventCanvas>();
			
			//Time to decide on an event
			//story events happen every other time 
			if (overallEventIndex % 2 == 1 && overallEventIndex != 0) //if it's an even-numbered event, do a story 
			{
				CreateEvent(storyEvents[storyEventIndex]);
				storyEventIndex++;

				yield return new WaitWhile((() => eventActive));
			}
			else if (!eventActive && randomEventIndex < randomEvents.Count) //Pick a random event
			{
				GameObject newEvent = RandomizeEvent();
				
				if(newEvent != null) //check to be sure a random event was still chosen
				{
					CreateEvent(newEvent);
					randomEventIndex++;
					yield return new WaitWhile((() => eventActive));
				}
				else
				{
					ConcludeEvent();
				}
			}
		}
		isTraveling = false;
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
			inkDriver.titleBox = eventCanvas.titleBox;
			inkDriver.textBox = eventCanvas.textBox;
			inkDriver.backgroundUI = eventCanvas.backgroundImage;
			inkDriver.buttonGroup = eventCanvas.buttonGroup;
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
		print("Concluded Event");
		eventInstance.GetComponent<InkDriverBase>().ClearUI();
		
		//in case a random event isn't chosen
		if(eventInstance != null)
		{
			Destroy(eventInstance);
		}

		//Go back to travel scene
		asm.UnloadScene("Event_General");
		AudioManager.instance.PlayMusicWithTransition("General Theme");
		
		//Potentially end the job entirely
		if (overallEventIndex >= maxEvents)
		{
			GameManager.instance.ChangeInGameState(InGameStates.Ending);
		}

		eventActive = false;
	}

	/// <summary>
	/// Decides whether or not an event will run. 
	/// </summary>
	/// <param name="chances">The number of times that the event chance has failed. The higher it is, the more likely an event</param>
	/// <returns></returns>
	private bool WillRunEvent(int chances)
	{
		int rng = Random.Range(1, 5);

		if(rng <= chances)
		{ return true; }
		else
		{ return false; }

		
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
			
			//if the event chosen has requirements that are not met
			if(thisEvent.GetComponent<EventRequirements>() && !thisEvent.GetComponent<EventRequirements>().MatchesRequirements(ship))
			{
				EventRequirements requirements = thisEvent.GetComponent<EventRequirements>();
				
				//copies the current index
				int newIndex = randomEventIndex;
				List<GameObject> newRandomEvents = randomEvents;

				//Copies the list to shuffle until it finds a new event to do or runs out of ideas
				while (requirements.MatchesRequirements(ship) && newIndex != randomEvents.Count)
				{
					//choose an event to check
					int newNum = Random.Range(newIndex, newRandomEvents.Count);
					
					thisEvent = newRandomEvents[newNum];

					//insert this event at the beginning of the list so it cannot be picked again
					newRandomEvents.RemoveAt(eventNum);
					newRandomEvents.Insert(0, thisEvent);

					newIndex++;
				}

				print("There were no other events to run.");
				return null;
			}
			else
			{
				print("Meets requirements");
			}

			randomEvents.RemoveAt(eventNum);
			randomEvents.Insert(0, thisEvent);
		}
		
		return thisEvent;
	}
}
