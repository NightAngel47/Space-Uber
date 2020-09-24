/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description: 
 */

using System.Collections;
using UnityEngine;

public class OverclockController : MonoBehaviour
{
    ShipStats shipStats;

    //If a room is already being overclocked
    public bool overclocking = false;

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    void Update()
    {
        
    }

    public void CallStartOverclocking(int moraleAmount, string resourceType = null, int resourceAmount = 0)
    {
        StartCoroutine(StartOverclocking(moraleAmount, resourceType, resourceAmount));
    }

    public void CallStopOverClocking(string resourceType = null, int resourceAmount = 0)
    {
        StartCoroutine(StopOverclocking(resourceType, resourceAmount));
    }

    private IEnumerator StartOverclocking(int moraleAmount, string resourceType = null, int resourceAmount = 0)
    {
        overclocking = true;
        switch (resourceType)
        {
            case "Credits":
                shipStats.UpdateCreditsAmount(resourceAmount);
                break;
            case "Energy":
                shipStats.UpdateEnergyAmount(resourceAmount, resourceAmount);
                break;
            case "Security":
                shipStats.UpdateSecurityAmount(resourceAmount);
                break;
            case "Ship Weapons":
                shipStats.UpdateShipWeaponsAmount(resourceAmount);
                break;
            case "Food":
                shipStats.UpdateFoodAmount(resourceAmount);
                break;
            case "Food Per Tick":
                shipStats.UpdateFoodPerTickAmount(resourceAmount);
                break;
            case "Hull Durability":
                shipStats.UpdateHullDurabilityAmount(resourceAmount);
                break;
            default:
                break;
        }
        shipStats.UpdateCrewMorale(moraleAmount);
        new WaitForSecondsRealtime(5.0f);
        StartCoroutine(StopOverclocking(resourceType, resourceAmount));
        return null;
    }

    private IEnumerator StopOverclocking(string resourceType = null, int resourceAmount = 0)
    {
        overclocking = false;
        switch (resourceType)
        {
            case "Credits":
                shipStats.UpdateCreditsAmount(resourceAmount);
                break;
            case "Energy":
                shipStats.UpdateEnergyAmount(resourceAmount, resourceAmount);
                break;
            case "Security":
                shipStats.UpdateSecurityAmount(resourceAmount);
                break;
            case "Ship Weapons":
                shipStats.UpdateShipWeaponsAmount(resourceAmount);
                break;
            case "Food":
                shipStats.UpdateFoodAmount(resourceAmount);
                break;
            case "Food Per Tick":
                shipStats.UpdateFoodPerTickAmount(resourceAmount);
                break;
            case "Hull Durability":
                shipStats.UpdateHullDurabilityAmount(resourceAmount);
                break;
            default:
                break;
        }
        return null;
    }
}
