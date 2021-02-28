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
    ShipBuildingBuyableRoom[] shopSlots = new ShipBuildingBuyableRoom[3];

    [SerializeField] Sprite blackButton;
    [SerializeField] Sprite redButton;
    [SerializeField] private RectTransform shopTabsContainer;
    private Image[] shopTabs = new Image[0];

    private void Awake()
    {
        shopSlots = GetComponentsInChildren<ShipBuildingBuyableRoom>();
        shopTabs = new Image[shopTabsContainer.childCount];
        for (int i = 0; i < shopTabs.Length; ++i)
        {
            shopTabs[i] = shopTabsContainer.GetChild(i).GetComponent<Image>();
        }
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
            shopToggle.TogglePanelVis();
            shopTabs[(int) tab].sprite = blackButton;
            tab = ShipBuildingTab.None;
            return;
        }

        if (tab != ShipBuildingTab.None)
        {
            shopTabs[(int) tab].sprite = blackButton;
        }
        
        // set ship building tab
        tab = shipBuildingTab;
        shopTabs[(int) tab].sprite = redButton;

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
    }

    void SetShopSlots(ResourceDataTypes[] resourceDataTypesArray)
    {
        // reset shop slots
        foreach (var slot in shopSlots)
        {
            slot.gameObject.SetActive(true);
        }
        
        // find matching resource rooms
        int i = 0;
        foreach (var roomPrefab in objectsToSpawn.availableRooms)
        {
            if (!roomPrefab.TryGetComponent(out Resource resource)) continue;
            if (resourceDataTypesArray.All(resourceDataType => resource.resourceType.Rt != resourceDataType)) continue;
            shopSlots[i].roomPrefab = roomPrefab;
            shopSlots[i].UpdateRoomInfo();
            ++i;
            if(i > shopSlots.Length) break;
        }

        // special exception case for medbay
        if (resourceDataTypesArray[0] == ResourceDataTypes._Crew)
        {
            foreach (var roomPrefab in objectsToSpawn.availableRooms.Where(roomPrefab => roomPrefab.GetComponent<RoomStats>().roomName.Equals("Medbay")))
            {
                shopSlots[i].roomPrefab = roomPrefab;
                shopSlots[i].UpdateRoomInfo();
                ++i;
            }
        }

        // set reset of the slots to inactive
        while (i < shopSlots.Length)
        {
            shopSlots[i].gameObject.SetActive(false);
            ++i;
        }
    }
}
