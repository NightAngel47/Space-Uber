/*
 * JobSelectCanvas.cs
 * Author(s): Scott Acler
 * Created on: 10/20/2020 (en-US)
 * Description: Stores UI information about which text boxes and panels should be used by JobManager
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobListUI : MonoBehaviour
{
    public Button continueButton;
    
    [Header("Primary Job UI")]
    [SerializeField] private GameObject primaryJob;
    [SerializeField] private Transform primaryJobUIPos;
    [SerializeField] private TMP_Text selectedJobNameText;
    [SerializeField] private TMP_Text selectedJobDescText;
    [SerializeField] private TMP_Text selectedJobPayText;
    private List<JobUI> availablePrimaryJobUIs = new List<JobUI>();

    [Header("Side Job UI")]
    [SerializeField] private GameObject sideJob;
    [SerializeField] private Transform sideJobUIPos;
    [SerializeField] private TMP_Text sideJobCountText; // TODO Not fully hooked up as side jobs are not in

    private JobUI selectedPrimaryJobUI;

    private void Start()
    {
        SpawnObject.donePreplacedRoom = false;
        ClearSelectedJobDetails();
    }

    /// <summary>
    /// Instantiates ui for the available job passed in. 
    /// Currently handles both primary and side jobs,
    /// might need to change when side jobs are added
    /// </summary>
    /// <param name="job">Job to display</param>
    public void ShowAvailableJob(Job job)
    {
        JobUI newJobUI = job.isSideJob ? Instantiate(sideJob, sideJobUIPos).GetComponent<JobUI>() : 
                                        Instantiate(primaryJob, primaryJobUIPos).GetComponent<JobUI>();
        newJobUI.SetJobInfo(job);

        if (!job.isSideJob)
        {
            availablePrimaryJobUIs.Add(newJobUI);
        }
    }

    public void UpdateSideJobCount(int x)
    {
        sideJobCountText.text = x + " out of 3 side jobs selected";
        print(sideJobCountText.text);
    }

    /// <summary>
    /// Updates the UI with the details of the selected job.
    /// </summary>
    /// <param name="selectedJob">The selected job</param>
    public void ShowSelectedJobDetails(Job selectedJob)
    {
        selectedJobNameText.text = selectedJob.jobName;
        selectedJobDescText.text = selectedJob.description;
        selectedJobPayText.text = selectedJob.payout + " Credits";

        // clears background selection for previously selected primary jobs. (only works if primary jobs are unique)
        foreach (var jobUI in availablePrimaryJobUIs.Where(jobUI => jobUI.availableJob != selectedJob))
        {
            jobUI.ClearSelectedBackground();
        }

        continueButton.interactable = true;
    }

    /// <summary>
    /// Clears UI of any selected primary job
    /// </summary>
    public void ClearSelectedJobDetails()
    {
        selectedJobNameText.text = "";
        selectedJobDescText.text = "";
        selectedJobPayText.text = "";
        
        continueButton.interactable = false;
    }
}
