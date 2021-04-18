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

    private List<string> mySFX;
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

            

            if (mySFX.Count >= eventText.pageToDisplay)
            {
                if (mySFX[eventText.pageToDisplay] != null)
                {
                    AudioManager.instance.PlaySFX(mySFX[eventText.pageToDisplay - 1]);
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

            if (mySFX.Count >= eventText.pageToDisplay)
            {
                if (mySFX[eventText.pageToDisplay] != null)
                {
                    AudioManager.instance.PlaySFX(mySFX[eventText.pageToDisplay - 1]);
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
        if (!inkDriver) inkDriver = FindObjectOfType<InkDriverBase>();
        int currentPage = eventText.pageToDisplay;

        //cuts list to keep pace with the array
        if(mySFX.Count < 1) //first pass cuts the original list from idb
        {
            mySFX = inkDriver.pageClips;
        }
        else //subsequent passes cut the current list
        {
            mySFX = mySFX.GetRange(currentPage + 1, inkDriver.pageClips.Count);
            //plus one, so we don't keep the SFX just played
        }

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
