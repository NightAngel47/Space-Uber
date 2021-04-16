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
    [SerializeField] Image levelImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI roomSize;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] GameObject newLevelText;
    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;

    private SpawnObject objectsToSpawn;
    private CampaignManager campaignManager;
    private ShipBuildingShop shop;

    

    public static bool cheatLevels = false;
    public static int cheatJob = 0;

    [HideInInspector] public int levelTemp = 1;

    private void Awake()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();
        campaignManager = FindObjectOfType<CampaignManager>();
        shop = FindObjectOfType<ShipBuildingShop>();
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
                        GameManager.instance.SetUnlockLevel(1, campaignManager.GetCurrentCampaignIndex() + 1);
                        break;
                    case 1:
                        GameManager.instance.SetUnlockLevel(1, campaignManager.GetCurrentCampaignIndex() + 1);
                        GameManager.instance.SetUnlockLevel(2, campaignManager.GetCurrentCampaignIndex() + 1);

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
                        GameManager.instance.SetUnlockLevel(1, campaignManager.GetCurrentCampaignIndex() + 1);
                        GameManager.instance.SetUnlockLevel(2, campaignManager.GetCurrentCampaignIndex() + 1);
                        GameManager.instance.SetUnlockLevel(3, campaignManager.GetCurrentCampaignIndex() + 1);


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

            GameManager.instance.SetUnlockLevel(1, cheatJob + 1);
            GameManager.instance.SetUnlockLevel(2, cheatJob + 1);
            GameManager.instance.SetUnlockLevel(3, cheatJob + 1);
            
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

        SavingLoadingManager.instance.SaveRoomLevels(GameManager.instance.GetUnlockLevel(1), GameManager.instance.GetUnlockLevel(2), GameManager.instance.GetUnlockLevel(3));
    }

    public void UpdateRoomInfo()
    {
        CheckActiveButtons();

        roomImage.sprite = roomPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        RoomStats roomStats = roomPrefab.GetComponent<RoomStats>();

        rname.text = roomStats.roomName;
        needsCredits.text = "" + roomStats.price[levelTemp - 1];
        needsPower.text = "" + roomStats.minPower[levelTemp - 1];
        needsCrew.text = "" + roomStats.minCrew + "-" + roomStats.maxCrew.ToString();
        roomSize.text = roomPrefab.GetComponent<ObjectScript>().shapeDataTemplate.roomSizeName;

        
        level.text = levelTemp.ToString();
        levelImage.sprite = shop.GetRoomLevelIcons()[levelTemp - 1];

        switch (campaignManager.GetCurrentJobIndex())
        {
            case 0:
                newLevelText.SetActive(roomStats.GetRoomGroup() == 1);
                break;
            case 1:
                newLevelText.SetActive(roomStats.GetRoomGroup() == 2);
                break;
            case 2:
                newLevelText.SetActive(roomStats.GetRoomGroup() == 3);
                break;
            default:
                newLevelText.SetActive(false);
                break;
        }

        if (roomPrefab.TryGetComponent(out Resource resource))
        {
            resourceIcon.sprite = resource.resourceType.resourceIcon;
            producesResource.text = resource.resourceType.resourceName;
            producesAmount.text = "" + resource.amount[levelTemp - 1];
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
        if (roomPrefab == null) return;
        RoomStats roomStats = roomPrefab.GetComponent<RoomStats>();

        switch (roomStats.GetRoomGroup())
        {
            case 1:
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < GameManager.instance.GetUnlockLevel(1)))
                {
                    levelTemp += levelChange;

                    UpdateRoomInfo();
                }
                break;
            case 2:
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < GameManager.instance.GetUnlockLevel(2)))
                {
                    levelTemp += levelChange;

                    UpdateRoomInfo();
                }
                break;
            case 3:
                if ((levelChange < 0 && levelTemp > 1) || (levelChange > 0 && levelTemp < GameManager.instance.GetUnlockLevel(3)))
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
        GameManager.instance.SetUnlockLevel(1, group1);
        GameManager.instance.SetUnlockLevel(2, group2);
        GameManager.instance.SetUnlockLevel(3, group3);
    }

    private void CheckActiveButtons()
    {
        if(levelTemp >= 3)
        {
            increaseButton.interactable = false;
            decreaseButton.interactable = true;
        }
        else if(levelTemp <= 1)
        {
            increaseButton.interactable = true;
            decreaseButton.interactable = false;
        }
        else
        {
            increaseButton.interactable = true;
            decreaseButton.interactable = true;
        }
    }

    
}
