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
    public TMP_Text eventText;
    public GameObject backButton;

    private void Start()
    {
        backButton.SetActive(false);
    }

    public void NextPage()
    {
        if(eventText.pageToDisplay < eventText.textInfo.pageCount)
        {
            eventText.pageToDisplay += 1;
            if(!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
        }
        else
        {
            InkDriverBase inkDriver = FindObjectOfType<InkDriverBase>();
            if(!inkDriver.ShowChoices())
            {
                inkDriver.ConcludeEvent();
            }
        }
    }

    public void PreviousPage()
    {
        if(eventText.pageToDisplay > 1)
        {
            eventText.pageToDisplay -= 1;
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
    }
}
