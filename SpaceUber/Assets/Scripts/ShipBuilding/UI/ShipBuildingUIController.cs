/* Frank Calabrese  
 * ShipBuildingUIController.cs
 * 2/25/21
 * Sets rooms to be displayed on buy screen depending on tabs
 * and spawns rooms
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipBuildingUIController : MonoBehaviour
{
    public enum shipBuildingTab { Credits, Crew, Food, HullDurability, Power, ShipWeapons, Security }
    shipBuildingTab tab;

    private GameObject roomPrefab;
    private GameObject objectsToSpawn;

    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;

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

    private void Start()
    {
        objectsToSpawn = GameObject.FindGameObjectWithTag("ObjectsToSpawn");
        tab = shipBuildingTab.Credits;
        UpdateRoomInfo();
    }

    private void Update()
    {
        

        

    }

    public void SpawnSelectedPrefab()
    {
        objectsToSpawn.GetComponent<SpawnObject>().SpawnRoom(roomPrefab);
    }

    private void UpdateRoomInfo()
    {
        roomImage.sprite = roomPrefab.GetComponent<Sprite>();

        rname.text = roomPrefab.GetComponent<RoomStats>().roomName;
        needsCredits.text = "" + roomPrefab.GetComponent<RoomStats>().price;
        needsPower.text = "" + roomPrefab.GetComponent<RoomStats>().minPower;
        needsCrew.text = "" + roomPrefab.GetComponent<RoomStats>().minCrew;
        producesResource.text = roomPrefab.GetComponent<Resource>().resourceType.name;
        producesAmount.text = "" + roomPrefab.GetComponent<Resource>().amount;
    }

    private void GetRoomsToDisplay()
    {
        switch(tab)
        {
            case shipBuildingTab.Credits:
                roomPrefab = roomBrig;
                break;
            case shipBuildingTab.Crew:
                roomPrefab = roomBunks;
                break;
            case shipBuildingTab.Food:
                roomPrefab = roomHydroponics;
                break;
            case shipBuildingTab.HullDurability:
                roomPrefab = roomArmorPlating;
                break;
            case shipBuildingTab.Power:
                roomPrefab = roomCoreChargingTerminal;
                break;
            case shipBuildingTab.ShipWeapons:
                roomPrefab = roomPhotonTorpedos;
                break;
            case shipBuildingTab.Security:
                roomPrefab = roomArmory;
                break;
            default:
                Debug.LogError("ShipBuildingUIController state is unset");
                break;
        }
            
    }

    public void ToCreditsTab()
    {
        tab = shipBuildingTab.Credits;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToCrewTab()
    {
        tab = shipBuildingTab.Crew;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToFoodTab()
    {
        tab = shipBuildingTab.Food;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToHullDurabilityTab()
    {
        tab = shipBuildingTab.HullDurability;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToPowerTab()
    {
        tab = shipBuildingTab.Power;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToShipWeaponsTab()
    {
        tab = shipBuildingTab.ShipWeapons;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }
    public void ToSecurityTab()
    {
        tab = shipBuildingTab.Security;
        GetRoomsToDisplay();
        UpdateRoomInfo();
    }


}
