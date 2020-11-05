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

public class JobManager : MonoBehaviour
{
    [SerializeField] private ShipStats ship;
    [SerializeField] private List<Job> availableJobs;
    private JobListUI jobListUI;
    private Job selectedJob;
    
    private EventSystem es;

    public void Start()
    {
        es = FindObjectOfType<EventSystem>();
        
        StartCoroutine(UpdateJobList());
    }

    private IEnumerator UpdateJobList()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Interface_JobList").isLoaded);
        jobListUI = FindObjectOfType<JobListUI>();

        foreach (var job in availableJobs)
        {
            // Show available job currently handles both primary and side jobs,
            // might need to change when side jobs are added
            jobListUI.ShowAvailableJob(job);
        }
        
        jobListUI.continueButton.onClick.AddListener(delegate {
            ChoiceChosen(selectedJob);
        });
    }

    public void SelectJob(Job selected)
    {
        if (selected != selectedJob)
        {
            selectedJob = selected;
            jobListUI.ShowSelectedJobDetails(selectedJob);
        }
        else
        {
            selectedJob = null;
            jobListUI.ClearSelectedJobDetails();
        }
    }

    private void ChoiceChosen(Job chosen)
    {
        ship.AddPayout(selectedJob.payout);
        es.TakeEvents(chosen);
    }
}
