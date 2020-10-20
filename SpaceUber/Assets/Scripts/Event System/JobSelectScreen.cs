/*
 * JobSelectScreen.cs
 * Author(s): 
 * Created on: 10/19/2020 (en-US)
 * Description: 
 */

using Boo.Lang;
using UnityEngine;

public class JobSelectScreen : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableJobs;
    [SerializeField] private Transform buttonGroup;
    private EventSystem es;

    public void Start()
    {
        es = GameObject.FindObjectOfType<EventSystem>();
    }

    private void ShowJobs()
    {
        foreach(GameObject i in availableJobs)
        {
            Job thisJob = i.GetComponent<Job>();
            thisJob.jobSelect = this;
            thisJob.ShowButton(buttonGroup);
        }
    }

    public void ChoiceChosen(Job chosen)
    {
        es.TakeEvents(chosen);
        foreach(GameObject button in buttonGroup.transform)
        {
            Destroy(button);
        }
    }
}
