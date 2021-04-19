/*
 * NextPage.cs
 * Author(s): Sam Ferstein
 * Created on: 12/2/2020 (en-US)
 * Description: This controls the page movement of the text boxes for events.
 */

using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
    int currentPage = 0; //the overall current page, used for the page clips

    private void Start()
    {
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
        currentPage = 0;
        //ResetPages();
        
    }

    private void PlayCurrentPageSFX()
    {
        if(currentPage != 1) //doesn't need to play on the first page of the event
        {
            //uses currentPage to figure out which SFX to play
            if (!inkDriver) inkDriver = FindObjectOfType<InkDriverBase>();
            
            if (inkDriver.pageClips.Count >= currentPage)
            {
                if (inkDriver.pageClips[currentPage - 1] != null) //make sure there is something in that slot
                {
                    AudioManager.instance.PlaySFX(inkDriver.pageClips[currentPage - 1]);

                }
            }
        }
        
    }

    public void NextPage()
    {
        
        //if there are still pages to display in this set
        if (eventText.pageToDisplay < eventText.textInfo.pageCount)
        {
            eventText.pageToDisplay += 1; //increment current page
            
            //back button is not active
            if(!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
            currentPage++;
            PlayCurrentPageSFX();
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
            currentPage--;

            PlayCurrentPageSFX();

        }
        else
        {
            backButton.SetActive(false);
        }
    }

    public void ResetPages()
    {
        currentPage++;
        eventText.pageToDisplay = 1;
        backButton.SetActive(false);
        nextButtonText.text = defaultNextMsg;
        PlayCurrentPageSFX();
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
