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
    public int minPower;
    public int maxPower;

    [Tooltip("Ex: .8 for 80% of the orginal price")] public float priceReducationPercent;

    [Tooltip("How many credits the room costs to place")] public int price;

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

    public bool usedRoom = false;
    [SerializeField] private bool isPowered = false;

    [SerializeField] private RoomTooltipUI roomTooltipUI;

    public Transform[] statCanvas;

    private Camera cam;

    public List<GameObject> CharacterEvents;

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
            print(resource.resourceType.resourceName);
            if (flatOutput)
            {
                switch (resource.resourceType.Rt)
                {
                    case ResourceDataTypes._Credits:
                        credits += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Energy:
                        energy += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Security:
                        security += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._ShipWeapons:
                        shipWeapons += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Crew:
                        crew += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Food:
                        food += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._FoodPerTick:
                        foodPerTick += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._HullDurability:
                        shipHealth += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._CrewMorale:
                        morale += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    case ResourceDataTypes._Payout:
                        credits += (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                        break;
                    default:
                        Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                        break;
                }
            }
            else
            {
                resource.minAmount = resource.amount - (int)(resource.amount * percent);

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
        if (flatOutput == false)
        {
            for (int i = crewRange - 1; i >= 0; i--)
            {
                if (currentCrew == maxCrew)
                {
                    resource.activeAmount = (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
                else if (currentCrew == 0 || currentCrew < minCrew)
                {
                    resource.activeAmount = (int)(resource.minAmount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
                else if (currentCrew == maxCrew - i)
                {
                    float percent = (float)i / (float)crewRange;
                    resource.activeAmount = (int)(((resource.amount - resource.minAmount) - (int)((resource.amount - resource.minAmount) * percent) + resource.minAmount) * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
                }
            }
        }
        else
        {
            resource.activeAmount = (int)(resource.amount * MoraleManager.instance.GetMoraleModifier(ignoreMorale));
        }
    }

    public void UpdateRoomStats(ResourceDataType resourceData)
    {
        shipStats.roomBeingPlaced = gameObject;
        SubtractOneRoomStat(resourceData);

        Resource resource = resources[0];
        
        switch (resourceData.Rt)
        {
            case ResourceDataTypes._Credits:
                credits = resource.activeAmount;
                break;
            case ResourceDataTypes._Energy:
                energy = resource.activeAmount;
                break;
            case ResourceDataTypes._Security:
                security = resource.activeAmount;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipWeapons = resource.activeAmount;
                break;
            case ResourceDataTypes._Crew:
                crew = resource.activeAmount;
                break;
            case ResourceDataTypes._Food:
                food = resource.activeAmount;
                break;
            case ResourceDataTypes._FoodPerTick:
                foodPerTick = resource.activeAmount;
                break;
            case ResourceDataTypes._HullDurability:
                shipHealth = resource.activeAmount;
                break;
            case ResourceDataTypes._CrewMorale:
                morale = resource.activeAmount;
                break;
            case ResourceDataTypes._Payout:
                credits = resource.activeAmount;
                break;
            default:
                Debug.LogError("Resource type: " + resource.resourceType.resourceName + " not setup in RoomStats");
                break;
        }
        
        AddOneRoomStat(resourceData);
    }

    private void AddOneRoomStat(ResourceDataType resourceData)
    {
        switch (resourceData.Rt)
        {
            case ResourceDataTypes._Credits:
                shipStats.Credits += -price;
                break;
            case ResourceDataTypes._Energy:
                shipStats.EnergyRemaining += new Vector2(energy, energy);
                shipStats.EnergyRemaining += new Vector2(-minPower, 0);
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
                shipStats.Credits += -price; if (usedRoom == true)
                {
                    shipStats.Credits += (int)(price * priceReducationPercent);
                }
                else
                {
                    shipStats.Credits += price;
                }
                break;
            case ResourceDataTypes._Energy:
                shipStats.EnergyRemaining += new Vector2(-energy, -energy);
                shipStats.EnergyRemaining += new Vector2(minPower, 0);
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
        shipStats.Credits += -price;
        shipStats.Payout += credits;
        shipStats.EnergyRemaining += new Vector2(energy, energy);
        shipStats.EnergyRemaining += new Vector2(-minPower, 0);
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
            shipStats.Credits += (int)(price * priceReducationPercent);
        }
        else
        {
            shipStats.Credits += price;
        }

        shipStats.Payout += -credits;
        shipStats.EnergyRemaining += new Vector2(-energy, -energy);
        shipStats.EnergyRemaining += new Vector2(minPower, 0);
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
        shipStats.CrewCurrent += new Vector3(currentCrew, 0, currentCrew);
    }
}
