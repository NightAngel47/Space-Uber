/* Frank Calabrese
 * ShipBuildingBuyableRoom.cs
 * Changes what room it holds/displays
 * based off of instructions from ShipBuildingShop.cs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildingBuyableRoom : MonoBehaviour
{
    public GameObject roomPrefab;
    
    [SerializeField] Image resourceIcon;
    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI roomSize;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] GameObject newLevelText;

    private SpawnObject objectsToSpawn;

    /// <summary>
    /// Group 1: hydroponics, Bunks, VIP Lounge, Armor plating
    /// </summary>
    private int currentMaxLvlGroup1 = 1;

    /// <summary>
    /// Group 2: Shield generator, photon torpedoes, armory, pantry
    /// </summary>
    private int currentMaxLvlGroup2 = 1;

    /// <summary>
    /// Group 3: Storage Container, energy cannon, core charging terminal, brig
    /// </summary>
    private int currentMaxLvlGroup3 = 1;

    private void Start()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();

        if (FindObjectOfType<CampaignManager>().GetCurrentCampaign() > 0)
        {
            RoomStats[] rooms = FindObjectsOfType<RoomStats>();

            switch (FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex())
            {
                case 0:
                    currentMaxLvlGroup1 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);
                    break;
                case 1:
                    currentMaxLvlGroup2 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);
                    currentMaxLvlGroup1 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);

                    foreach(RoomStats room in rooms)
                    {
                        if(room.roomName == "Power Core")
                        {
                            room.ChangeRoomLevel(1);
                        }
                    }
                    break;
                case 2:
                    currentMaxLvlGroup3 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);
                    currentMaxLvlGroup1 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);
                    currentMaxLvlGroup2 = (FindObjectOfType<CampaignManager>().GetCurrentCampaign() + 2);

                    
                    foreach (RoomStats room in rooms)
                    {
                        if (room.roomName == "Power Core")
                        {
                            room.ChangeRoomLevel(1);
                        }
                    }
                    break;
            }
        }

        SavingLoadingManager.instance.SaveRoomLevels(currentMaxLvlGroup1, currentMaxLvlGroup2, currentMaxLvlGroup3);
    }

    public void UpdateRoomInfo()
    {
        roomImage.sprite = roomPrefab.GetComponentInChildren<SpriteRenderer>().sprite;

        RoomStats roomStats = roomPrefab.GetComponent<RoomStats>();
        rname.text = roomStats.roomName;
        needsCredits.text = "" + roomStats.price[roomStats.GetRoomLevel() - 1];
        needsPower.text = "" + roomStats.minPower[roomStats.GetRoomLevel() - 1];
        needsCrew.text = "" + roomStats.minCrew + "-" + roomStats.maxCrew.ToString();
        roomSize.text = roomPrefab.GetComponent<ObjectScript>().shapeDataTemplate.roomSizeName;
        level.text = roomPrefab.GetComponent<RoomStats>().GetRoomLevel().ToString();

        if (FindObjectOfType<CampaignManager>().GetCurrentCampaign() > 0)
        {
            switch (FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex())
            {
                case 0:
                    if (roomStats.GetRoomGroup() == 1)
                    {
                        newLevelText.SetActive(true);
                    }
                    else
                    {
                        newLevelText.SetActive(false);
                    }
                    break;
                case 1:
                    if (roomStats.GetRoomGroup() == 2)
                    {
                        newLevelText.SetActive(true);
                    }
                    else
                    {
                        newLevelText.SetActive(false);
                    }
                    break;
                case 2:
                    if (roomStats.GetRoomGroup() == 3)
                    {
                        newLevelText.SetActive(true);
                    }
                    else
                    {
                        newLevelText.SetActive(false);
                    }
                    break;
            }
        }

        if (roomPrefab.TryGetComponent(out Resource resource))
        {
            resourceIcon.sprite = resource.resourceType.resourceIcon;
            producesResource.text = resource.resourceType.resourceName;
            producesAmount.text = "" + resource.amount[roomStats.GetRoomLevel() - 1];
        }
        else
        {
            resourceIcon.gameObject.SetActive(false);
            producesResource.text = "No Production";
            producesAmount.text = "";
        }
    }

    public void SpawnSelectedPrefab()
    {
        if (LevelChangeUI.isMouseOverLevel == false)
        {
            objectsToSpawn.SpawnRoom(roomPrefab);
        }
    }

    /// <summary>
    /// Calls the room stats level to be changed so that when placed its the correct stats
    /// Will do other things for room level changing
    /// </summary>
    public void CallRoomLevelChange(int levelChange)
    {
        RoomStats roomStats = roomPrefab.GetComponent<RoomStats>();

        switch(roomStats.GetRoomGroup())
        {
            case 1:
                if ((levelChange < 0 && roomStats.GetRoomLevel() > 1) || (levelChange > 0 && roomStats.GetRoomLevel() < currentMaxLvlGroup1))
                {
                    roomStats.ChangeRoomLevel(levelChange);

                    UpdateRoomInfo();
                }
                break;
            case 2:
                if ((levelChange < 0 && roomStats.GetRoomLevel() > 1) || (levelChange > 0 && roomStats.GetRoomLevel() < currentMaxLvlGroup2))
                {
                    roomStats.ChangeRoomLevel(levelChange);

                    UpdateRoomInfo();
                }
                break;
            case 3:
                if ((levelChange < 0 && roomStats.GetRoomLevel() > 1) || (levelChange > 0 && roomStats.GetRoomLevel() < currentMaxLvlGroup3))
                {
                    roomStats.ChangeRoomLevel(levelChange);

                    UpdateRoomInfo();
                }
                break;
            default:
                break;
        }

        

        //sprite change TODO
    }

    /// <summary>
    /// Takes in a vector 3 to update the currentMaxLvlGroup variables, x = group1, y = group 2, z = group 3
    /// For no change in that group enter in a 0
    /// </summary>
    public void UpdateMaxLevelGroups(int group1, int group2, int group3)
    {
        currentMaxLvlGroup1 = group1;
        currentMaxLvlGroup2 = group2;
        currentMaxLvlGroup3 = group3;
    }
}
