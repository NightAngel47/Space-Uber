/*
 * JobSelectScreen.cs
 * Author(s): 
 * Created on: 10/19/2020 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class JobSelectScreen : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableJobs;
    private Transform buttonGroup;
    private EventSystem es;

    private JobSelectCanvas jsc;
    private GameObject detailsPanel;

    private TMP_Text titleBox;
    private TMP_Text descriptionBox;
    private TMP_Text payoutBox;

    private Job selectedJob;

    public void Start()
    {
        StartCoroutine(RefreshJobs());
    }

    public IEnumerator RefreshJobs()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName("JobPicker").isLoaded);
        es = GameObject.FindObjectOfType<EventSystem>();
        jsc = GameObject.FindObjectOfType<JobSelectCanvas>();
        titleBox = jsc.titleBox;
        descriptionBox = jsc.descriptionBox;
        payoutBox = jsc.rewardBox;
        detailsPanel = jsc.detailsPanel;
    }

    private void ShowJobs()
    {
        print("Showing jobs");
        foreach (GameObject i in availableJobs)
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
        payoutBox.text = selectedJob.payout + " credits";
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
