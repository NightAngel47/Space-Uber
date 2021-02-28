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
    private enum ShipBuildingTab { None, Credits, Crew, Food, HullDurability, Energy, ShipWeapons, Security }
    ShipBuildingTab tab = ShipBuildingTab.None;
    private SpawnObject objectsToSpawn;

    [SerializeField] RoomPanelToggle shopToggle;
    [SerializeField] ShipBuildingBuyableRoom[] shopSlots = new ShipBuildingBuyableRoom[3];

    [SerializeField] Sprite blackButton;
    [SerializeField] Sprite redButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject crewButton;
    [SerializeField] GameObject foodButton;
    [SerializeField] GameObject hullButton;
    [SerializeField] GameObject powerButton;
    [SerializeField] GameObject weaponsButton;
    [SerializeField] GameObject securityButton;


    private void Start()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();
    }

    public void ToResourceTab(string resourceDataType)
    {
        Enum.TryParse("_" + resourceDataType, true, out ResourceDataTypes resourceType);
        Enum.TryParse(resourceDataType, true, out ShipBuildingTab shipBuildingTab);

        ResetButtons();
        switch(resourceType)
        {
            case ResourceDataTypes._Food:
                foodButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._Credits:
                creditsButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._Crew:
                crewButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._HullDurability:
                hullButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._Security:
                securityButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._ShipWeapons:
                weaponsButton.GetComponent<Image>().sprite = redButton;
                break;
            case ResourceDataTypes._Energy:
                powerButton.GetComponent<Image>().sprite = redButton;
                break;
        }
        
        // close if same tab
        if (tab == shipBuildingTab)
        {
            shopToggle.TogglePanelVis();
            tab = ShipBuildingTab.None;
            return;
        }
        // set ship building tab
        tab = shipBuildingTab;

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

    public void ResetButtons()
    {
        foodButton.GetComponent<Image>().sprite = blackButton;
        creditsButton.GetComponent<Image>().sprite = blackButton;
        crewButton.GetComponent<Image>().sprite = blackButton;
        hullButton.GetComponent<Image>().sprite = blackButton;
        securityButton.GetComponent<Image>().sprite = blackButton;
        weaponsButton.GetComponent<Image>().sprite = blackButton;
        powerButton.GetComponent<Image>().sprite = blackButton;
       
    }
}
