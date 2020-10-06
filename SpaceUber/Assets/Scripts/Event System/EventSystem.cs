/*
 * EventSystem.cs
 * Author(s): #Greg Brandt#, Scott Acker, Steven Drovie
 * Created on: 9/17/2020 (en-US)
 * Description:
 */

using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum EventSystemState { Traveling, Docked}

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	public EventSystemState systemState;
	public ShipStats ship;

	//how many events will happen in this journey
	public int maxEvents = 3;
	[SerializeField] private string waitMessage = "Wait for Next Event";
	[SerializeField] private List<GameObject> storyEvents;
	[SerializeField] private List<GameObject> randomEvents;
	[HideInInspector] [SerializeField] private GameObject canvas;
	[HideInInspector] [SerializeField] private TMP_Text titleBox;
	[HideInInspector] [SerializeField] private TMP_Text textBox;
	[HideInInspector] [SerializeField] private UnityEngine.UI.Image backgroundImage;

	[HideInInspector] [SerializeField] public Transform buttonGroup;

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

		canvas = GameObject.FindGameObjectWithTag("Canvas");
		titleBox = GameObject.FindGameObjectWithTag("Header").GetComponent<TMP_Text>();
		textBox = GameObject.FindGameObjectWithTag("Body").GetComponent<TMP_Text>();
		backgroundImage = GameObject.FindGameObjectWithTag("Background").GetComponent<UnityEngine.UI.Image>();
		buttonGroup = GameObject.FindGameObjectWithTag("ButtonGroup").transform;

		titleBox.text = waitMessage;
		textBox.text = ""; // make sure that the text has been cleared.
	}

	private void Update()
	{
		if (systemState == EventSystemState.Traveling && !isTraveling)
		{
			print("Starting Travel Coroutine");
			StartCoroutine(Travel());
		}
	}

	IEnumerator Travel()
	{
		//float travelTicker = 0;
		
		while (systemState == EventSystemState.Traveling)
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

			//Time to decide on an event
			//story events happen every other time
			if (storyEventIndex % 2 == 0 && storyEventIndex != storyEvents.Count) 
			{
				eventInstance = Instantiate(storyEvents[storyEventIndex], canvas.transform);

				if (eventInstance.TryGetComponent(out InkDriverBase inkDriver))
				{
					inkDriver.titleBox = titleBox;
					inkDriver.textBox = textBox;
					inkDriver.backgroundUI = backgroundImage;
					inkDriver.choicesPos = buttonGroup;
				}

				eventActive = true;
				storyEventIndex++;
				overallEventIndex++;
				yield return new WaitWhile((() => eventActive));
				
			}
			else if (!eventActive && randomEventIndex < randomEvents.Count) //Pick a random event
			{
				GameObject newEvent = RandomizeEvent();
				
				if(newEvent != null) //check to be sure a random event was still chosen
				{
					eventInstance = Instantiate(newEvent, canvas.transform);

					if (eventInstance.TryGetComponent(out InkDriverBase inkDriver))
					{
						inkDriver.titleBox = titleBox;
						inkDriver.textBox = textBox;
						inkDriver.backgroundUI = backgroundImage;
					}

					eventActive = true;
					randomEventIndex++;
					overallEventIndex++;
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

	public void ConcludeEvent()
	{
		print("Concluded Event");
		eventInstance.GetComponent<InkDriverBase>().ClearUI();
		
		//in case a random event isn't chosen
		if(eventInstance != null)
		{
			Destroy(eventInstance);
		}

		eventActive = false;
		titleBox.text = waitMessage;
		textBox.text = ""; // make sure that the text has been cleared.

		if (overallEventIndex >= maxEvents)
		{
			GameManager.instance.LoadScene("PromptScreen");
		}
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
