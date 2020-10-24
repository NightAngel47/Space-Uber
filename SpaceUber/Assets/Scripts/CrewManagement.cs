/*
 * CrewManagement.cs
 * Author(s): Sydney
 * Created on: 10-20-20
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrewManagement : MonoBehaviour
{
    private int crewAddAmount = 1;
    private ShipStats ss;
    private RoomStats rs;
    public TextMeshProUGUI crewRemainingText;
    public GameObject nextButton;
    public GameObject roomPanel;
    public GameObject addButton;
    public GameObject subtractButton;
    public GameObject roomPanelVisualToggle;

    private GameObject room;

    public void Start()
    {
        ss = FindObjectOfType<ShipStats>();
        crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew();
        
    }

    public void UpdateRoom(GameObject g)
    {
        room = g;
        rs = room.GetComponent<RoomStats>();

        roomPanelVisualToggle.SetActive(true);
        addButton.GetComponent<Button>().interactable = true;
        subtractButton.GetComponent<Button>().interactable = true;

        roomPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomName;
        roomPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().price.ToString();
        roomPanel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().roomDescription;
        //roomPanel.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = room.GetComponent<RoomStats>().;    //stats
        roomPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = "Crew Assigned: " + room.GetComponent<RoomStats>().currentCrew.ToString();
        roomPanel.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = "Min Crew: " + room.GetComponent<RoomStats>().minCrew.ToString();
        roomPanel.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = "Max Crew: " + room.GetComponent<RoomStats>().maxCrew.ToString();
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
            crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew().ToString();
            roomPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = "Crew Assigned: " + room.GetComponent<RoomStats>().currentCrew.ToString();

            if (ss.GetRemainingCrew() == 0)
            {
                nextButton.SetActive(true);
            }
        }
    }

    public void SubtractCrew()
    {
        if (rs.currentCrew > 0)
        {
            rs.UpdateCurrentCrew(-1);
            ss.UpdateCrewAmount(1, 0);
            crewRemainingText.text = "Crew Remaining: " + ss.GetRemainingCrew().ToString();
            roomPanel.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = "Crew Assigned: " + room.GetComponent<RoomStats>().currentCrew.ToString();

            if (ss.GetRemainingCrew() > 0)
            {
                nextButton.SetActive(false);
            }
        }
    }
}
