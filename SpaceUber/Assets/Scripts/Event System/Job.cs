
/*
 * Job.cs
 * Author(s): Scott Acker
 * Created on: 10/16/2020 (en-US)
 * Description: Stores information about a particular job. Intended to be a prefab
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Job : MonoBehaviour
{
    public List<GameObject> events;

    public string jobName;
    public string description;

    public Button buttonPrefab;
    public Transform buttonGroup;
    private EventSystem es;

    public void Start()
    {
        es = GameObject.FindObjectOfType<EventSystem>();
    }

    public void ShowButton()
    {
        Button thisButton = Instantiate(buttonPrefab, buttonGroup);

        // Set listener
        thisButton.onClick.AddListener(delegate {
            AssignJobs();
        });
    }

    /// <summary>
    /// Assigns this jobs events to the event system
    /// </summary>
    void AssignJobs()
    {
        es.TakeEvents(this);
    }


}