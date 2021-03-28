/*
 * NextPage.cs
 * Author(s): Sam Ferstein
 * Created on: 12/2/2020 (en-US)
 * Description: This controls the page movement of the text boxes for events.
 */

using System.Collections;
using UnityEngine;
using TMPro;

public class PageController : MonoBehaviour
{
    public TMP_Text eventText;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private string defaultNextMsg;
    [SerializeField] private string continueNextMsg;
    private bool madeChoice;
    private TMP_Text nextButtonText;

    private InkDriverBase inkDriver;

    private IEnumerator Start()
    {
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
        ResetPages();

        // wait for ink driver to be loaded
        yield return new WaitUntil(() => FindObjectOfType<InkDriverBase>());
        inkDriver = FindObjectOfType<InkDriverBase>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }

    public void NextPage()
    {
        // next page
        if (inkDriver.isAtPageLimit)
        {
            // conclude event
            if (inkDriver.donePrinting || madeChoice)
            {
                inkDriver.ConcludeEvent();
            }
            print("ere");
            StartCoroutine(inkDriver.PrintText());
            
            if (!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
        }

        
    }

    public void PreviousPage()
    {
        if(eventText.pageToDisplay > 1)
        {
            StartCoroutine(inkDriver.PrintText(true));
            
            eventText.pageToDisplay -= 1;
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

    public void UpdateMadeChoice()
    {
        madeChoice = true;
        nextButtonText.text = continueNextMsg;
    }
}
