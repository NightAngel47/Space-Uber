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
    [SerializeField] private List<Job> availableJobs;
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
        es = FindObjectOfType<EventSystem>();
        
        StartCoroutine(RefreshJobs());
    }

    private IEnumerator RefreshJobs()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName("JobPicker").isLoaded);
        jsc = FindObjectOfType<JobSelectCanvas>();
        
        titleBox = jsc.titleBox;
        descriptionBox = jsc.descriptionBox;
        payoutBox = jsc.rewardBox;
        detailsPanel = jsc.detailsPanel;
        buttonGroup = jsc.buttonGroup;

        jsc.confirmButton.onClick.AddListener(delegate {
            ChoiceChosen(selectedJob);
        });

        HideDetails();
        ShowJobs();


    }

    private void ShowJobs()
    {
        foreach (Job job in availableJobs)
        {
            job.jobSelect = this;
            job.ShowButton(buttonGroup);
        }
    }

    private void ShowDetails()
    {
        detailsPanel.SetActive(true);
        titleBox.text = selectedJob.jobName;
        descriptionBox.text = selectedJob.description;
        payoutBox.text = selectedJob.payout + " credits";
    }

    private void HideDetails()
    {
        detailsPanel.SetActive(false);
    }

    public void SelectJob(Job selected)
    {
        selectedJob = selected;
        ShowDetails();
    }

    private void ChoiceChosen(Job chosen)
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
