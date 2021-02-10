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
    public List<Resource> resources { get; private set; } = new List<Resource>();

    public int minCrew;
    public int maxCrew;
    public int minPower;
    public int maxPower;

    [Tooltip("Ex: .8 for 80% of the orginal price")]
    public float priceReducationPercent;

    [Tooltip("How many credits the room costs to place")]
    public int price;

    public int currentCrew;

    [ResizableTextArea]
    public string roomDescription;

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

    ShipStats shipStats;

    public bool flatOutput;

    public bool usedRoom = false;
    [SerializeField] private bool isPowered = false;

    [SerializeField] private RoomTooltipUI roomTooltipUI;

    public Transform[] statCanvas;
    
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
        shipStats = FindObjectOfType<ShipStats>();
        StartCoroutine(LateStart(0.1f));
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SubtractRoomStats();
        //    Destroy(this.gameObject);
        //}
    }

    public void UpdateUsedRoom()
    {
        usedRoom = true;
        roomTooltipUI.RoomIsUsed();
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
    /// Delays when the script calls functions in start
    /// </summary>
    /// <param name="time">How long the delay should be</param>
    /// <returns></returns>
    private IEnumerator LateStart(float time)
    {
        yield return new WaitForSeconds(time);
        GetStats();
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
            resource.minAmount = resource.amount - (int)(resource.amount * percent);
            if (flatOutput == true)
            {
                switch (resource.resourceType.resourceName)
                {
                    case "Credits":
                        credits += resource.amount;
                        break;
                    case "Energy":
                        energy += resource.amount;
                        break;
                    case "Security":
                        security += resource.amount;
                        break;
                    case "Ship Weapons":
                        shipWeapons += resource.amount;
                        break;
                    case "Crew":
                        crew += resource.amount;
                        break;
                    case "Food":
                        food += resource.amount;
                        break;
                    case "Food Per Tick":
                        foodPerTick += resource.amount;
                        break;
                    case "Hull Durability":
                        shipHealth += resource.amount;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (resource.resourceType.resourceName)
                {
                    case "Credits":
                        credits += resource.minAmount;
                        break;
                    case "Energy":
                        energy += resource.minAmount;
                        break;
                    case "Security":
                        security += resource.minAmount;
                        break;
                    case "Ship Weapons":
                        shipWeapons += resource.minAmount;
                        break;
                    case "Crew":
                        crew += resource.minAmount;
                        break;
                    case "Food":
                        food += resource.minAmount;
                        break;
                    case "Food Per Tick":
                        foodPerTick += resource.minAmount;
                        break;
                    case "Hull Durability":
                        shipHealth += resource.minAmount;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void SetIsPowered()
    {
        isPowered = !isPowered;
    }

    public bool GetIsPowered()
    {
        return isPowered;
    }

    public void UpdateRoomStats()
    {
        shipStats.roomBeingPlaced = gameObject;
        SubtractRoomStats();
        foreach (Resource resource in resources)
        {
            switch (resource.resourceType.resourceName)
            {
                case "Credits":
                    //credits -= resource.minAmount;
                    credits = resource.activeAmount;
                    break;
                case "Energy":
                    //energy -= resource.minAmount;
                    energy = resource.activeAmount;
                    break;
                case "Security":
                    //security -= resource.minAmount;
                    security = resource.activeAmount;
                    break;
                case "Ship Weapons":
                    //shipWeapons -= resource.minAmount;
                    shipWeapons = resource.activeAmount;
                    break;
                case "Crew":
                    //crew -= resource.minAmount;
                    crew = resource.activeAmount;
                    break;
                case "Food":
                    //food -= resource.minAmount;
                    food = resource.activeAmount;
                    break;
                case "Food Per Tick":
                    //foodPerTick -= resource.minAmount;
                    foodPerTick = resource.activeAmount;
                    break;
                case "Hull Durability":
                    //shipHealth -= resource.minAmount;
                    shipHealth = resource.activeAmount;
                    break;
                default:
                    break;
            }
        }
        AddRoomStats();
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

    private void OnDestroy()
    {
        // reset the ship's crew stats back to before room was placed
        shipStats.CrewCurrent += new Vector3(currentCrew, 0, currentCrew);
    }
}
