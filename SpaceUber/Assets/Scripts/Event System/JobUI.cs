/*
 * JobUI.cs
 * Author(s): Steven Drovie []
 * Created on: 11/4/2020 (en-US)
 * Description: 
 */

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobUI : MonoBehaviour, IPointerClickHandler 
{
    public Job availableJob { get; private set; }
    private JobManager jobManager;
    
    [SerializeField] private TMP_Text jobNameText;
    [SerializeField] private Image selectBgImg;

    [SerializeField] private bool isSideJob;
    [SerializeField, ShowIf("isSideJob")] private TMP_Text sideJobPayText;
    [SerializeField, ShowIf("isSideJob")] private TMP_Text sideJobLengthText;

    public string[] jobSelectSFX;

    private void Start()
    {
        jobManager = FindObjectOfType<JobManager>();
    }

    public void SetJobInfo(Job job)
    {
        availableJob = job;
        
        jobNameText.text = job.jobName;
        
        if (isSideJob)
        {
            sideJobPayText.text = job.payout.ToString() + " Credits";

            switch (job.maxEvents)
            {
                //TODO add other cases for other side job lengths
                default:
                    sideJobLengthText.text = "Length: +" + job.maxEvents + " Event(s)";
                    Debug.LogWarning("UI not setup for side job length");
                    break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectBgImg.enabled = !selectBgImg.enabled;
        jobManager.SelectJob(availableJob);
        AudioManager.instance.PlaySFX(jobSelectSFX[Random.Range(0, jobSelectSFX.Length - 1)]);
    }

    public void ClearSelectedBackground()
    {
        selectBgImg.enabled = false;
    }
}
