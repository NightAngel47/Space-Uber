/*
 * EventSystem.cs
 * Author(s): #Greg Brandt#
 * Created on: 9/17/2020 (en-US)
 * Description:
 */

using System.Collections;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum EventSystemState { Traveling, Docked}

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	public EventSystemState systemState;
	public List<GameObject> testStoryPrefabs;
	public GameObject canvas;
	GameObject testStoryInstance;
	public TMP_Text titleBox;
	public TMP_Text textBox;

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
		if(systemState == EventSystemState.Traveling && !isTraveling) { StartCoroutine(Travel()); }
	}

	IEnumerator Travel()
	{
		//float travelTicker = 0;


		while (systemState == EventSystemState.Traveling)
		{
			isTraveling = true;
			yield return new WaitForSeconds(travelTicTime);
			if (!eventActive && eventIndex < testStoryPrefabs.Count)
			{
				//prompt an event
				testStoryInstance = Instantiate(testStoryPrefabs[eventIndex], canvas.transform);

				if (testStoryInstance.TryGetComponent(out InkDriverBase inkDriver))
				{
					inkDriver.titleBox = titleBox;
					inkDriver.textBox = textBox;
				}
				testStoryInstance.transform.SetSiblingIndex(0);

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
		testStoryInstance.GetComponent<InkDriverBase>().ClearUI();
		Destroy(testStoryInstance);
		eventActive = false;
		titleBox.text = "Waiting for Event";
	}
}
