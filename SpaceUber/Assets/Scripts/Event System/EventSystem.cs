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
using System.Collections.Generic;

public enum EventSystemState { Traveling, Docked}

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	public EventSystemState systemState;
	public List<GameObject> testStoryPrefabs;
	public GameObject canvas;
	GameObject testStoryInstance;
	public Text textBox;

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
		while(systemState == EventSystemState.Traveling)
		{
			isTraveling = true;
			yield return new WaitForSeconds(travelTicTime);
			if (!eventActive && eventIndex < testStoryPrefabs.Count) 
			{
				//prompt an event
				testStoryInstance = Instantiate(testStoryPrefabs[eventIndex]);
				testStoryInstance.transform.SetParent(canvas.transform);
//<<<<<<< Updated upstream
				testStoryInstance.GetComponent<InkDriverBase>().textBox = textBox;
//=======
				testStoryInstance.transform.SetSiblingIndex(0);
				testStoryInstance.GetComponent<RectTransform>().anchoredPosition = new Vector3(-17.11f, 54.87f, 0);

				//testStoryInstance.GetComponent<InkExample>().textBox = textBox;
//>>>>>>> Stashed changes
				eventActive = true;
				eventIndex++;
				while (eventActive) { yield return null; }
			}
		}
		isTraveling = false;
	}

	public void ConcludeEvent()
	{
		Destroy(testStoryInstance);
		eventActive = false;
	}
}