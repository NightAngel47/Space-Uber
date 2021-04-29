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
    //private bool pageSeen = false;
    private TMP_Text nextButtonText;

    private void Start()
    {
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
        ResetPages();
    }

    public void NextPage()
    {
        InkDriverBase inkDriver = FindObjectOfType<InkDriverBase>();
        if (eventText.pageToDisplay < eventText.textInfo.pageCount)
        {
            eventText.pageToDisplay += 1;
            inkDriver.textMask.fillAmount = 0;
            //if(!pageSeen)
            //{

            //}
            if (!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
        }
        else
        {
            if (!inkDriver.ShowChoices())
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
        InkDriverBase inkDriver = FindObjectOfType<InkDriverBase>();
        if (eventText.pageToDisplay > 1)
        {
            eventText.pageToDisplay -= 1;
            //pageSeen = true;
            inkDriver.textMask.fillAmount = 1;
            nextButtonText.text = defaultNextMsg;
            if(eventText.pageToDisplay == 1)
            {
                backButton.SetActive(false);
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
