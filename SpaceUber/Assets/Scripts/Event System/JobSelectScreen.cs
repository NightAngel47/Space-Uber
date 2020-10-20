/*
 * JobSelectScreen.cs
 * Author(s): 
 * Created on: 10/19/2020 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobSelectScreen : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableJobs;
    [SerializeField] private Transform buttonGroup;
    private EventSystem es;

    public GameObject detailsPanel;

    private TMP_Text titleBox;
    private TMP_Text descriptionBox;
    private TMP_Text rewardBox;

    private Job selectedJob;

    public void Start()
    {
        es = GameObject.FindObjectOfType<EventSystem>();

        titleBox = GameObject.FindGameObjectWithTag("Header").GetComponent<TMP_Text>();
        descriptionBox = GameObject.FindGameObjectWithTag("Body").GetComponent<TMP_Text>();
        rewardBox = GameObject.FindGameObjectWithTag("Misc Text").GetComponent<TMP_Text>();

        HideDetails();
        ShowJobs();
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

    public void ShowDetails(Job selectedJob)
    {
        detailsPanel.SetActive(true);
        titleBox.text = selectedJob.jobName;
        descriptionBox.text = selectedJob.description;
        rewardBox.text = selectedJob.payout + " credits";
    }
    public void HideDetails()
    {
        detailsPanel.SetActive(false);
    }

    public void SelectJob(Job selected)
    {
        selectedJob = selected;
        ShowDetails(selectedJob);
    }

    public void ChoiceChosen(Job chosen)
    {
        es.TakeEvents(chosen);
        foreach (var button in buttonGroup.transform.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }

        foreach (var text in transform.GetComponentsInChildren<TMP_Text>())
        {
            Destroy(text.gameObject);
        }
    }
}
