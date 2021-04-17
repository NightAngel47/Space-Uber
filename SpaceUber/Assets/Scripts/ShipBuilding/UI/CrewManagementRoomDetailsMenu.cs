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
    [ReadOnly] public GameObject selectedRoom = null;

    [SerializeField] private string noRoomSelectedMessage = "Select a room to view its details.";
    [SerializeField] private GameObject[] roomDetailsInfo = new GameObject[2];
    
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
        Tutorial.Instance.SetCurrentTutorial(2, true);
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
                break;
        }

        // see if chat available
        UpdateChatAvailability();
    }

    /// <summary>
    /// Updates the crew stat and room production stats
    /// </summary>
    public void UpdateCrewAssignment()
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
                producesAmount.text = resource.activeAmount + " / " + resource.amount[roomStats.GetRoomLevel() - 1];
            }
            else
            {
                producesAmount.text = resource.activeAmount.ToString();
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

    private void UpdateCrewButtons()
    {
        if (roomStats.maxCrew == 0) // no crew needed for room
        {
            for (var i = 0; i < crewButtons.Length; i++)
            {
                crewButtons[i].interactable = false;
                crewButtonTexts[i].SetButtonInteractable(false);
            }
        }
        else if(roomStats.currentCrew == 0) // no crew assigned to room 
        {
            crewButtons[0].interactable = false;
            crewButtons[1].interactable = true;
            
            crewButtonTexts[0].SetButtonInteractable(false);
            crewButtonTexts[1].SetButtonInteractable(true);
        }
        else if (roomStats.currentCrew == roomStats.maxCrew) // crew assigned at max
        {
            crewButtons[0].interactable = true;
            crewButtons[1].interactable = false;
            
            crewButtonTexts[0].SetButtonInteractable(true);
            crewButtonTexts[1].SetButtonInteractable(false);
        }
        else // crew assigned between min and max (or something went wrong so both buttons active)
        {
            for (var i = 0; i < crewButtons.Length; i++)
            {
                crewButtons[i].interactable = true;
                crewButtonTexts[i].SetButtonInteractable(true);
            }
        }
    }
    
    public void AddCrew(bool fromSave = false)
    {
        if (selectedRoom == null) return;
        
        if (shipStats.CrewCurrent.z > 0 && roomStats.currentCrew < roomStats.maxCrew)
        {
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
    }

    public void SetTalkToCrewButtonState(bool state)
    {
        talkButton.interactable = state;
        talkButtonText.SetButtonInteractable(state);
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
