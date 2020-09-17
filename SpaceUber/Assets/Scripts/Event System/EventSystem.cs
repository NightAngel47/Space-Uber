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

public enum EventSystemState { Traveling, Docked}

public class EventSystem : MonoBehaviour
{
	public static EventSystem instance;
	public EventSystemState systemState;
	public GameObject testStoryPrefab;
	public GameObject canvas;
	GameObject testStoryInstance;
	public Text textBox;

	//time inbetween possible event triggers
	float travelTicTime = 5;
	bool isTraveling = false;
	bool eventActive = false;

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
			if (!eventActive) 
			{ 
				testStoryInstance = Instantiate(testStoryPrefab);
				testStoryInstance.transform.SetParent(canvas.transform);
				testStoryInstance.GetComponent<InkExample>().textBox = textBox;
				eventActive = true;
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