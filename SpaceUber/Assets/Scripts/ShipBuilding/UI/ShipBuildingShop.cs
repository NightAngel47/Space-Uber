using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuildingShop : MonoBehaviour
{
    public enum shipBuildingTab { None, Credits, Crew, Food, HullDurability, Power, ShipWeapons, Security }
    shipBuildingTab tab;

    
    private GameObject objectsToSpawn;

    [SerializeField] RoomPanelToggle shopToggle;

    [SerializeField] ShipBuildingBuyableRoom shopSlot1;
    [SerializeField] ShipBuildingBuyableRoom shopSlot2;
    [SerializeField] ShipBuildingBuyableRoom shopSlot3;

    [SerializeField] GameObject roomArmory;
    [SerializeField] GameObject roomArmorPlating;
    [SerializeField] GameObject roomBrig;
    [SerializeField] GameObject roomBunks;
    [SerializeField] GameObject roomCoreChargingTerminal;
    [SerializeField] GameObject roomEnergyCannon;
    [SerializeField] GameObject roomHydroponics;
    [SerializeField] GameObject roomMedbay;
    [SerializeField] GameObject roomPantry;
    [SerializeField] GameObject roomPhotonTorpedos;
    [SerializeField] GameObject roomPowerCore;
    [SerializeField] GameObject roomShieldGenerator;
    [SerializeField] GameObject roomStorage;
    [SerializeField] GameObject roomTeleportationStation;
    [SerializeField] GameObject roomVIPLounge;
    [SerializeField] GameObject roomWarpDrive;

    [SerializeField] Sprite creditsIcon;
    [SerializeField] Sprite crewIcon;
    [SerializeField] Sprite foodIcon;
    [SerializeField] Sprite hullIcon;
    [SerializeField] Sprite powerIcon;
    [SerializeField] Sprite weaponsIcon;
    [SerializeField] Sprite securityIcon;


    private void Start()
    {
        objectsToSpawn = GameObject.FindGameObjectWithTag("ObjectsToSpawn");
        tab = shipBuildingTab.None;
        UpdateRoomInfo();
    }

    private void GetRoomsToDisplay()
    {
        if (shopSlot1.gameObject.activeSelf == false) shopSlot1.gameObject.SetActive(true);
        if (shopSlot2.gameObject.activeSelf == false) shopSlot2.gameObject.SetActive(true);
        if (shopSlot3.gameObject.activeSelf == false) shopSlot3.gameObject.SetActive(true);

        switch (tab)
        {
            case shipBuildingTab.None:
                shopSlot1.gameObject.SetActive(false);
                shopSlot2.gameObject.SetActive(false);
                shopSlot3.gameObject.SetActive(false);
                break;
            case shipBuildingTab.Credits:
                shopSlot1.roomPrefab = roomBrig;
                shopSlot2.roomPrefab = roomStorage;
                shopSlot3.roomPrefab = roomVIPLounge;
                break;
            case shipBuildingTab.Crew:
                shopSlot1.roomPrefab = roomBunks;
                shopSlot2.roomPrefab = roomMedbay;
                shopSlot3.gameObject.SetActive(false);
                break;
            case shipBuildingTab.Food:
                shopSlot1.roomPrefab = roomHydroponics;
                shopSlot2.roomPrefab = roomPantry;
                shopSlot3.gameObject.SetActive(false);
                break;
            case shipBuildingTab.HullDurability:
                shopSlot1.roomPrefab = roomArmorPlating;
                shopSlot2.roomPrefab = roomShieldGenerator;
                shopSlot3.gameObject.SetActive(false);
                break;
            case shipBuildingTab.Power:
                shopSlot1.roomPrefab = roomCoreChargingTerminal;
                shopSlot2.roomPrefab = roomTeleportationStation;
                shopSlot3.roomPrefab = roomWarpDrive;
                break;
            case shipBuildingTab.ShipWeapons:
                shopSlot1.roomPrefab = roomPhotonTorpedos;
                shopSlot2.roomPrefab = roomEnergyCannon;
                shopSlot3.gameObject.SetActive(false);
                break;
            case shipBuildingTab.Security:
                shopSlot1.roomPrefab = roomArmory;
                shopSlot2.gameObject.SetActive(false);
                shopSlot3.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError("ShipBuildingUIController state is unset");
                break;
        }

    }

    public void ToCreditsTab()
    {
        if(tab == shipBuildingTab.Credits)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(creditsIcon);
        tab = shipBuildingTab.Credits;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToCrewTab()
    {
        if (tab == shipBuildingTab.Crew)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(crewIcon);
        tab = shipBuildingTab.Crew;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToFoodTab()
    {
        if (tab == shipBuildingTab.Food)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(foodIcon);
        tab = shipBuildingTab.Food;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToHullDurabilityTab()
    {
        if (tab == shipBuildingTab.HullDurability)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(hullIcon);
        tab = shipBuildingTab.HullDurability;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToPowerTab()
    {
        if (tab == shipBuildingTab.Power)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(powerIcon);
        tab = shipBuildingTab.Power;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToShipWeaponsTab()
    {
        if (tab == shipBuildingTab.ShipWeapons)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(weaponsIcon);
        tab = shipBuildingTab.ShipWeapons;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToSecurityTab()
    {
        if (tab == shipBuildingTab.Security)
        {
            shopToggle.TogglePanelVis();
            tab = shipBuildingTab.None;
            return;
        }

        ChangeIcons(securityIcon);
        tab = shipBuildingTab.Security;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }

    private void UpdateRoomInfo()
    {
        shopSlot1.UpdateRoomInfo();
        shopSlot2.UpdateRoomInfo();
        shopSlot3.UpdateRoomInfo();
    }

    private void ChangeIcons(Sprite spr)
    {
        shopSlot1.resourceIcon.sprite = spr;
        shopSlot2.resourceIcon.sprite = spr;
        shopSlot3.resourceIcon.sprite = spr;
    }

    
}
