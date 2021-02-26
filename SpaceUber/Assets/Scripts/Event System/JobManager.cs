/*
 * JobSelectScreen.cs
 * Author(s): Scott Acker
 * Created on: 10/19/2020 (en-US)
 * Description: Manages which jobs are available for the player to pick from
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

        for (var i = 0; i < campaignManager.cateringToTheRich.campaignJobs.Count; i++)
        {
            Job thisJob = campaignManager.GetAvailableJobs()[i];
            // Show available job currently handles both primary and side jobs,
            // might need to change when side jobs are added

            if (thisJob.campaignIndexAvailable ==
                campaignManager.GetCurrentCampaignIndex() ||
                thisJob.isSideJob)
            {
                jobListUI.ShowAvailableJob(thisJob, i);
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
        ship.Payout += selectedMainJob.payout;
        //ship.gameObject.GetComponent<ShipStatsUI>().UpdateCreditsUI(ship.Credits, ship.Payout);
        es.TakeStoryJobEvents(selectedMainJob);
        es.TakeSideJobEvents(selectedSideJobs);
    }

    public void EndJob()
    {
        campaignManager.GoToNextJob();
    }
}
