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
    [SerializeField] private CampaignManager campaignManager;
    [SerializeField] private EventSystem es;

    private JobListUI jobListUI;
    private Job selectedMainJob;
    
    private List<Job> selectedSideJobs = new List<Job>();

    public void RefreshJobList()
    {
        print("here 2");
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
        print("here 3");

        foreach (Job job in campaignManager.campaigns[(int)campaignManager.currentCamp].campaignJobs)
        {
            // Show available job currently handles both primary and side jobs,
            // might need to change when side jobs are added
            if (job.campaignIndexAvailable == campaignManager.campaigns[(int) campaignManager.currentCamp].currentCampaignJobIndex ||
                job.isSideJob)
            {
                jobListUI.ShowAvailableJob(job);
            }
        }
        
        jobListUI.continueButton.onClick.AddListener(FinalizeJobSelection);
    }

    public void SelectJob(Job selected)
    {
        if(!selected.isSideJob)
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
        else
        {
            if (!selectedSideJobs.Contains(selected))
            {
                selectedSideJobs.Add(selected);
            }
            else
            {
                selectedSideJobs.Remove(selected);
            }
            jobListUI.UpdateSideJobCount(selectedSideJobs.Count);
        }
    }

    private void FinalizeJobSelection()
    {
        ship.AddPayout(selectedMainJob.payout);
        es.TakeStoryJobEvents(selectedMainJob);
        es.TakeSideJobEvents(selectedSideJobs);
        campaignManager.campaigns[(int) campaignManager.currentCamp].currentCampaignJobIndex++;
    }
}
