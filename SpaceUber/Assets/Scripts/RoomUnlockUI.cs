using System;
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

    private CampaignManager campaignManager;

    private void Awake()
    {
        campaignManager = FindObjectOfType<CampaignManager>();
    }

    public void Start()
    {
        // see if there are any rooms to unlock
        count = 0;
        foreach (GameObject room in GameManager.instance.allRoomList)
        {
            switch (campaignManager.GetCurrentJobIndex())
            {
                case 0:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 2 && GameManager.instance.GetUnlockLevel(2) < 3)
                    {
                        newRooms.Add(room);
                    }
                    break;
                case 1:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 3 && GameManager.instance.GetUnlockLevel(3) < 3)
                    {
                        newRooms.Add(room);
                    }
                    break;
                case 2:
                    if (room.GetComponent<RoomStats>().GetRoomGroup() == 1 && GameManager.instance.GetUnlockLevel(1) < 3)
                    {
                        newRooms.Add(room);
                    }
                    break;
            }
        }
        
        // display unlocked rooms or skip to job select if no new rooms
        if(newRooms.Count > 0) // if there are new rooms do stuff
        {
            nextButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Next";
            nextButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Next";
            UpdateRoomUnlockUI(newRooms[count].GetComponent<RoomStats>()); // show upgrades if there are new rooms
            count++;
        }
        else  // otherwise skip to job select
        {
            GoToNextRoomOrJob();
        }
    }

    private void UpdateRoomUnlockUI(RoomStats roomStats)
    {
        rname.text = roomStats.roomName;
        //Debug.Log(FindObjectOfType<CampaignManager>().GetCurrentJobIndex());

        if ((campaignManager.GetCurrentCampaignIndex() > 0 && campaignManager.GetCurrentJobIndex() < 3) || 
            (campaignManager.GetCurrentCampaignIndex() == 0 && campaignManager.GetCurrentJobIndex() == 2)) //room is getting a new level
        {
            // old level
            levelOld.text = "Level " + GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup());
            needsCreditsOld.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString(); //-2 to get old level
            needsPowerOld.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();
            needsCrewOld.text = roomStats.minCrew + "-" + roomStats.maxCrew;

            // new level
            levelNew.text = "Level " + (GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) + 1);
            needsCreditsNew.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString(); //-1 to get current level
            needsPowerNew.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString();
            needsCrewNew.text = roomStats.minCrew + "-" + roomStats.maxCrew;

            newRoomText.SetActive(false);
            rightSideData.SetActive(true);
        }
        else //Getting a entire new room, just getting one the new rooms stats
        {
            levelOld.text = "Level " + GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup());
            needsCreditsOld.text = roomStats.price[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString(); 
            needsPowerOld.text = roomStats.minPower[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();
            needsCrewOld.text = roomStats.minCrew + "-" + roomStats.maxCrew;

            newRoomText.SetActive(true);
            rightSideData.SetActive(false);
        }

        roomSprite.sprite = roomStats.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        description.text = roomStats.roomDescription;

        // update produces stats
        if (roomStats.TryGetComponent(out Resource resource))
        {
            // old produces
            resourceIconLeft.sprite = resource.resourceType.resourceIcon;
            producesResourceOld.text = resource.resourceType.resourceName;
            producesAmountOld.text = resource.amount[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup()) - 1].ToString();
            
            // new produces
            resourceIconRight.sprite = resource.resourceType.resourceIcon;
            producesResourceNew.text = resource.resourceType.resourceName;
            producesAmountNew.text = resource.amount[GameManager.instance.GetUnlockLevel(roomStats.GetRoomGroup())].ToString();
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
            foreach(RoomStats room in FindObjectsOfType<RoomStats>())
            {
                room.UpdateUsedRoom();
            }
            
            FindObjectOfType<ShipStats>().ReAddPayoutFromRooms();
            campaignManager.GoToNextJob(); //tells campaign manager to activate the next available job
        }
    }
}
