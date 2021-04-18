/*
 * NextPage.cs
 * Author(s): Sam Ferstein
 * Created on: 12/2/2020 (en-US)
 * Description: This controls the page movement of the text boxes for events.
 */

using UnityEngine;
using TMPro;

public class PageController : MonoBehaviour
{
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private string defaultNextMsg;
    [SerializeField] private string continueNextMsg;
    private bool madeChoice;
    private TMP_Text nextButtonText;
    InkDriverBase inkDriver;
    private void Start()
    {
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
        ResetPages();
        
    }

    public void NextPage()
    {
        //if there are still pages to display
        if (eventText.pageToDisplay < eventText.textInfo.pageCount)
        {
            eventText.pageToDisplay += 1; //increment current page
            
            //back button is not active
            if(!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
            if(!inkDriver) inkDriver = FindObjectOfType<InkDriverBase>();
            if (inkDriver.pageClips.Count >= eventText.pageToDisplay)
            {
                if (inkDriver.pageClips[eventText.pageToDisplay] != null)
                {
                    AudioManager.instance.PlaySFX(inkDriver.pageClips[eventText.pageToDisplay]);
                }
            }
        }
        else
        {
            if (!inkDriver) inkDriver = FindObjectOfType<InkDriverBase>();

            if (!inkDriver.ShowChoices()) //if no choices remain
            {
                inkDriver.ConcludeEvent();
            }
        }

        if (madeChoice && eventText.pageToDisplay == eventText.textInfo.pageCount)
        {
            nextButtonText.text = continueNextMsg;
        }
    }

    public void PreviousPage()
    {
        if(eventText.pageToDisplay > 1)
        {
            eventText.pageToDisplay -= 1;
            nextButtonText.text = defaultNextMsg;
            if(eventText.pageToDisplay == 1)
            {
                backButton.SetActive(false);
            }
            if (!inkDriver) inkDriver = FindObjectOfType<InkDriverBase>();
            if (inkDriver.pageClips.Count >= eventText.pageToDisplay)
            {
                if (inkDriver.pageClips[eventText.pageToDisplay] != null)
                {
                    AudioManager.instance.PlaySFX(inkDriver.pageClips[eventText.pageToDisplay]);
                }
            }

        }
        else
        {
            backButton.SetActive(false);
        }
    }

    public void ResetPages()
    {
        eventText.pageToDisplay = 1;
        backButton.SetActive(false);
        nextButtonText.text = defaultNextMsg;
    }

    public void UpdateNextPageText()
    {
        madeChoice = true;
        if (madeChoice && eventText.pageToDisplay == eventText.textInfo.pageCount)
        {
            nextButtonText.text = continueNextMsg;
        }
    }
}
