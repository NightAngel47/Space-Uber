using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUnlockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] Image resourceIconLeft;
    [SerializeField] Image resourceIconRight;
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
        count = 0;
        foreach (GameObject room in GameManager.instance.allRoomList)
        {
            switch (FindObjectOfType<CampaignManager>().GetCurrentJobIndex())
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

        if (FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex() > 0 || (FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex() == 0 && FindObjectOfType<CampaignManager>().GetCurrentJobIndex() == 2)) //room is getting a new level
        {
            levelOld.text = "Level " + (GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())).ToString();
            needsCreditsOld.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString(); //-2 to get old level
            needsPowerOld.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();
            needsCrewOld.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountOld.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();

            levelNew.text = "Level " + (GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) + 1).ToString();
            needsCreditsNew.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString(); //-1 to get current level
            needsPowerNew.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString();
            needsCrewNew.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountNew.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString();

            newRoomText.SetActive(false);
            rightSideData.SetActive(true);
        }
        else //Getting a entire new room, just getting one the new rooms stats
        {
            needsCreditsOld.text = "Level " + roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString(); 
            needsPowerOld.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();
            needsCrewOld.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
            producesAmountOld.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();

            newRoomText.SetActive(true);
            rightSideData.SetActive(false);
        }

        roomSprite.sprite = roomStats.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        description.text = roomStats.roomDescription;

        if (roomStats.gameObject.TryGetComponent(out Resource resource))
        {
            resourceIconLeft.sprite = resource.resourceType.resourceIcon;
            resourceIconRight.sprite = resource.resourceType.resourceIcon;
            producesResourceOld.text = resource.resourceType.resourceName;

            if (roomStats.GetRoomLevel() > 1)
            {
                producesAmountOld.text = "" + resource.amount[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 2];
            }
            else
            {
                producesAmountOld.text = "" + resource.amount[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1];
            }
            
            producesAmountNew.text = "" + resource.amount[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1];
        }
        else
        {
            resourceIconLeft.gameObject.SetActive(false);
            resourceIconRight.gameObject.SetActive(false);
            producesResourceOld.text = "No Production";
            producesAmountOld.text = "";
        }
    }

    public void GoToNextRoomOrJob()
    {
        if (newRooms.Count - 1 >= count)
        {
            UpdateRoomUnlockUI(newRooms[count].GetComponent<RoomStats>());
            

            if(count == newRooms.Count - 1)
            {
                nextButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Continue to the Next Job";
                nextButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Continue to the Next Job";
            }

            count++;
        }
        else
        {
            FindObjectOfType<CampaignManager>().GoToNextJob(); //tells campaign manager to activate the next available job
        }
    }
}
