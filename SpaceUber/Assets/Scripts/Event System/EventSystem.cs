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

	[SerializeField] private string waitMessage = "Wait for Next Event";
	[SerializeField] private List<GameObject> storyEvents;
	[SerializeField] private GameObject canvas;
	[SerializeField] private TMP_Text titleBox;
	[SerializeField] private TMP_Text textBox;
	[SerializeField] private Image backgroundImage;

	GameObject storyEventInstance;
	
	//time inbetween possible event triggers
	float travelTicTime = 5;
	bool isTraveling = false;
	bool eventActive = false;
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
			
			if (!eventActive && eventIndex < storyEvents.Count)
			{
				//prompt an event
				storyEventInstance = Instantiate(RandomizeEvent(), canvas.transform);

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

		if (eventIndex >= storyEvents.Count)
		{
			GameManager.instance.LoadScene("PromptScreen");
		}
	}

	/// <summary>
	/// Picks a random event to spawn. The random numbers include the eventIndex to the count of storyEvents list
	/// When an event is picked, it is moved to the start of the list and cannot be picked again.
	/// </summary>
	/// <returns></returns>
	private GameObject RandomizeEvent()
	{
		GameObject thisEvent = storyEvents[eventIndex];
		print("Event index is " + eventIndex);

		if (eventIndex != storyEvents.Count)
		{
			int eventNum = Random.Range(eventIndex, storyEvents.Count);
			thisEvent = storyEvents[eventNum];
			
			if(thisEvent.GetComponent<EventRequirements>())
			{
				//maximum number of events still available
				int counter = eventIndex;

				//Copies the list to shuffle until it finds a new event to do or runs out of ideas
				while(!thisEvent.GetComponent<EventRequirements>().MatchesRequirements(ship) && counter != storyEvents.Count)
				{
					int newNum = Random.Range(counter, storyEvents.Count);
					List<GameObject> newStoryEvents = storyEvents;
					thisEvent = newStoryEvents[newNum];

					newStoryEvents.RemoveAt(eventNum);
					newStoryEvents.Insert(0, thisEvent);

					counter++;
				}

				if(counter == storyEvents.Count)
				{
					print("There were no other events to run.");
				}
			}

			storyEvents.RemoveAt(eventNum);
			storyEvents.Insert(0, thisEvent);
		}
		
		return thisEvent;
	}
}
