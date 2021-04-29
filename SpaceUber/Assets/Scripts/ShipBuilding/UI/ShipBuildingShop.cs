/* Frank Calabrese
 * ShipBuildingShop.cs
 * various functions pertaining to the shipbuilding shop UI
 * Controls what rooms are displayed for purchase. 
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildingShop : MonoBehaviour
{
    private enum ShipBuildingTab { None = -1, Credits, Crew, Food, HullDurability, Energy, ShipWeapons, Security }
    ShipBuildingTab tab = ShipBuildingTab.None;
    private SpawnObject objectsToSpawn;

    [SerializeField] RoomPanelToggle shopToggle;
    [SerializeField] Sprite[] roomLevelIcons;
    ShipBuildingBuyableRoom[] shopSlots = new ShipBuildingBuyableRoom[3];

    private void Awake()
    {
        shopSlots = GetComponentsInChildren<ShipBuildingBuyableRoom>();
    }

    private void Start()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();
    }

    public void ToResourceTab(string resourceDataType)
    {

        Enum.TryParse("_" + resourceDataType, true, out ResourceDataTypes resourceType);
        Enum.TryParse(resourceDataType, true, out ShipBuildingTab shipBuildingTab);

        // close if same tab
        if (tab == shipBuildingTab)
        {
            shopToggle.ClosePanel((int) tab);
            tab = ShipBuildingTab.None;
            return;
        }
        
        // set ship building tab
        tab = shipBuildingTab;
        shopToggle.OpenPanel((int) tab);

        // set shop slots based on resource type
        ResourceDataTypes[] resourceDataTypesArray = null;
        switch (resourceType)
        {
            case ResourceDataTypes._Food:
            case ResourceDataTypes._FoodPerTick:
                resourceDataTypesArray = new []{ResourceDataTypes._Food, ResourceDataTypes._FoodPerTick};
                break;
            case ResourceDataTypes._Credits:
            case ResourceDataTypes._Payout:
                resourceDataTypesArray = new []{ResourceDataTypes._Credits, ResourceDataTypes._Payout};
                break;
            default:
                resourceDataTypesArray = new[] {resourceType};
                break;
        }
        SetShopSlots(resourceDataTypesArray);

        //max out displayed room levels
        foreach (ShipBuildingBuyableRoom room in shopSlots)
        {
            room.CallRoomLevelChange(1);
            room.CallRoomLevelChange(1);
        }
    }

    public string GetCurrentTab()
    {
        return tab.ToString();
    }

    void SetShopSlots(ResourceDataTypes[] resourceDataTypesArray)
    {
        // reset shop slots
        foreach (var slot in shopSlots)
        {
            slot.gameObject.SetActive(true);
            slot.levelTemp = 1;
        }
        
        // find matching resource rooms
        int i = 0;
        foreach (var roomPrefab in objectsToSpawn.availableRooms)
        {
            if (!roomPrefab.TryGetComponent(out Resource resource)) continue;
            if (resourceDataTypesArray.All(resourceDataType => resource.resourceType.Rt != resourceDataType)) continue;
            shopSlots[i].roomPrefab = roomPrefab;
            shopSlots[i].UpdateRoomInfo();
            if(++i > shopSlots.Length) break;
        }

        // special exception case for medbay
        if (resourceDataTypesArray[0] == ResourceDataTypes._Crew)
        {
            foreach (var roomPrefab in objectsToSpawn.availableRooms.Where(roomPrefab => roomPrefab.GetComponent<RoomStats>().roomName.Equals("Medbay")))
            {
                shopSlots[i].roomPrefab = roomPrefab;
                shopSlots[i].UpdateRoomInfo();
                ++i;
                break;
            }
        }

        // set reset of the slots to inactive
        while (i < shopSlots.Length)
        {
            shopSlots[i].gameObject.SetActive(false);
            ++i;
        }
    }

    public bool GetShopOpen()
    {
        return shopToggle.GetIsOpen();
    }

    public Sprite[] GetRoomLevelIcons()
    {
        return roomLevelIcons;
    }
}
