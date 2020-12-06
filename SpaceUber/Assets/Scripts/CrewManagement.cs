/*
 * CrewManagement.cs
 * Author(s): Sydney
 * Created on: 10-20-20
 * Description:
 */

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrewManagement : MonoBehaviour
{
    private int crewAddAmount = 1;
    private ShipStats ss;
    private RoomStats rs;
    public TMP_Text crewUnassignedText;
    public GameObject crewManagementText;
    public GameObject roomText;
    public GameObject costsText;
    public GameObject crewAmount;
    public GameObject powerAmount;
    public GameObject overclockOutput;
    private GameObject statPanel;

    private GameObject room;

    private int minAssignableCrew;
    private RoomStats[] currentRoomList;

    public Button overclockButton;
    public GameObject statAndNumPrefab;
    public GameObject outputObject;
    public GameObject[] sceneButtons; //contains finish ship and back to shipbuilding buttons

    private List<GameObject> overtimeStats = new List<GameObject>();
    private List<GameObject> outputStats = new List<GameObject>();

    public void Start()
    {
        ss = FindObjectOfType<ShipStats>();
        crewUnassignedText.text = "Unassigned Crew: " + ss.CrewUnassigned;

        statPanel = gameObject.transform.GetChild(0).gameObject;
        TurnOffPanel();
        
        overclockButton.gameObject.SetActive(false);

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

        room = FindObjectOfType<ObjectScript>().gameObject;
    }
    
    private void Update()
    {
        if (statPanel.activeSelf && EventSystem.instance.eventActive)
        {
            TurnOffPanel();
        }
    }

    public void UpdateRoom(GameObject g)
    {
        for(int i = 0; i < overtimeStats.Count; i++)
        {
            Destroy(overtimeStats[i]);
        }
        overtimeStats.Clear();
        
        room = g;
        rs = room.GetComponent<RoomStats>();

        statPanel.SetActive(true);

        roomText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomName;
        costsText.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().price.ToString();
        costsText.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().minPower.ToString()
            + " - " + room.GetComponent<RoomStats>().maxPower.ToString();
        roomText.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomDescription;
        crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
        costsText.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().minCrew.ToString()
            + " - " + room.GetComponent<RoomStats>().maxCrew.ToString();

        UpdateOutput();

        if (GameManager.instance.currentGameState != InGameStates.Events && overclockButton.interactable)
        {
            overclockButton.interactable = false;
        }
        else if(!overclockButton.interactable && GameManager.instance.currentGameState == InGameStates.Events)
        {
            overclockButton.interactable = true;
        }
        
        if (GameManager.instance.currentGameState != InGameStates.Events)
        {
            overclockButton.gameObject.SetActive(false);
        }
        else if(GameManager.instance.currentGameState == InGameStates.Events)
        {
            overclockButton.gameObject.SetActive(true);
        }

        GameObject resourceGO;
        switch (room.GetComponent<OverclockRoom>().GetMiniGame())
        {
            case MiniGameType.Security:
                //security
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[1]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Security"; // resource name
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.Asteroids:
                //shipweapons
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[2]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Ship Weapons";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.CropHarvest:
                //food amount
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[3]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Food Amount";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.StabilizeEnergyLevels:
                //Energy
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[5]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Energy";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.SlotMachine:
                //Credits
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[0]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Credits";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            case MiniGameType.HullRepair:
                //Hull Durability
                resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
                resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ss.statIcons[6]; // resource icon
                resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Hull Durability";
                resourceGO.transform.GetChild(2).gameObject.SetActive(false); // resource amount, which for some minigames can be variable so for now I'm just not showing it
                overtimeStats.Add(resourceGO);
                overclockButton.gameObject.SetActive(true);
                break;
            default:
                overclockButton.gameObject.SetActive(false);
                break;
        }
    }

    public void UpdateOutput()
    {
        for (int i = 0; i < outputStats.Count; i++)
        {
            Destroy(outputStats[i]);
        }
        outputStats.Clear();

        int crewRange = room.GetComponent<RoomStats>().maxCrew - room.GetComponent<RoomStats>().minCrew + 1;
        foreach (var resource in room.GetComponent<RoomStats>().resources)
        {
            GameObject resourceGO = Instantiate(statAndNumPrefab, outputObject.transform);
            resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = resource.resourceIcon; // resource icon
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = resource.resourceType; // resource name

            if (room.GetComponent<RoomStats>().flatOutput == false)
            {
                for (int i = crewRange - 1; i >= 0; i--)
                {
                    if (room.GetComponent<RoomStats>().currentCrew == room.GetComponent<RoomStats>().maxCrew)
                    {
                        resource.activeAmount = resource.amount;
                    }

                    else if (room.GetComponent<RoomStats>().currentCrew == 0)
                    {
                        resource.activeAmount = 0;
                    }

                    else if (room.GetComponent<RoomStats>().currentCrew == room.GetComponent<RoomStats>().maxCrew - i)
                    {
                        float percent = (float)i / (float)crewRange;
                        resource.activeAmount = resource.amount - (int)(resource.amount * percent);
                    }
                }

                resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.activeAmount.ToString() + " / " + resource.amount.ToString(); // resource amount
            }
            else
            {
                //just the assgined active amount
                //might need to do things here
            }

            resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.activeAmount.ToString(); // resource amount
            outputStats.Add(resourceGO);
        }
    }

    public void ChangeAmount(int a)
    {
        crewAddAmount = a;
    }

    public void AddCrew()
    {
        if (ss.CrewUnassigned > 0 && rs.currentCrew < room.GetComponent<RoomStats>().maxCrew)
        {
            rs.UpdateCurrentCrew(1);
            ss.UpdateCrewAmount(-1);
            minAssignableCrew--;
            crewUnassignedText.text = "Unassigned Crew: " + ss.CrewUnassigned;
            crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
            UpdateOutput();
            room.GetComponent<RoomStats>().UpdateRoomStats();

            if (minAssignableCrew <= 0)
            {
                sceneButtons[0].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void SubtractCrew()
    {
        if (rs.currentCrew > 0)
        {
            rs.UpdateCurrentCrew(-1);
            ss.UpdateCrewAmount(1);
            minAssignableCrew++;
            crewUnassignedText.text = "Unassigned Crew: " + ss.CrewUnassigned;
            crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
            UpdateOutput();
            room.GetComponent<RoomStats>().UpdateRoomStats();

            if (minAssignableCrew > 0)
            {
                sceneButtons[0].GetComponent<Button>().interactable = false;
            }
        }
    }

    //public void AddPower()
    //{
    //    if(ss.HasEnoughPower(rs.minPower) && rs.GetIsPowered() == false)
    //    {
    //        ss.UpdateEnergyAmount(-rs.minPower);
    //        rs.SetIsPowered();
    //        powerAmount.GetComponent<TextMeshProUGUI>().text = rs.minPower.ToString();
    //        UpdateOutput();
    //    }
    //}

    //public void SubtractPower()
    //{
    //    if (rs.GetIsPowered() == true)
    //    {
    //        ss.UpdateEnergyAmount(rs.minPower);
    //        rs.SetIsPowered();
    //        powerAmount.GetComponent<TextMeshProUGUI>().text = rs.minPower.ToString();
    //        UpdateOutput();
    //    }
    //}

    public void LoseCrew(int crewLost)
    {
        RoomStats[] currentRoomList = FindObjectsOfType<RoomStats>();

        do
        {
            int rand = Random.Range(0, currentRoomList.Length);

            if (currentRoomList[rand].currentCrew > 0)
            {
                currentRoomList[rand].currentCrew -= 1;
                crewLost -= 1;
            }
        } while (crewLost > 0);
    }

    public void TurnOffPanel()
    {
        statPanel.SetActive(false);
    }

    public void TurnOnPanel()
    {
        statPanel.SetActive(true);
    }

    public void StartOverclockGame()
    {
        TurnOffPanel();
        //crewManagementText.SetActive(false);
        room.GetComponent<OverclockRoom>().PlayMiniGame();
    }

    public void TurnOnOverclockButton()
    {
        if (room.GetComponent<OverclockRoom>().GetMiniGame() != MiniGameType.None)
        {
            overclockButton.gameObject.SetActive(true);
        }

        for (int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i].SetActive(false);
        }
    }
}
