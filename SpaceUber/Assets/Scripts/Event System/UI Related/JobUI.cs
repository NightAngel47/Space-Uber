/*
 * JobUI.cs
 * Author(s): Steven Drovie []
 * Created on: 11/4/2020 (en-US)
 * Description: 
 */

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobUI : MonoBehaviour
{
    public Job availableJob { get; private set; }
    private JobManager jobManager;
    
    [SerializeField] private TMP_Text jobNameText;
    [SerializeField] private TMP_Text jobNameTextOutline;
    [SerializeField] private Image jobImage;
    [SerializeField] private GameObject jobSelectedImageOutline;
    [SerializeField] private GameObject jobNotSelectedImageOverlay;
    [SerializeField] private float jobTextSelectedIncrease = 0.25f;

    [SerializeField] private bool isSideJob = false;
    [SerializeField, ShowIf("isSideJob")] private TMP_Text sideJobPayText;
    [SerializeField, ShowIf("isSideJob")] private TMP_Text sideJobLengthText;

    private bool isSelected;
    
    public string[] jobSelectSFX;

    private void Start()
    {
        jobManager = FindObjectOfType<JobManager>();
    }

    public void SetJobInfo(Job job)
    {
        availableJob = job;
        
        jobNameText.text = job.jobName;
        jobNameTextOutline.text = job.jobName;

        jobImage.sprite = job.jobListImage;
        
        if (isSideJob)
        {
            sideJobPayText.text = job.payout.ToString() + " Credits";

            switch (job.maxRandomEvents)
            {
                //TODO add other cases for other side job lengths
                default:
                    sideJobLengthText.text = "Length: +" + job.maxRandomEvents + " Event(s)";
                    Debug.LogWarning("UI not setup for side job length");
                    break;
            }
        }
    }

    public void JobSelected()
    {
        jobManager.SelectJob(availableJob);
        AudioManager.instance.PlaySFX(jobSelectSFX[Random.Range(0, jobSelectSFX.Length - 1)]);
        
        if (!isSelected)
        {
            isSelected = true;
            
            // update text
            jobNameText.transform.localScale = Vector3.one * (1 + jobTextSelectedIncrease);
            jobNameTextOutline.transform.localScale = Vector3.one * (1 + jobTextSelectedIncrease);
            jobNameTextOutline.gameObject.SetActive(true);
        
            jobSelectedImageOutline.SetActive(true);
        }
        else
        {
            isSelected = false;
            
            // update text
            jobNameText.transform.localScale = Vector3.one;
            jobNameTextOutline.transform.localScale = Vector3.one;
            jobNameTextOutline.gameObject.SetActive(false);
        
            jobSelectedImageOutline.SetActive(false);

            foreach (var jobUI in FindObjectsOfType<JobUI>())
            {
                jobUI.GetComponent<Button>().interactable = true;
                jobUI.jobNotSelectedImageOverlay.SetActive(false);
            }
        }
    }

    public void JobNotSelected()
    {
        if (!isSelected)
        {
            jobNotSelectedImageOverlay.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
    }
}
