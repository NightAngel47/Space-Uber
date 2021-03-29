/*
 * RoomStats.cs
 * Author(s): Grant Frey
 * Created on: 9/16/2020 (en-US)
 * Description:
 */

using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStats : MonoBehaviour
{
    [ReadOnly] public List<Resource> resources = new List<Resource>();

    public int minCrew;
    public int maxCrew;
    public List<int> minPower = new List<int>();
    public int maxPower;

    [Tooltip("Ex: .8 for 80% of the orginal price")] public float priceReducationPercent;

    [Tooltip("How many credits the room costs to place")] public List<int> price = new List<int>();

    public int currentCrew;

    [ResizableTextArea] public string roomDescription;

    public string roomName;

    //List of stats for the room that will affect the ship's stats
    private int credits = 0;
    private int energy = 0;
    private int security = 0;
    private int shipWeapons = 0;
    private int crew = 0;
    private int food = 0;
    private int foodPerTick = 0;
    private int shipHealth = 0;
    private int morale = 0;

    ShipStats shipStats;

    public bool flatOutput;
    public bool ignoreMorale;

    [SerializeField, Min(1)] private int roomLevel = 1;
    [SerializeField] private int roomGroup;

    public bool usedRoom = false;
    [SerializeField] private bool isPowered = false;

    [SerializeField] private RoomTooltipUI roomTooltipUI;

    public Transform[] statCanvas;

    private Camera cam;

    public List<GameObject> CharacterEvents;

    private int resourceChange = 0;

    private void Awake()
    {
        cam = Camera.main;
        shipStats = FindObjectOfType<ShipStats>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => TryGetComponent(out Resource resource));
        
        GetStats();
    }

    public void UpdateUsedRoom()
    {
        usedRoom = true;
    }

    public void UpdateCurrentCrew(int crew)
    {
        currentCrew += crew;
    }

    /// <summary>
    /// Adds a Resource component to the list of Resource components for the individual room
    /// </summary>
    /// <param name="item">The Resource component to add</param>
    public void AddToResourceList(Resource item)
    {
        resources.Add(item);
    }

    /// <summary>
    /// Adds the stats from all attached Resource components to RoomStats
    /// </summary>
    private void GetStats()
    {
        int crewRange = maxCrew - minCrew + 1;
        float percent = (float)(maxCrew - 1) / (float)crewRange;

        foreach (Resource resource in resources)
        {
            //print(resource.resourceType.resourceName);
            if (flatOutput)
            {
                switch (resource.resourceType.Rt)
                {
                    case ResourceDataTypes._Credits:
                        credits += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Energy:
                        energy += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Security:
                        security += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._ShipWeapons:
                        shipWeapons += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Crew:
                        crew += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Food:
                        food += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._FoodPerTick:
                        foodPerTick += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._HullDurability:
                        shipHealth += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._CrewMorale:
                        morale += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Payout:
                        credits += (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    default:
                        Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                        break;
                }
            }
            else
            {
                resource.minAmount = resource.amount[roomLevel - 1] - (int)(resource.amount[roomLevel - 1] * percent);

                switch (resource.resourceType.Rt)
                {
                    case ResourceDataTypes._Credits:
                        credits += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Energy:
                        energy += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Security:
                        security += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._ShipWeapons:
                        shipWeapons += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Crew:
                        crew += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Food:
                        food += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._FoodPerTick:
                        foodPerTick += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._HullDurability:
                        shipHealth += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._CrewMorale:
                        morale += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Payout:
                        credits += (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    default:
                        Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Used when setting value from save for rooms that don't cooperate :(
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="value"></param>
    public void SetStatOnLoad(Resource resource, int value)
    {
        switch (resource.resourceType.Rt)
        {
            case ResourceDataTypes._Credits:
                credits += value;
                break;
            case ResourceDataTypes._Energy:
                energy += value;
                break;
            case ResourceDataTypes._Security:
                security += value;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipWeapons += value;
                break;
            case ResourceDataTypes._Crew:
                crew += value;
                break;
            case ResourceDataTypes._Food:
                food += value;
                break;
            case ResourceDataTypes._FoodPerTick:
                foodPerTick += value;
                break;
            case ResourceDataTypes._HullDurability:
                shipHealth += value;
                break;
            case ResourceDataTypes._CrewMorale:
                morale += value;
                break;
            case ResourceDataTypes._Payout:
                credits += value;
                break;
            default:
                Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                break;
        }
        
        SetActiveAmount(resource);
    }

    public void SetIsPowered()
    {
        isPowered = !isPowered;
    }

    public bool GetIsPowered()
    {
        return isPowered;
    }
    public void KeepRoomStatsUpToDateWithMorale()
    {
        foreach (Resource resource in resources)
        {
            UpdateRoomStats(resource.resourceType);
        }
    }

    public void SetActiveAmount(Resource resource)
    {
        int crewRange = maxCrew - minCrew + 1;
        int prevResourceValue = resource.activeAmount;
        int newResourceValue = 0;
        if (flatOutput == false)
        {
            for (int i = crewRange - 1; i >= 0; i--)
            {
                if (currentCrew == maxCrew)
                {
                    newResourceValue = (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
                else if (currentCrew == 0 || currentCrew < minCrew)
                {
                    newResourceValue = (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
                else if (currentCrew == maxCrew - i)
                {
                    float percent = (float)i / (float)crewRange;
                    newResourceValue = (int)(((resource.amount[roomLevel - 1] - resource.minAmount) - (int)((resource.amount[roomLevel - 1] - resource.minAmount) * percent) + resource.minAmount) * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
            }
        }
        else
        {
            newResourceValue = (int)(resource.amount[roomLevel - 1] * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
        }

        resourceChange = newResourceValue - prevResourceValue;
    }

    public void UpdateRoomStats(ResourceDataType resourceData)
    {
        shipStats.roomBeingPlaced = gameObject;
        //SubtractOneRoomStat(resourceData);

        Resource resource = resources[0];
        
        switch (resourceData.Rt)
        {
            case ResourceDataTypes._Credits:
                credits += resourceChange;
                resource.activeAmount = credits;
                shipStats.Credits += resourceChange;
                break;
            case ResourceDataTypes._Energy:
                energy += resourceChange;
                resource.activeAmount = energy;
                shipStats.Energy += new Vector3(resourceChange, resourceChange, resourceChange);
                shipStats.Energy += new Vector3(-minPower[roomLevel - 1], 0, -minPower[roomLevel - 1]);
                break;
            case ResourceDataTypes._Security:
                security += resourceChange;
                resource.activeAmount = security;
                shipStats.Security += resourceChange;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipWeapons += resourceChange;
                resource.activeAmount = shipWeapons;
                shipStats.ShipWeapons += resourceChange;
                break;
            case ResourceDataTypes._Crew:
                crew += resourceChange;
                resource.activeAmount = crew;
                shipStats.CrewCurrent += new Vector3(resourceChange, resourceChange, resourceChange);
                break;
            case ResourceDataTypes._Food:
                food += resourceChange;
                resource.activeAmount = food;
                shipStats.Food += resourceChange;
                break;
            case ResourceDataTypes._FoodPerTick:
                foodPerTick += resourceChange;
                resource.activeAmount = foodPerTick;
                shipStats.FoodPerTick += resourceChange;
                break;
            case ResourceDataTypes._HullDurability:
                shipHealth += resourceChange;
                resource.activeAmount = shipHealth;
                shipStats.ShipHealthCurrent += new Vector2(resourceChange, resourceChange);
                break;
            case ResourceDataTypes._CrewMorale:
                morale += resourceChange;
                break;
            case ResourceDataTypes._Payout:
                credits += resourceChange;
                resource.activeAmount = credits;
                shipStats.Payout += resourceChange;
                break;
            default:
                Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                break;
        }

        FindObjectOfType<CrewManagementRoomDetailsMenu>().UpdateCrewAssignment(currentCrew);
        //AddOneRoomStat(resourceData);
    }

    private void AddOneRoomStat(ResourceDataType resourceData)
    {
        switch (resourceData.Rt)
        {
            case ResourceDataTypes._Credits:
                shipStats.Credits += -price[roomLevel - 1];
                break;
            case ResourceDataTypes._Energy:
                shipStats.Energy += new Vector3(energy, energy, energy);
                shipStats.Energy += new Vector3(-minPower[roomLevel - 1], 0, -minPower[roomLevel - 1]);
                break;
            case ResourceDataTypes._Security:
                shipStats.Security += security;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipStats.ShipWeapons += shipWeapons;
                break;
            case ResourceDataTypes._Crew:
                shipStats.CrewCurrent += new Vector3(crew, crew, crew);
                break;
            case ResourceDataTypes._Food:
                shipStats.Food += food;
                break;
            case ResourceDataTypes._FoodPerTick:
                shipStats.FoodPerTick += foodPerTick;
                break;
            case ResourceDataTypes._HullDurability:
                shipStats.ShipHealthCurrent += new Vector2(shipHealth, shipHealth);
                break;
            case ResourceDataTypes._Payout:
                shipStats.Payout += credits;
                break;
            default:
                Debug.LogError("Resource type: " + resourceData.resourceName + " not setup in RoomStats");
                break;
        }
    }

    private void SubtractOneRoomStat(ResourceDataType resourceData)
    {
        switch (resourceData.Rt)
        {
            case ResourceDataTypes._Credits:
                shipStats.Credits += -price[roomLevel - 1]; if (usedRoom == true)
                {
                    shipStats.Credits += (int)(price[roomLevel - 1] * priceReducationPercent);
                }
                else
                {
                    shipStats.Credits += price[roomLevel - 1];
                }
                break;
            case ResourceDataTypes._Energy:
                shipStats.Energy += new Vector3(-energy, -energy, -energy);
                shipStats.Energy += new Vector3(minPower[roomLevel - 1], 0, minPower[roomLevel - 1]);
                break;
            case ResourceDataTypes._Security:
                shipStats.Security += -security;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipStats.ShipWeapons += -shipWeapons;
                break;
            case ResourceDataTypes._Crew:
                shipStats.CrewCurrent += new Vector3(-crew, -crew, -crew);
                break;
            case ResourceDataTypes._Food:
                shipStats.Food += -food;
                break;
            case ResourceDataTypes._FoodPerTick:
                shipStats.FoodPerTick += -foodPerTick;
                break;
            case ResourceDataTypes._HullDurability:
                shipStats.ShipHealthCurrent += new Vector2(-shipHealth, -shipHealth);
                break;
            case ResourceDataTypes._Payout:
                shipStats.Payout += -credits;
                break;
            default:
                Debug.LogError("Resource type: " + resourceData.resourceName + " not setup in RoomStats");
                break;
        }
    }

    /// <summary>
    /// Adds the room's stats to the ship's total
    /// </summary>
    public void AddRoomStats()
    {
        shipStats.roomBeingPlaced = gameObject;
        shipStats.Credits += -price[roomLevel - 1];
        shipStats.Payout += credits;
        shipStats.Energy += new Vector3(energy, energy, energy);
        shipStats.Energy += new Vector3(-minPower[roomLevel - 1], 0, -minPower[roomLevel - 1]);
        shipStats.Security += security;
        shipStats.ShipWeapons += shipWeapons;
        shipStats.CrewCurrent += new Vector3(crew, crew, crew);
        shipStats.Food += food;
        shipStats.FoodPerTick += foodPerTick;
        shipStats.ShipHealthCurrent += new Vector2(shipHealth, shipHealth);
        MoraleManager.instance.CrewMorale += morale;
        
        AnalyticsManager.AddRoomForAnalytics(this);
    }

    /// <summary>
    /// Subtracts the room's stats from the ship's total
    /// </summary>
    public void SubtractRoomStats()
    {
        if(usedRoom == true)
        {
            shipStats.Credits += (int)(price[roomLevel - 1] * priceReducationPercent);
        }
        else
        {
            shipStats.Credits += price[roomLevel - 1];
        }

        shipStats.Payout += -credits;
        shipStats.Energy += new Vector3(-energy, -energy, -energy);
        shipStats.Energy += new Vector3(minPower[roomLevel - 1], 0, minPower[roomLevel - 1]);
        shipStats.Security += -security;
        shipStats.ShipWeapons += -shipWeapons;
        shipStats.CrewCurrent += new Vector3(-crew, -crew, -crew);
        shipStats.Food += -food;
        shipStats.FoodPerTick += -foodPerTick;
        shipStats.ShipHealthCurrent += new Vector2(-shipHealth, -shipHealth);
        MoraleManager.instance.CrewMorale -= morale;
        
        AnalyticsManager.SubtractRoomForAnalytics(this);
    }

    public void SpawnStatChangeText(int value, Sprite icon = null)
    {
        ShipStatsUI shipStatsUI = shipStats.GetComponent<ShipStatsUI>();
        GameObject statChangeUI = Instantiate(shipStatsUI.statChangeText);

        RectTransform rect = statChangeUI.GetComponent<RectTransform>();

        Vector3 spawnPos = cam.WorldToScreenPoint(transform.GetChild(0).position);
        rect.anchoredPosition = new Vector2(spawnPos.x, spawnPos.y);

        statChangeUI.transform.parent = shipStats.GetComponent<ShipStatsUI>().canvas; // you have to set the parent after you change the anchored position or the position gets messed up.  Don't set it in the instantiation.  I don't know why someone decided to change that.

        MoveAndFadeBehaviour moveAndFadeBehaviour = statChangeUI.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, 25 + transform.GetChild(0).localPosition.y * 100);
        moveAndFadeBehaviour.SetValue(value, icon);
    }

    public void ReturnCrewOnRemove()
    {
        // reset the ship's crew stats back to before room was placed
        shipStats.CrewCurrent += new Vector3(0, 0, currentCrew);
    }

    public int GetRoomLevel()
    {
        return roomLevel;
    }

    /// <summary>
    /// Subtracts or adds to the room level based on whether the up or down arrow was pressed
    /// levelChange will be positive if adding or negative when subtracting
    /// </summary>
    public void ChangeRoomLevel(int levelChange)
    {
        roomLevel = levelChange;
        
        if (roomLevel > 3)
        {
            roomLevel = 3;
        }
        
        if (roomLevel < 1)
        {
            roomLevel = 1;
        }
    }

    public int GetRoomGroup()
    {
        return roomGroup;
    }

    public void UpgradePower()
    {
        if(gameObject.GetComponent<ObjectScript>().objectNum == 3)
        {
            switch(FindObjectOfType<CampaignManager>().GetCurrentCampaignIndex())
            {
                case 0:
                    roomLevel = 1;
                    break;
                case 1:
                    roomLevel = 2;
                    break;
                case 2:
                    roomLevel = 3;
                    break;
            }

            foreach (Resource resource in resources)
            {
                resourceChange = resource.amount[roomLevel - 1] - resource.amount[roomLevel - 2];
                UpdateRoomStats(resource.resourceType);
            }
        }
    }
}
