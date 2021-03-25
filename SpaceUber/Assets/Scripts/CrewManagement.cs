/*
 * CrewManagement.cs
 * Author(s): Sydney
 * Created on: 10-20-20
 * Description:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrewManagement : MonoBehaviour
{
    private ShipStats shipStats;
    private RoomStats roomStats;
    public GameObject crewAmount;
    public GameObject powerAmount;
    public GameObject overclockOutput;
    [SerializeField] GameObject statPanel;
    [SerializeField] GameObject crewAssignmentCanvas;

    private GameObject room;

    private int minAssignableCrew;
    private RoomStats[] currentRoomList;

    public Button chatButton;
    public Button overclockButton;
    public GameObject statAndNumPrefab;
    public GameObject outputObject;
    public GameObject[] sceneButtons; //contains finish ship and back to shipbuilding buttons

    private List<GameObject> overtimeStats = new List<GameObject>();
    private List<GameObject> outputStats = new List<GameObject>();

    public IEnumerator Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
        //crewUnassignedText.text = "Unassigned Crew: " + (int)shipStats.CrewCurrent.z;

        //statPanel = gameObject.transform.GetChild(0).gameObject;
        TurnOffPanel();

        overclockButton.gameObject.SetActive(false);
        chatButton.gameObject.SetActive(false);

        currentRoomList = FindObjectsOfType<RoomStats>();

        foreach (RoomStats r in currentRoomList)
        {
            minAssignableCrew += r.minCrew;
            minAssignableCrew -= r.currentCrew;
        }

        if (minAssignableCrew > 0)
        {
            sceneButtons[0].GetComponent<Button>().interactable = false;
        }

        // wait for object script to load if loading savefile
        yield return new WaitUntil((() => FindObjectOfType<ObjectScript>()));
        room = FindObjectOfType<ObjectScript>().gameObject;

        CheckForMinCrew();


        CrewViewManager.Instance.EnableCrewView();//automatically enable crew view when you enter crew mgmt


        Tutorial.Instance.SetCurrentTutorial(2, true);
    }

    private void Update()
    {
        if (statPanel.activeSelf && EventSystem.instance.eventActive)
        {
            TurnOffPanel();
        }
    }

    /// <summary>
    /// Updates the Room Panel that shows up after building ship. This updates it so that
    /// all stats are correct for the room displayed and makes sure the buttons that need
    /// to be enabled are.
    /// </summary>
    public void UpdateRoom(GameObject g)
    {
        for(int i = 0; i < overtimeStats.Count; i++)
        {
            Destroy(overtimeStats[i]);
        }
        overtimeStats.Clear();

        room = g;
        roomStats = room.GetComponent<RoomStats>();
        shipStats.roomBeingPlaced = room;

        statPanel.SetActive(true);

        //FUNCTIONALITY MOVED TO CrewManagementRoomDetailsMenu.cs
        //roomText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomName;
        //costsText.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().price.ToString();
        //costsText.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().minPower.ToString()
          //  + " - " + room.GetComponent<RoomStats>().maxPower.ToString();
        //roomText.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomDescription;
        //powerAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().minPower.ToString();
        //crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
        //costsText.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().minCrew.ToString()
            //+ " - " + room.GetComponent<RoomStats>().maxCrew.ToString();

        if(true || room.GetComponent<RoomStats>().maxPower == 0) // right now power management doesn't function so the buttons are just always disabled
        {
            powerAmount.transform.parent.GetChild(0).gameObject.SetActive(false);
            powerAmount.transform.parent.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            powerAmount.transform.parent.GetChild(0).gameObject.SetActive(true);
            powerAmount.transform.parent.GetChild(1).gameObject.SetActive(true);
        }

        if(room.GetComponent<RoomStats>().maxCrew == 0)
        {
            crewAmount.transform.parent.GetChild(0).gameObject.SetActive(false);
            crewAmount.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            crewAmount.transform.parent.GetChild(0).gameObject.SetActive(true);
            crewAmount.transform.parent.GetChild(2).gameObject.SetActive(true);
        }

        UpdateOutput();

        if (GameManager.instance.currentGameState != InGameStates.Events && overclockButton.interactable)
        {
            overclockButton.interactable = false;
        }
        else if(!overclockButton.interactable && GameManager.instance.currentGameState == InGameStates.Events)
        {
            overclockButton.interactable = true;
        }

        GameObject resourceGO;
        switch (room.GetComponent<OverclockRoom>().GetMiniGame())
        {
            case MiniGameType.Security:
                //security
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Security"; // resource name
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.Asteroids:
                //shipweapons
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Ship Weapons";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.CropHarvest:
                //food amount
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Food Amount";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.StabilizeEnergyLevels:
                //Energy
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energy";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.SlotMachine:
                //Credits
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Credits";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.HullRepair:
                //Hull Durability
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Hull Durability";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            default:
                overclockButton.gameObject.SetActive(false);
                break;
        }

        if (GameManager.instance.currentGameState != InGameStates.Events)
        {
            overclockButton.gameObject.SetActive(false);
            chatButton.gameObject.SetActive(false);
        }

        UpdateChatAvailability();
    }

    /// <summary>
    /// Updates the rooms output based on the crew assigned. So if any crew assigned it gives full amount,
    /// and gives percentage of full amount for when amount of crew matters.
    /// </summary>
    private void UpdateOutput()
    {
        foreach (var stat in outputStats)
        {
            Destroy(stat);
        }
        outputStats.Clear();

        foreach (var resource in room.GetComponent<RoomStats>().resources)
        {
            GameObject resourceGO = Instantiate(statAndNumPrefab, outputObject.transform);
            resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = resource.resourceType.resourceIcon; // resource icon
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = resource.resourceType.resourceName; // resource name

            room.GetComponent<RoomStats>().SetActiveAmount(resource);

            if (room.GetComponent<RoomStats>().flatOutput == false)
            {
                resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.activeAmount + " / " + resource.amount[roomStats.GetRoomLevel() - 1];  // This part wasn't being called before, by uncommenting it'll fix it, but ruin the text placement in the UI + " / " + (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(room.GetComponent<RoomStats>().ignoreMorale)); // resource amount
            }
            else
            {
                resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.activeAmount.ToString(); // resource amount
            }

            outputStats.Add(resourceGO);
        }
    }

    public void AddCrew(bool fromSave = false)
    {
        if (shipStats.CrewCurrent.z > 0 && roomStats.currentCrew < roomStats.maxCrew)
        {
            roomStats.UpdateCurrentCrew(1);
            if (!fromSave)
            {
                shipStats.CrewCurrent += new Vector3(0, 0, -1);
                minAssignableCrew--;
            }
            FindObjectOfType<CrewManagementRoomDetailsMenu>().UpdateCrewAssignment(roomStats.currentCrew);
            UpdateOutput();
            if(room.GetComponent<RoomStats>().resources.Count > 0)
                room.GetComponent<RoomStats>().UpdateRoomStats(room.GetComponent<Resource>().resourceType);

            CheckForMinCrew();
        }
    }

    public void SubtractCrew()
    {
        if (roomStats.currentCrew > 0)
        {
            roomStats.UpdateCurrentCrew(-1);
            shipStats.CrewCurrent += new Vector3(0, 0, 1);
            minAssignableCrew++;
            FindObjectOfType<CrewManagementRoomDetailsMenu>().UpdateCrewAssignment(roomStats.currentCrew);
            UpdateOutput();
            room.GetComponent<RoomStats>().UpdateRoomStats(room.GetComponent<Resource>().resourceType);

            CheckForMinCrew();
        }
    }

    private void CheckForMinCrew()
    {
        sceneButtons[0].GetComponent<ButtonTwoBehaviour>().SetButtonInteractable(FindObjectsOfType<RoomStats>().All(room => room.minCrew <= room.currentCrew));
    }

    public void TurnOffPanel()
    {
        FindObjectOfType<RoomPanelToggle>().ClosePanel();
    }

    public void TurnOnPanel()
    {
        statPanel.SetActive(true);
        UpdateChatAvailability();
    }

    public void StartOverclockGame()
    {
        TurnOffPanel();
        //crewManagementText.SetActive(false);
        room.GetComponent<OverclockRoom>().PlayMiniGame();

    }

    public void StartChat()
    {
        print("Starting a chat");
        OverclockRoom ovRoom = room.GetComponent<OverclockRoom>();
        StartCoroutine(EventSystem.instance.StartNewCharacterEvent(ovRoom.GetEvents()));
        TurnOffPanel();

    }

    public void FinishWithCrewAssignment()
    {
        if (room != null && room.GetComponent<OverclockRoom>().GetMiniGame() != MiniGameType.None)
        {
            overclockButton.gameObject.SetActive(true);
        }

        foreach (var button in sceneButtons)
        {
            button.SetActive(false);
        }

        crewAssignmentCanvas.SetActive(false);

        if(!overclockButton.interactable)
        {
            overclockButton.interactable = true;
        }
    }

    /// <summary>
    /// Changes whether or not the chat availability button will activate
    /// </summary>
    public void UpdateChatAvailability()
    {
        OverclockRoom ovRoom = room.GetComponent<OverclockRoom>();

        if(ovRoom.hasEvents && GameManager.instance.currentGameState == InGameStates.Events)
        {
            print(ovRoom.GetEvents().Count + " events for this room");

            chatButton.gameObject.SetActive(EventSystem.instance.CanChat(ovRoom.GetEvents()));
        }
        else
        {
            chatButton.gameObject.SetActive(false);
        }
    }
}
