/* Frank Calabrese
 * 3/6/21
 * Tutorial.cs
 * Singleton class responsible for holding and tracking all tutorials. 
 * Will display specified tutorial when setCurrentTutorial(tutorial ID) is called
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialNode
{
    public string tutorialName;
    public string[] tutorialMessages;
    public bool tutorialFinished; 
}

public class Tutorial : Singleton<Tutorial>
{
    [SerializeField] TutorialNode[] tutorials = new TutorialNode[10];
    [SerializeField] TextMeshProUGUI tutorialTextbox;
    [SerializeField] TextMeshProUGUI tutorialTitleTextbox;
    [SerializeField] GameObject tutorialPanel;

    [SerializeField] GameObject highlightPanel;
    [SerializeField] GameObject ghostCursor;

    
    [SerializeField] float timeStartedLerping;
    [SerializeField] float lerpTime;
    private bool lerping = false;
    private float percentageComplete;
    private Vector3 lerpStart;
    private Vector3 lerpEnd;

    //location index for ghostCursor
    //private Vector3 vecInsideShip = new Vector3(500, 300, 0);
    //private Vector3 vecShopPanel = new Vector3(200, 450, 0);
    //private Vector3 vecStatsLeft = new Vector3(200, 600, 0);
    //private Vector3 vecStatsRight = new Vector3(800, 600, 0);

    [SerializeField] GameObject vecInsideShip;
    [SerializeField] GameObject vecShopPanel;
    [SerializeField] GameObject vecStatsLeft;
    [SerializeField] GameObject vecStatsRight;


    private TutorialNode currentTutorial;
    private int index;
    private bool tutorialPrerequisitesComplete = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))// && tutorialPanel.activeSelf == true)
        {
            continueButton();
        }

        //dirty implementation of ghost cursor until I get it working good

        //Shipbuilding Message 0
        if (tutorialPanel.activeSelf == true && lerping == false && currentTutorial == tutorials[1] && currentTutorial.tutorialMessages[0] == currentTutorial.tutorialMessages[index - 1])
        {
            if(tutorialPrerequisitesComplete == false)
            {
                //FindObjectOfType<RoomPanelToggle>().OpenPanel();
                FindObjectOfType<ShipBuildingShop>().ToResourceTab("Food");
                tutorialPrerequisitesComplete = true;
            }
            

            BeginLerping(vecShopPanel.transform.position, vecInsideShip.transform.position);
        }
        //Shipbuilding Message 4
        if (tutorialPanel.activeSelf == true && lerping == false && currentTutorial == tutorials[1] && currentTutorial.tutorialMessages[4] == currentTutorial.tutorialMessages[index - 1]) BeginLerping(vecStatsLeft.transform.position, vecStatsRight.transform.position);
        //////////////////////////////////////////////////////////////////

        if (lerping)
        {
            ghostCursor.transform.position = LerpGhostCursor(lerpStart, lerpEnd, timeStartedLerping, lerpTime);

            if (ghostCursor.transform.position == lerpEnd)
            {
                lerping = false;
                BeginLerping(lerpStart, lerpEnd);
            }
        }





    }


    //call this to display a tutorial. Tutorial IDs can be found in the inspector
    public void setCurrentTutorial(int tutorialID, bool forcedTutorial)
    {
        //if you're already in a tutorial, stop.
        if (tutorialPanel.activeSelf == true) return;
        //if the game tries to force a tutorial the player has already seen, stop.
        if (tutorials[tutorialID].tutorialFinished == true && forcedTutorial == true) return;

        currentTutorial = tutorials[tutorialID];

        if (tutorialPanel.activeSelf == false)
        {
            tutorialPanel.SetActive(true);
            tutorialTextbox.text = currentTutorial.tutorialMessages[0];
            tutorialTitleTextbox.text = currentTutorial.tutorialName;
            index = 1;
        }

    }
    
    public void closeCurrentTutorial()
    {
        if (tutorialPanel.activeSelf == true)
        {
            highlightPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            index = 0;

            currentTutorial.tutorialFinished = true;
        }
    }

    //X and Y of center of box, with (0,0) being screen center. Width and height of box
    public void highlightScreenLocation(int xPos, int yPos, int width, int height)
    {
        highlightPanel.SetActive(true);
        RectTransform loc = highlightPanel.GetComponent<RectTransform>();
        loc.sizeDelta = new Vector2(width, height);
        loc.localPosition = new Vector3(xPos, yPos, 1);
    }

    public void continueButton()
    {
        if (tutorialPanel.activeSelf == true)
        {
            StopLerping();
            tutorialPrerequisitesComplete = false;

            if (index < currentTutorial.tutorialMessages.Length)
            {
                tutorialTextbox.text = currentTutorial.tutorialMessages[index];
                index++;
            }
            else closeCurrentTutorial();
        }
    }

    public void BeginLerping(Vector3 start, Vector3 end)
    {
        ghostCursor.SetActive(true);

        lerpStart = start;
        lerpEnd = end;

        timeStartedLerping = Time.time;
        lerping = true;
    }
    public void StopLerping()
    {
        lerping = false;

        ghostCursor.SetActive(false);
    }

    public Vector3 LerpGhostCursor(Vector3 start, Vector3 end, float timeStarted, float ltime = 1)
    {

        timeStarted = Time.time - timeStarted;
        percentageComplete = timeStarted / ltime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;


    }

}
