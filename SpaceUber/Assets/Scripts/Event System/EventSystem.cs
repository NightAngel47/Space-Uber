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
	[SerializeField] private GameObject canvas;
	[SerializeField] private TMP_Text titleBox;
	[SerializeField] private TMP_Text textBox;
	[SerializeField] private Image backgroundImage;

	GameObject storyEventInstance;
	
	//time inbetween possible event triggers
	float travelTicTime = 5;
	bool isTraveling = false;
	bool eventActive = false;
	
	//how many events have passed. Tells code how many events to ignore at the start of the list
	int eventIndex = 0;
	

	private void Awake()
	{
		//Singleton pattern
		if(instance) { Destroy(gameObject); }
		else { instance = this; }
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
			isTraveling = true;
			yield return new WaitForSeconds(travelTicTime);

			if (!eventActive && eventIndex < randomEvents.Count)
			{
				//prompt a random event
				GameObject newEvent = RandomizeEvent();
				
				if(newEvent != null) //check to be sure a random event was still chosen
				{
					storyEventInstance = Instantiate(newEvent, canvas.transform);

					if (storyEventInstance.TryGetComponent(out InkDriverBase inkDriver))
					{
						inkDriver.titleBox = titleBox;
						inkDriver.textBox = textBox;
						inkDriver.backgroundUI = backgroundImage;
					}

					eventActive = true;
					eventIndex++;
					yield return new WaitWhile((() => eventActive));
				}
				else
				{

				}
				
			}

			////Some small animation to show time is ticking
			//++travelTicker;
			//if(travelTicker % 30 == 0 && travelTicker != 180)
			//{
			//	titleBox.text += ".";
			//}
			//else if(travelTicker == 12)
			//{
			//	titleBox.text = "Waiting for Event";
			//}
		}
		isTraveling = false;
	}

	public void ConcludeEvent()
	{
		print("Concluded Event");
		storyEventInstance.GetComponent<InkDriverBase>().ClearUI();
		Destroy(storyEventInstance);
		eventActive = false;
		titleBox.text = waitMessage;
		textBox.text = ""; // make sure that the text has been cleared.

		if (eventIndex >= maxEvents)
		{
			GameManager.instance.LoadScene("PromptScreen");
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
		GameObject thisEvent = randomEvents[eventIndex];

		if (eventIndex != randomEvents.Count)
		{
			int eventNum = Random.Range(eventIndex, randomEvents.Count);
			thisEvent = randomEvents[eventNum];
			
			//if the event chosen has requirements that are not met
			if(thisEvent.GetComponent<EventRequirements>() && !thisEvent.GetComponent<EventRequirements>().MatchesRequirements(ship))
			{
				EventRequirements requirements = thisEvent.GetComponent<EventRequirements>();
				
				//copies the current index
				int newIndex = eventIndex;
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
