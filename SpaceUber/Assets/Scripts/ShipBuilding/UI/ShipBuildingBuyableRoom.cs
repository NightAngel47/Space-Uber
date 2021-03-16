/* Frank Calabrese
 * ShipBuildingBuyableRoom.cs
 * Changes what room it holds/displays
 * based off of instructions from ShipBuildingShop.cs
 */

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
    private CampaignManager campaignManager;

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

    public static bool cheatLevels = false;
    public static int cheatJob = 0;

    private int levelTemp = 0;

    private void Awake()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();
        campaignManager = FindObjectOfType<CampaignManager>();
    }

    private void Start()
    {
        if (cheatLevels == false)
        {
            if (campaignManager.GetCurrentCampaignIndex() > 0)
            {
                RoomStats[] rooms = FindObjectsOfType<RoomStats>();

                switch (campaignManager.GetCurrentJobIndex())
                {
                    case 0:
                        currentMaxLvlGroup1 = (campaignManager.GetCurrentCampaignIndex() + 1);
                        break;
                    case 1:
                        currentMaxLvlGroup2 = (campaignManager.GetCurrentCampaignIndex() + 1);
                        currentMaxLvlGroup1 = (campaignManager.GetCurrentCampaignIndex() + 1);

                        foreach (RoomStats room in rooms)
                        {
                            if (room.roomName == "Power Core")
                            {
                                room.ChangeRoomLevel(1);
                                break;
                            }
                        }
                        break;
                    case 2:
                        currentMaxLvlGroup3 = (campaignManager.GetCurrentCampaignIndex() + 1);
                        currentMaxLvlGroup1 = (campaignManager.GetCurrentCampaignIndex() + 1);
                        currentMaxLvlGroup2 = (campaignManager.GetCurrentCampaignIndex() + 1);


                        foreach (RoomStats room in rooms)
                        {
                            if (room.roomName == "Power Core")
                            {
                                room.ChangeRoomLevel(1);
                                break;
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            RoomStats[] rooms = FindObjectsOfType<RoomStats>();

            currentMaxLvlGroup3 = (cheatJob + 1);
            currentMaxLvlGroup1 = (cheatJob + 1);
            currentMaxLvlGroup2 = (cheatJob + 1);
            
            foreach (RoomStats room in rooms)
            {
                if (room.roomName == "Power Core")
                {
                    if (cheatJob == 0 && room.GetRoomLevel() == 3)
                    {
                        room.ChangeRoomLevel(-2);
                    }
                    else
                    {
                        room.ChangeRoomLevel(1);
                    }
                    break;
                }
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

        if (campaignManager.GetCurrentCampaignIndex() > 0)
        {
            switch (campaignManager.currentCamp)
            {
                case CampaignManager.Campaigns.CateringToTheRich:
                    newLevelText.SetActive(roomStats.GetRoomGroup() == 1);
                    break;
                case CampaignManager.Campaigns.MysteriousEntity:
                    newLevelText.SetActive(roomStats.GetRoomGroup() == 2);
                    break;
                case CampaignManager.Campaigns.FinalTest:
                    newLevelText.SetActive(roomStats.GetRoomGroup() == 3);
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
            objectsToSpawn.SpawnRoom(roomPrefab, levelTemp);
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
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < currentMaxLvlGroup1))
                {
                    levelTemp += levelChange;

                    UpdateRoomInfo();
                }
                break;
            case 2:
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < currentMaxLvlGroup2))
                {
                    levelTemp += levelChange;

                    UpdateRoomInfo();
                }
                break;
            case 3:
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < currentMaxLvlGroup3))
                {
                    levelTemp += levelChange;

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
