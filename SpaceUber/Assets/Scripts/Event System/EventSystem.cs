/*
 * EventSystem.cs
 * Author(s): #Greg Brandt#, Scott Acker, Steven Drovie
 * Created on: 9/17/2020 (en-US)
 * Description:
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
	

	private void Awake()
	{
		//Singleton pattern
		if(instance) { Destroy(gameObject); }
		else { instance = this; }

		ship = FindObjectOfType<ShipStats>();
		asm = FindObjectOfType<AdditiveSceneManager>();
	}

	void SetupEventUI()
	{
		asm.LoadSceneMerged("Event_General");

		eventCanvas = FindObjectOfType<EventCanvas>();
		print(eventCanvas);
	}

	public IEnumerator Travel()
	{
		//float travelTicker = 0;
		
		while (GameManager.currentGameState == InGameStates.Events)
		{
			//int chanceOfEvent = 1;
			//while(!WillRunEvent(chanceOfEvent))
			//{
			//	isTraveling = true;
			//	++chanceOfEvent;
			//	yield return new WaitForSeconds(travelTicTime);

			//}

			isTraveling = true;
			yield return new WaitForSeconds(travelTicTime);

			// Load Event_General Scene for upcoming event
			SetupEventUI();
			
			//Time to decide on an event

			//story events happen every other time 
			if (overallEventIndex % 2 == 1 && overallEventIndex != 0) //if it's an even-numbered event 
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

	public void ConcludeEvent()
	{
		print("Concluded Event");
		eventInstance.GetComponent<InkDriverBase>().ClearUI();
		
		//in case a random event isn't chosen
		if(eventInstance != null)
		{
			Destroy(eventInstance);
		}

		asm.UnloadScene("Event_General");

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
