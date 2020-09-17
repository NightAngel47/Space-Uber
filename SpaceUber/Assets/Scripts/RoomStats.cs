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
    private List<Resource> resources = new List<Resource>();

    public int minCrew;
    public int maxCrew;
    public int minPower;
    public int maxPower;

    int currentCrew;

    [ResizableTextArea]
    public string roomDescription;

    public string roomName;

    private int credits = 0;
    private int energy = 0;
    private int security = 0;
    private int shipWeapons = 0;
    private int crew = 0;
    private int food = 0;
    private int foodPerTick = 0;
    private int shipHealth = 0;

    ShipStats shipStats;

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

    public void AddToResourceList(Resource item)
    {
        resources.Add(item);
    }

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
        AddRoomStats();
    }

    /// <summary>
    /// Adds the room's stats to the ship's total
    /// </summary>
    public void AddRoomStats()
    {
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
        shipStats.UpdateCreditsAmount(-credits);
        shipStats.UpdateEnergyAmount(-energy, energy);
        shipStats.UpdateSecurityAmount(-security);
        shipStats.UpdateShipWeaponsAmount(-shipWeapons);
        shipStats.UpdateCrewAmount(-crew, crew);
        shipStats.UpdateFoodAmount(-food);
        shipStats.UpdateFoodPerTickAmount(-foodPerTick);
        shipStats.UpdateHullDurabilityAmount(-shipHealth, shipHealth);
    }
}
