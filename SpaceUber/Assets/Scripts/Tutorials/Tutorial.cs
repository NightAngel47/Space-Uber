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
using NaughtyAttributes;

[System.Serializable]
public class TutorialNode
{
    public string tutorialName;
    public TutorialMessage[] tutorialMessages;
    public bool tutorialFinished;
}

[System.Serializable]
public class TutorialMessage
{
    [TextArea] public string message;
    [Foldout("Ghost Cursor Effects")] public bool ghostCursorHydroponics;
    [Foldout("Ghost Cursor Effects")] public bool ghostCursorChargingTerminal;
    [Foldout("Ghost Cursor Effects")] public bool ghostCursorArmorPlating;
    [Foldout("Ghost Cursor Effects")] public bool ghostCursorStatBar;

    [Foldout("Other effects")] public bool selectRoom;
}

public class Tutorial : Singleton<Tutorial>
{
    [SerializeField, Tooltip("Ability to disable all tutorial elements")] private bool disableTutorial;
    [SerializeField] TutorialNode[] tutorials = new TutorialNode[10];
    [SerializeField] TextMeshProUGUI tutorialTextbox;
    [SerializeField] TextMeshProUGUI tutorialTitleTextbox;
    [SerializeField] GameObject tutorialPanel;
    private Tick ticker;

    [SerializeField] GameObject highlightPanel;
    [SerializeField] GameObject ghostCursor;


    [SerializeField] float timeStartedLerping;
    [SerializeField] float lerpTime;
    private bool lerping = false;
    private float percentageComplete;
    private Vector3 lerpStart;
    private Vector3 lerpEnd;

    [SerializeField] GameObject vecInsideShip;
    [SerializeField] GameObject vecShopPanel;
    [SerializeField] GameObject vecStatsLeft;
    [SerializeField] GameObject vecStatsRight;
    [SerializeField] GameObject vecRoomDetails;


    private TutorialNode currentTutorial;
    private int index;
    private bool tutorialPrerequisitesComplete = false;

    private void Start()
    {
        if (SavingLoadingManager.instance.GetHasSave())
        {
            LoadTutorialStatus();
        }
        else
        {
            for(int i = 0; i < tutorials.Length; i++)
            {
                tutorials[i].tutorialFinished = false;
            }
            SaveTutorialStatus();
        }
        
        ticker = FindObjectOfType<Tick>();

        currentTutorial = tutorials[1];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ContinueButton();
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            ContinueButton(true);
        }

        if(tutorialPanel.activeSelf == true && GameManager.instance.currentGameState == InGameStates.Events && !ticker.IsTickStopped())
        {
            Debug.LogWarning("stopping tick");
            ticker.StopTickUpdate();
        }

        //Effects
        ///////////////////////////////////////////////////////////////////////////////////////
        if(tutorialPanel.activeSelf == true)
        {
            if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
            {
                if (currentTutorial.tutorialMessages[index].ghostCursorHydroponics) GhostCursorHydroponics();
                else if (currentTutorial.tutorialMessages[index].ghostCursorChargingTerminal) GhostCursorChargingTerminal();
                else if (currentTutorial.tutorialMessages[index].ghostCursorArmorPlating) GhostCursorArmorPlating();
                else if (currentTutorial.tutorialMessages[index].ghostCursorStatBar) GhostCursorStatBar();
            }

            if (GameManager.instance.currentGameState == InGameStates.CrewManagement)
            {
                if (currentTutorial.tutorialMessages[index].selectRoom) EffectSelectRoom();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////

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
    public void SetCurrentTutorial(int tutorialID, bool forcedTutorial)
    {
        if(disableTutorial) return;

        //if you're already in a tutorial, stop.
        if (tutorialPanel.activeSelf == true) return;
        //if the game tries to force a tutorial the player has already seen, stop.
        if (tutorials[tutorialID].tutorialFinished == true && forcedTutorial == true) return;

        currentTutorial = tutorials[tutorialID];

        if (tutorialPanel.activeSelf == false)
        {
            tutorialPanel.SetActive(true);
            tutorialTextbox.text = currentTutorial.tutorialMessages[0].message;
            tutorialTitleTextbox.text = currentTutorial.tutorialName;
            index = 0;
        }

    }

    public void CloseCurrentTutorial(bool finished = true)
    {
        if(disableTutorial) return;



        if (tutorialPanel.activeSelf == true)
        {
            if (GameManager.instance.currentGameState == InGameStates.Events && ticker.IsTickStopped())
            {
                Debug.LogWarning("resuming tick");
                if(!EventSystem.instance.eventActive) ticker.StartTickUpdate();
            }

            highlightPanel.SetActive(false);
            lerping = false;
            ghostCursor.SetActive(false);
            tutorialPanel.SetActive(false);
            index = 0;

            if(!currentTutorial.tutorialFinished) currentTutorial.tutorialFinished = finished;
            SaveTutorialStatus();
        }
    }

    //X and Y of center of box, with (0,0) being screen center. Width and height of box
    private void HighlightScreenLocation(int xPos, int yPos, int width, int height)
    {
        highlightPanel.SetActive(true);
        RectTransform loc = highlightPanel.GetComponent<RectTransform>();
        loc.sizeDelta = new Vector2(width, height);
        loc.localPosition = new Vector3(xPos, yPos, 1);
    }
    public void UnHighlightScreenLocation()
    {
        if(disableTutorial) return;

        highlightPanel.SetActive(false);
    }

    public void ContinueButton(bool back = false)
    {
        if(disableTutorial) return;

        if (tutorialPanel.activeSelf == true)
        {
            tutorialPrerequisitesComplete = false;

            //forward
            if (index < currentTutorial.tutorialMessages.Length - 1 && back == false)
            {
                index++;
                tutorialTextbox.text = currentTutorial.tutorialMessages[index].message;

                StopLerping();
                UnHighlightScreenLocation();
            }
            //backward
            else if(index < currentTutorial.tutorialMessages.Length && index > 0 && back == true)
            {
                index --;
                tutorialTextbox.text = currentTutorial.tutorialMessages[index].message;
                StopLerping();
                UnHighlightScreenLocation();
            }
            else if(back == false)
            {
                CloseCurrentTutorial();
            }
        }
    }

    private void BeginLerping(Vector3 start, Vector3 end)
    {
        ghostCursor.SetActive(true);

        lerpStart = start;
        lerpEnd = end;

        timeStartedLerping = Time.time;
        lerping = true;
    }
    private void StopLerping()
    {
        lerping = false;

        ghostCursor.SetActive(false);
    }

    private Vector3 LerpGhostCursor(Vector3 start, Vector3 end, float timeStarted, float ltime = 1)
    {

        timeStarted = Time.time - timeStarted;
        percentageComplete = timeStarted / ltime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;


    }

    private void GhostCursorHydroponics()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            if (FindObjectOfType<ShipBuildingShop>().GetCurrentTab() != "Food") FindObjectOfType<ShipBuildingShop>().ToResourceTab("Food");
            tutorialPrerequisitesComplete = true;
        }
        if(lerping == false) BeginLerping(vecShopPanel.transform.position, vecInsideShip.transform.position);
    }
    private void GhostArmorPlating()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            if (FindObjectOfType<ShipBuildingShop>().GetCurrentTab() != "HullDurability") FindObjectOfType<ShipBuildingShop>().ToResourceTab("HullDurability");
            tutorialPrerequisitesComplete = true;
        }
        if (lerping == false) BeginLerping(vecShopPanel.transform.position, vecInsideShip.transform.position);
    }
    private void GhostCursorChargingTerminal()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            if (FindObjectOfType<ShipBuildingShop>().GetCurrentTab() != "Energy") FindObjectOfType<ShipBuildingShop>().ToResourceTab("Energy");
            tutorialPrerequisitesComplete = true;
        }
        if (lerping == false) BeginLerping(vecShopPanel.transform.position, vecInsideShip.transform.position);
    }
    private void GhostCursorArmorPlating()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            if (FindObjectOfType<ShipBuildingShop>().GetCurrentTab() != "HullDurability") FindObjectOfType<ShipBuildingShop>().ToResourceTab("HullDurability");
            tutorialPrerequisitesComplete = true;
        }
        if (lerping == false) BeginLerping(vecShopPanel.transform.position, vecInsideShip.transform.position);
    }
    private void GhostCursorStatBar()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            tutorialPrerequisitesComplete = true;
        }
        if (lerping == false) BeginLerping(vecStatsLeft.transform.position, vecStatsRight.transform.position);
    }
    private void EffectSelectRoom()
    {
        if (tutorialPrerequisitesComplete == false)
        {
            FindObjectOfType<RoomPanelToggle>().OpenPanel(0);
            FindObjectOfType<CrewManagementRoomDetailsMenu>().ChangeCurrentRoom(FindObjectsOfType<RoomStats>()[0].gameObject);
            tutorialPrerequisitesComplete = true;
        }
    }

    public bool GetTutorialActive()
    {
        return tutorialPanel.activeSelf;
    }

    public void SaveTutorialStatus()
    {
        SavingLoadingManager.instance.Save<TutorialNode[]>("tutorials", tutorials);
    }
    public void LoadTutorialStatus()
    {
        tutorials = SavingLoadingManager.instance.Load<TutorialNode[]>("tutorials");
    }

}
