/*
 * NextPage.cs
 * Author(s): Sam Ferstein
 * Created on: 12/2/2020 (en-US)
 * Description: This controls the page movement of the text boxes for events.
 */

using UnityEngine;
using TMPro;
using DG.Tweening;

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
    private int furthestPage = 1;

    private void Start()
    {
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
        ResetPages();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }

    public void NextPage()
    {
        InkDriverBase inkDriver = FindObjectOfType<InkDriverBase>();
        if (!inkDriver.donePrinting)
        {
            inkDriver.textMask.DOComplete();
            inkDriver.donePrinting = true;
        }
        else if (eventText.pageToDisplay < eventText.textInfo.pageCount && inkDriver.donePrinting)
        {
            eventText.pageToDisplay += 1;
            if (eventText.pageToDisplay >= furthestPage)
            {
                furthestPage++;
                inkDriver.textMask.fillAmount = 0;
                inkDriver.FillBox();
            }
            else
            {
                inkDriver.textMask.fillAmount = 1;
            }
            if (!backButton.activeSelf)
            {
                backButton.SetActive(true);
            }
        }
        else
        {
            if(inkDriver && !inkDriver.ShowChoices()) // normal
            {
                inkDriver.ConcludeEvent();
            }
            else if(!inkDriver && FindObjectOfType<GameIntroManager>()) // game intro
            {
                GameIntroManager intro = FindObjectOfType<GameIntroManager>();
                intro.ConcludeEvent();
            }
            else if(FindObjectOfType<MoneyEndingBehaviour>()) // endings
            {
                FindObjectOfType<MoneyEndingBehaviour>().TransitionToScene();
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
            inkDriver.textMask.DOComplete();
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
        if(!nextButtonText)
            nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
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
