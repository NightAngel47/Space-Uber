using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUnlockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] Image resourceIcon;
    [SerializeField] Image roomSprite;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI needsCreditsOld;
    [SerializeField] TextMeshProUGUI needsPowerOld;
    [SerializeField] TextMeshProUGUI needsCrewOld;
    [SerializeField] TextMeshProUGUI producesResourceOld;
    [SerializeField] TextMeshProUGUI producesAmountOld;
    [SerializeField] TextMeshProUGUI levelOld;

    [SerializeField] TextMeshProUGUI needsCreditsNew;
    [SerializeField] TextMeshProUGUI needsPowerNew;
    [SerializeField] TextMeshProUGUI needsCrewNew;
    [SerializeField] TextMeshProUGUI producesResourceNew;
    [SerializeField] TextMeshProUGUI producesAmountNew;
    [SerializeField] TextMeshProUGUI levelNew;

    [SerializeField] GameObject rightSideData;
    [SerializeField] GameObject newRoomText;
    [SerializeField] GameObject nextButton;

    private List<GameObject> newRooms = new List<GameObject>();
    private int count = 0;

    public void Start()
    {
        foreach (GameObject room in GameManager.instance.allRoomList)
        {
            switch (FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex())
            {
                case 0:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 2)
                    {
                        newRooms.Add(room);
                    }
                    break;
                case 1:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 3)
                    {
                        newRooms.Add(room);
                    }
                    break;
                case 2:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 1)
                    {
                        newRooms.Add(room);
                    }
                    break;
            }
        }

        nextButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Next";
        nextButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Next";

        UpdateRoomUnlockUI(newRooms[count].GetComponent<RoomStats>());
        count++;
    }

    public void UpdateRoomUnlockUI(RoomStats roomStats)
    {
        rname.text = roomStats.roomName;

        if (roomStats.GetRoomLevel() > 1) //room is getting a new level
        {
            needsCreditsOld.text = roomStats.price[roomStats.GetRoomLevel() - 2].ToString(); //-2 to get old level
            needsPowerOld.text = roomStats.minPower[roomStats.GetRoomLevel() - 2].ToString();
            needsCrewOld.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountOld.text = roomStats.price[roomStats.GetRoomLevel() - 2].ToString();

            needsCreditsNew.text = roomStats.price[roomStats.GetRoomLevel() - 1].ToString(); //-1 to get current level
            needsPowerNew.text = roomStats.minPower[roomStats.GetRoomLevel() - 1].ToString();
            needsCrewNew.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountNew.text = roomStats.price[roomStats.GetRoomLevel() - 1].ToString();

            newRoomText.SetActive(false);
            rightSideData.SetActive(true);
        }
        else //Getting a entire new room, just getting one the new rooms stats
        {
            needsCreditsOld.text = roomStats.price[roomStats.GetRoomLevel() - 1].ToString(); 
            needsPowerOld.text = roomStats.minPower[roomStats.GetRoomLevel() - 1].ToString();
            needsCrewOld.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountOld.text = roomStats.price[roomStats.GetRoomLevel() - 1].ToString();

            newRoomText.SetActive(true);
            rightSideData.SetActive(false);
        }

        roomSprite.sprite = roomStats.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        description.text = roomStats.roomDescription;

        if (roomStats.gameObject.TryGetComponent(out Resource resource))
        {
            resourceIcon.sprite = resource.resourceType.resourceIcon;
            producesResourceOld.text = resource.resourceType.resourceName;

            if (roomStats.GetRoomLevel() > 1)
            {
                producesAmountOld.text = "" + resource.amount[roomStats.GetRoomLevel() - 2];
            }
            else
            {
                producesAmountOld.text = "" + resource.amount[roomStats.GetRoomLevel() - 1];
            }
            
            producesAmountNew.text = "" + resource.amount[roomStats.GetRoomLevel() - 1];
        }
        else
        {
            resourceIcon.gameObject.SetActive(false);
            producesResourceOld.text = "No Production";
            producesAmountOld.text = "";
        }
    }

    public void GoToNextRoomOrJob()
    {
        if (newRooms.Count - 1 >= count)
        {
            UpdateRoomUnlockUI(newRooms[count].GetComponent<RoomStats>());
            count++;

            if(count == newRooms.Count - 1)
            {
                nextButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Continue to the Next Job";
                nextButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Continue to the Next Job";
            }
        }
        else
        {
            FindObjectOfType<CampaignManager>().GoToNextJob(); //tells campaign manager to activate the next available job
        }
    }
}
