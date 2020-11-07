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
    private Job selectedMainJob;
    private List<Job> selectedSideJobs;
    
    private EventSystem es;

    public void Start()
    {
        es = FindObjectOfType<EventSystem>();
        
        StartCoroutine(UpdateJobList());
    }

    /// <summary>
    /// Once the Job Picker scene is loaded, the manager finds all UI, creates buttons for each job, and adds listeners to them
    /// </summary>
    /// <returns></returns>
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
            FinalizeJobSelection(selectedMainJob);
        });
    }

    public void SelectJob(Job selected)
    {
        if (selected != selectedMainJob)
        {
            selectedMainJob = selected;
            jobListUI.ShowSelectedJobDetails(selectedMainJob);
        }
        else
        {
            selectedMainJob = null;
            jobListUI.ClearSelectedJobDetails();
        }
    }

    private void FinalizeJobSelection(Job chosen)
    {
        ship.AddPayout(selectedMainJob.payout);
        es.TakeStoryEvents(chosen);
    }
}
