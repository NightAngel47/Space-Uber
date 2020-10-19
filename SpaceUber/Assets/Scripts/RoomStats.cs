/*
 * RoomStats.cs
 * Author(s): Grant Frey
 * Created on: 9/16/2020 (en-US)
 * Description: 
 */

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

    int currentCrew;

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

    [SerializeField] private bool usedRoom = false;

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
        StartCoroutine(LateStart(0.25f));
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
        foreach(Resource resource in resources)
        {
            switch (resource.resourceType)
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
        //AddRoomStats();
    }

    /// <summary>
    /// Adds the room's stats to the ship's total
    /// </summary>
    public void AddRoomStats()
    {
        shipStats.UpdateCreditsAmount(-price);
        shipStats.UpdateCreditsAmount(credits);
        shipStats.UpdateEnergyAmount(energy, energy);
        shipStats.UpdateSecurityAmount(security);
        shipStats.UpdateShipWeaponsAmount(shipWeapons);
        shipStats.UpdateCrewAmount(crew, crew);
        shipStats.UpdateFoodAmount(food);
        shipStats.UpdateFoodPerTickAmount(foodPerTick);
        shipStats.UpdateHullDurabilityAmount(shipHealth, shipHealth);
    }

    /// <summary>
    /// Subtracts the room's stats from the ship's total
    /// </summary>
    public void SubtractRoomStats()
    {
        if(usedRoom == true)
        {
            shipStats.UpdateCreditsAmount((int)(price * priceReducationPercent));
        }
        else
        {
            shipStats.UpdateCreditsAmount(price);
        }
        
        shipStats.UpdateCreditsAmount(-credits);
        shipStats.UpdateEnergyAmount(-energy, -energy);
        shipStats.UpdateSecurityAmount(-security);
        shipStats.UpdateShipWeaponsAmount(-shipWeapons);
        shipStats.UpdateCrewAmount(-crew, crew);
        shipStats.UpdateFoodAmount(-food);
        shipStats.UpdateFoodPerTickAmount(-foodPerTick);
        shipStats.UpdateHullDurabilityAmount(-shipHealth, shipHealth);
    }
}
