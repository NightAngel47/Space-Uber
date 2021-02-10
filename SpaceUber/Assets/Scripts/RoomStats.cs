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
    private int morale = 0;

    private float moraleModifier;

    ShipStats shipStats;

    public bool flatOutput;
    public bool ignoreMorale;

    public bool usedRoom = false;
    [SerializeField] private bool isPowered = false;

    [SerializeField] private RoomTooltipUI roomTooltipUI;

    public Transform[] statCanvas;

    private Camera cam;

    void Start()
    {
        moraleModifier = MoraleManager.instance.GetMoraleModifier(ignoreMorale);

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
            if (flatOutput == true)
            {
                switch (resource.resourceType)
                {
                    case "Credits":
                        credits += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Energy":
                        energy += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Security":
                        security += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Ship Weapons":
                        shipWeapons += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Crew":
                        crew += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Food":
                        food += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Food Per Tick":
                        foodPerTick += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Hull Durability":
                        shipHealth += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    case "Crew Morale":
                        morale += Mathf.RoundToInt(resource.amount * moraleModifier);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                resource.minAmount = resource.amount - (int)(resource.amount * percent);

                switch (resource.resourceType)
                {
                    case "Credits":
                        credits += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Energy":
                        energy += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Security":
                        security += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Ship Weapons":
                        shipWeapons += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Crew":
                        crew += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Food":
                        food += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Food Per Tick":
                        foodPerTick += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Hull Durability":
                        shipHealth += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    case "Crew Morale":
                        morale += Mathf.RoundToInt(resource.minAmount * moraleModifier);
                        break;
                    default:
                        break;
                }
            }
            print(resource.minAmount);
            print(resource.activeAmount);
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

    public void KeepRoomStatsUpToDateWithMorale()
    {
        if(!ignoreMorale)
        {
            float newMoraleModifier = MoraleManager.instance.GetMoraleModifier();
            if(moraleModifier != newMoraleModifier)
            {
                moraleModifier = newMoraleModifier;
                UpdateRoomStats();
            }
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
                    resource.activeAmount = resource.amount;
                }
                else if (currentCrew == 0)
                {
                    resource.activeAmount = resource.minAmount;
                }
                else if (currentCrew == maxCrew - i)
                {
                    float percent = (float)i / (float)crewRange;
                    resource.activeAmount = (resource.amount - resource.minAmount) - (int)((resource.amount - resource.minAmount) * percent) + resource.minAmount;
                }
            }
        }
        else
        {
            resource.activeAmount = resource.amount;
        }
    }

    public void UpdateRoomStats()
    {
        shipStats.roomBeingPlaced = gameObject;
        SubtractRoomStats();
        foreach (Resource resource in resources)
        {
            SetActiveAmount(resource);

            switch (resource.resourceType)
            {
                case "Credits":
                    //credits -= resource.minAmount;
                    credits = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Energy":
                    //energy -= resource.minAmount;
                    energy = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Security":
                    //security -= resource.minAmount;
                    security = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Ship Weapons":
                    //shipWeapons -= resource.minAmount;
                    shipWeapons = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Crew":
                    //crew -= resource.minAmount;
                    crew = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Food":
                    //food -= resource.minAmount;
                    food = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Food Per Tick":
                    //foodPerTick -= resource.minAmount;
                    foodPerTick = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Hull Durability":
                    //shipHealth -= resource.minAmount;
                    shipHealth = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
                    break;
                case "Crew Morale":
                    //shipHealth -= resource.minAmount;
                    morale = Mathf.RoundToInt(resource.activeAmount * moraleModifier);
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
        MoraleManager.instance.CrewMorale += morale;
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
    }

    public void SpawnStatChangeText(int value, int icon = -1)
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
