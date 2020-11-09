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
    public TMP_Text crewRemainingText;
    public GameObject crewManagementText;
    public Button nextButton;
    public GameObject roomText;
    public GameObject costsText;
    public GameObject crewAmount;
    public GameObject overclockOutput;
    public GameObject addButton;
    public GameObject subtractButton;
    private GameObject statPanel;

    private GameObject room;

    private int minAssignableCrew;
    private RoomStats[] currentRoomList;

    public Button overclockButton;
    public GameObject statAndNumPrefab;
    public GameObject outputObject;
    public GameObject[] sceneButtons;

    private List<GameObject> overtimeStats = new List<GameObject>();
    private List<GameObject> outputStats = new List<GameObject>();

    public void Start()
    {
        ss = FindObjectOfType<ShipStats>();
        crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew();

        statPanel = gameObject.transform.GetChild(0).gameObject;
        TurnOffPanel();

        currentRoomList = FindObjectsOfType<RoomStats>();

        overclockButton.interactable = false;

        foreach(RoomStats r in currentRoomList)
        {
            minAssignableCrew += r.minCrew;
        }
        
        if (minAssignableCrew > 0)
        {
            nextButton.interactable = false;
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

        addButton.GetComponent<Button>().interactable = true;
        subtractButton.GetComponent<Button>().interactable = true;

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

        if (room.GetComponent<OverclockRoom>().GetMiniGame() == MiniGameType.Security) 
        {
            //security
            GameObject resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
            //resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = ; // resource icon
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Security"; // resource name
            overtimeStats.Add(resourceGO);
        }

        if (room.GetComponent<OverclockRoom>().GetMiniGame() == MiniGameType.Asteroids) 
        {
            //shipweapons
            GameObject resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Ship Weapons"; // resource name
            overtimeStats.Add(resourceGO);
        }

        if (room.GetComponent<OverclockRoom>().GetMiniGame() == MiniGameType.CropHarvest) 
        {
            //food amount
            GameObject resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Food Amount";
            overtimeStats.Add(resourceGO);
        }

        if (room.GetComponent<OverclockRoom>().GetMiniGame() == MiniGameType.StabilizeEnergyLevels) 
        {
            //Hull Durability
            GameObject resourceGO = Instantiate(statAndNumPrefab, overclockOutput.transform);
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "Hull Durability";
            overtimeStats.Add(resourceGO);
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
        if (ss.GetRemainingCrew() > 0 && rs.currentCrew < room.GetComponent<RoomStats>().maxCrew)
        {
            rs.UpdateCurrentCrew(1);
            ss.UpdateCrewAmount(-1, 0);
            minAssignableCrew--;
            crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew().ToString();
            crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
            UpdateOutput();
            room.GetComponent<RoomStats>().UpdateRoomStats();

            if (minAssignableCrew <= 0)
            {
                nextButton.interactable = true;
            }
        }
    }

    public void SubtractCrew()
    {
        if (rs.currentCrew > 0)
        {
            rs.UpdateCurrentCrew(-1);
            ss.UpdateCrewAmount(1, 0);
            minAssignableCrew++;
            crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew().ToString();
            crewAmount.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().currentCrew.ToString();
            UpdateOutput();
            room.GetComponent<RoomStats>().UpdateRoomStats();

            if (minAssignableCrew > 0)
            {
                nextButton.interactable = false;
            }
        }
    }

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
        crewManagementText.SetActive(false);
        room.GetComponent<OverclockRoom>().PlayMiniGame();
    }

    public void TurnOnOverclockButton()
    {
        if (room.GetComponent<OverclockRoom>().GetMiniGame() != MiniGameType.None)
        {
            overclockButton.interactable = true;
        }

        for(int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i].SetActive(false);
        }
    }
}
