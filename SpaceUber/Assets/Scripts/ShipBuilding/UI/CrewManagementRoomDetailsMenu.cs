/* Frank Calabrese
 * CrewManagementRoomDetailsMenu.cs
 * holds various stats of rooms like name, image, etc.
 * swaps them out depending on what room is being viewed
 */

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewManagementRoomDetailsMenu : MonoBehaviour
{
    private ShipStats shipStats;
    private RoomStats roomStats;
    private OverclockRoom overclockRoom;
    private bool tutorialAlreadyPlayed = false;
    [ReadOnly] public GameObject selectedRoom = null;

    [SerializeField] private string noRoomSelectedMessage = "Select a room to view its details.";
    [SerializeField] private GameObject[] roomDetailsInfo = new GameObject[2];

    [SerializeField] private GameObject addCrewToolTipDisabledText;
    [SerializeField] private GameObject removeCrewToolTipDisabledText;
    [SerializeField] private GameObject overtimeToolTipDisabledText;
    [SerializeField] private GameObject talkToCrewToolTipDisabledText;

    #region UI Elements

    [SerializeField, Foldout("Descriptive Info")] Image roomImage;
    [SerializeField, Foldout("Descriptive Info")] TMP_Text roomName;
    [SerializeField, Foldout("Descriptive Info")] TMP_Text roomDesc;
    [SerializeField, Foldout("Descriptive Info")] TMP_Text roomLevel;
    [SerializeField, Foldout("Descriptive Info")] TMP_Text roomSize;
    [SerializeField, Foldout("Descriptive Info")] GameObject usedImage;

    [SerializeField, Foldout("Requirements")] TMP_Text needsCredits;
    [SerializeField, Foldout("Requirements")] TMP_Text needsPower;
    [SerializeField, Foldout("Requirements")] TMP_Text needsCrew;
    
    [SerializeField, Foldout("Production")] TMP_Text producesResource;
    [SerializeField, Foldout("Production")] Image producesIcon;
    [SerializeField, Foldout("Production")] TMP_Text producesAmount;
    
    [SerializeField, Foldout("Crew Assignment")] TMP_Text currentCrew;
    [SerializeField, Foldout("Crew Assignment")] Button[] crewButtons = new Button[2];
    [SerializeField, Foldout("Crew Assignment")] ButtonTwoBehaviour[] crewButtonTexts = new ButtonTwoBehaviour[2];
    
    [SerializeField, Foldout("Overtime")] TMP_Text overtimeResource;
    [SerializeField, Foldout("Overtime")] Image overtimeIcon;
    [SerializeField, Foldout("Overtime")] TMP_Text overtimeAmount;
    [SerializeField, Foldout("Overtime")] Button overtimeButton;
    [SerializeField, Foldout("Overtime")] ButtonTwoBehaviour overtimeButtonText;
    
    [SerializeField, Foldout("Talk To Crew")] Button talkButton;
    [SerializeField, Foldout("Talk To Crew")] ButtonTwoBehaviour talkButtonText;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ClearUI();
    }

    public void ChangeCurrentRoom(GameObject room)
    {
        if (selectedRoom != null) selectedRoom.GetComponent<RoomHighlight>().Unhighlight();
        
        selectedRoom = room;
        roomStats = room.GetComponent<RoomStats>();
        overclockRoom = room.GetComponent<OverclockRoom>();
        shipStats.roomBeingPlaced = selectedRoom;

        if (FindObjectOfType<RoomPanelToggle>().GetIsOpen())
        {
            room.GetComponent<RoomHighlight>().Highlight();
        }
        else
        {
            room.GetComponent<RoomHighlight>().Unhighlight();
        }
        
        UpdatePanelInfo();
        if (!tutorialAlreadyPlayed)
        {
            Tutorial.Instance.SetCurrentTutorial(2, true);
            tutorialAlreadyPlayed = true;
        }
    }
    
    public void UpdatePanelInfo()
    {
        // enable UI elements
        foreach (GameObject roomDetailSection in roomDetailsInfo)
        {
            roomDetailSection.SetActive(true);
        }
        
        // set room details
        roomImage.sprite = selectedRoom.GetComponentInChildren<SpriteRenderer>().sprite;
        roomName.text = roomStats.roomName;
        roomDesc.text = roomStats.roomDescription;
        roomLevel.text = "Level: " + roomStats.GetRoomLevel();
        roomSize.text = selectedRoom.GetComponent<ObjectScript>().shapeDataTemplate.roomSizeName;
        usedImage.SetActive(roomStats.usedRoom); 
        
        // set room requirements details
        needsCredits.text = roomStats.price[roomStats.GetRoomLevel() - 1].ToString();
        needsPower.text = roomStats.minPower[roomStats.GetRoomLevel() - 1].ToString();
        needsCrew.text = roomStats.minCrew + "-" + roomStats.maxCrew;
        
        // update room production, crew value, and crew buttons
        UpdateCrewAssignment();
        
        // set room overtime details
        SetOvertimeButtonState(GameManager.instance.currentGameState == InGameStates.Events && overclockRoom.MinigameCooledDown);
        overtimeIcon.gameObject.SetActive(true);
        switch (overclockRoom.GetMiniGame())
        {
            case MiniGameType.Security:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._Security).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._Security).resourceIcon;
                break;
            case MiniGameType.Asteroids:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._ShipWeapons).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._ShipWeapons).resourceIcon;
                break;
            case MiniGameType.CropHarvest:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._Food).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._Food).resourceIcon;
                break;
            case MiniGameType.StabilizeEnergyLevels:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._Energy).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._Energy).resourceIcon;
                break;
            case MiniGameType.SlotMachine:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._Credits).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._Credits).resourceIcon;
                break;
            case MiniGameType.HullRepair:
                overtimeResource.text = GameManager.instance.GetResourceData((int) ResourceDataTypes._HullDurability).resourceName;
                overtimeIcon.sprite = GameManager.instance.GetResourceData((int) ResourceDataTypes._HullDurability).resourceIcon;
                break;
            default:
                overtimeResource.text = "";
                overtimeIcon.gameObject.SetActive(false);
                SetOvertimeButtonState(false); // disable button is no mini-game on room
                //overtimeToolTipDisabledText.SetActive(true);
                overtimeToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Overtime Mini-Game for this Room";
                break;
        }

        // see if chat available
        UpdateChatAvailability();
    }

    /// <summary>
    /// Updates the crew stat and room production stats
    /// </summary>
    private void UpdateCrewAssignment()
    {
        currentCrew.text = roomStats.currentCrew + " / " + roomStats.maxCrew;

        // set room production details
        if (selectedRoom.TryGetComponent(out Resource resource))
        {
            producesResource.text = resource.resourceType.resourceName;
            producesIcon.sprite = resource.resourceType.resourceIcon;
            
            roomStats.SetActiveAmount(resource);

            if (roomStats.flatOutput == false)
            {
                producesAmount.text = resource.activeAmount + " / " + (int)(resource.amount[roomStats.GetRoomLevel() - 1] * MoraleManager.instance.GetMoraleModifier(roomStats.ignoreMorale));
            }
            else
            {
                producesAmount.text = (resource.amount[roomStats.GetRoomLevel() - 1] * MoraleManager.instance.GetMoraleModifier(roomStats.ignoreMorale)).ToString();
            }
        }
        else
        {
            producesResource.text = "No Production";
            producesIcon.gameObject.SetActive(false);
            producesAmount.text = "";
        }
        
        UpdateCrewButtons();
    }

    private void UpdateCrewButtons(bool clear = false)
    {
        if (clear || roomStats.maxCrew == 0) // no crew needed for room
        {
            for (var i = 0; i < crewButtons.Length; i++)
            {
                crewButtons[i].interactable = false;
                crewButtonTexts[i].SetButtonInteractable(false);

                addCrewToolTipDisabledText.SetActive(true);
                addCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Crew Required for this Room";

                removeCrewToolTipDisabledText.SetActive(true);
                removeCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Crew Required for this Room";
            }
        }
        else if(roomStats.currentCrew == 0) // no crew assigned to room 
        {
            crewButtons[0].interactable = false;
            crewButtons[1].interactable = true;
            
            crewButtonTexts[0].SetButtonInteractable(false);
            crewButtonTexts[1].SetButtonInteractable(true);

            removeCrewToolTipDisabledText.SetActive(true);
            removeCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Crew Assigned to this Room";

            addCrewToolTipDisabledText.SetActive(false);
        }
        else if (roomStats.currentCrew == roomStats.maxCrew) // crew assigned at max
        {
            crewButtons[0].interactable = true;
            crewButtons[1].interactable = false;
            
            crewButtonTexts[0].SetButtonInteractable(true);
            crewButtonTexts[1].SetButtonInteractable(false);

            addCrewToolTipDisabledText.SetActive(true);
            addCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "The Room is at Max Crew Capacity";

            removeCrewToolTipDisabledText.SetActive(false);
        }
        else // crew assigned between min and max (or something went wrong so both buttons active)
        {
            for (var i = 0; i < crewButtons.Length; i++)
            {
                crewButtons[i].interactable = true;
                crewButtonTexts[i].SetButtonInteractable(true);

                addCrewToolTipDisabledText.SetActive(false);
                removeCrewToolTipDisabledText.SetActive(false);
            }
        }
    }
    
    public void AddCrew(bool fromSave = false)
    {
        if (selectedRoom == null) return;
        
        if (shipStats.CrewCurrent.z > 0 && roomStats.currentCrew < roomStats.maxCrew)
        {
            addCrewToolTipDisabledText.SetActive(false);

            roomStats.UpdateCurrentCrew(1);
            if (!fromSave)
            {
                shipStats.CrewCurrent += new Vector3(0, 0, -1);
            }
            
            UpdateCrewAssignment();
            if(selectedRoom.GetComponent<RoomStats>().resources.Count > 0) 
                roomStats.UpdateRoomStats(roomStats.resources[0].resourceType);
            UpdateCrewAssignment();

            if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
            {
                FindObjectOfType<CrewManagement>().CheckForMinCrew();
            }
        }
        else if(shipStats.CrewCurrent.z == 0)
        {
            addCrewToolTipDisabledText.SetActive(true);
            addCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Available Crew on the Ship to be able to Assign to this Room";
        }
    }

    public void SubtractCrew()
    {
        if (selectedRoom == null) return;
        
        if (roomStats.currentCrew > 0)
        {
            roomStats.UpdateCurrentCrew(-1);
            shipStats.CrewCurrent += new Vector3(0, 0, 1);
            
            UpdateCrewAssignment();
            roomStats.UpdateRoomStats(roomStats.resources[0].resourceType);
            UpdateCrewAssignment();

            if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
            {
                FindObjectOfType<CrewManagement>().CheckForMinCrew();
            }
        }
    }

    public void SetOvertimeButtonState(bool state)
    {
        overtimeButton.interactable = state;
        overtimeButtonText.SetButtonInteractable(state);

        //changes tool tip text on why button is disabled
        if(GameManager.instance.currentGameState == InGameStates.ShipBuilding)
        {
            //overtimeToolTipDisabledText.SetActive(true);
            overtimeToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "Can't Perform Overtime Mini-Game while docked in the StarPort";
        }
        else if(state == true)
        {
            //overtimeToolTipDisabledText.SetActive(false);
            overtimeToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = " ";
        }
        else
        {
            //overtimeToolTipDisabledText.SetActive(true);
            overtimeToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "Overtime Mini-Game is on Cooldown and will be available again shortly";
        }
    }

    public void SetTalkToCrewButtonState(bool state)
    {
        talkButton.interactable = state;
        talkButtonText.SetButtonInteractable(state);

        //changes tool tip text on why button is disabled
        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
        {
            //overtimeToolTipDisabledText.SetActive(true);
            talkToCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "The Crew Member is awat from the ship. Can't talk with them while Docked in the StarPort";
        }
        else if (state == true)
        {
            //overtimeToolTipDisabledText.SetActive(false);
            talkToCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = " ";
        }
        else
        {
            //overtimeToolTipDisabledText.SetActive(true);
            talkToCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "No Crew to Talk to in this Room";
        }
    }
    
    public void StartOverclockGame()
    {
        overclockRoom.PlayMiniGame();
    }
    
    /// <summary>
    /// Changes whether or not the chat availability button will activate
    /// </summary>
    private void UpdateChatAvailability()
    {
        if(overclockRoom.hasEvents && GameManager.instance.currentGameState == InGameStates.Events)
        {
            SetTalkToCrewButtonState(EventSystem.instance.CanChat(overclockRoom.GetEvents()));
            if(EventSystem.instance.CanChat(overclockRoom.GetEvents()) == false)
            {
                talkToCrewToolTipDisabledText.GetComponent<TextMeshProUGUI>().text = "The Crew Member isn't available to talk right now, come back later";
            }
        }
        else
        {
            SetTalkToCrewButtonState(false);
        }
    }

    public void StartChat()
    {
        StartCoroutine(EventSystem.instance.StartNewCharacterEvent(overclockRoom.GetEvents()));
    }

    public void ClearUI()
    {
        shipStats = FindObjectOfType<ShipStats>();
        roomName.text = noRoomSelectedMessage;
        usedImage.SetActive(false);

        foreach (GameObject roomDetailSection in roomDetailsInfo)
        {
            roomDetailSection.SetActive(false);
        }
        
        SetOvertimeButtonState(false);
        SetTalkToCrewButtonState(false);
        UpdateCrewButtons(true);
    }

    public void ToggleHighlight()
    {
        if (selectedRoom != null) selectedRoom.GetComponent<RoomHighlight>().ToggleHighlight();
    }

    public void UnHighlight()
    {
        if (selectedRoom != null) selectedRoom.GetComponent<RoomHighlight>().Unhighlight();
    }

    public void Highlight()
    {
        if (selectedRoom != null) selectedRoom.GetComponent<RoomHighlight>().Highlight();
    }
}
