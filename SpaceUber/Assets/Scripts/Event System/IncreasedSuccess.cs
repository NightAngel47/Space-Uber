/*
 * IncreasedSuccess.cs
 * Author(s): Sam Ferstein
 * Created on: 11/17/2020 (en-US)
 * Description: This stores the information that is required for an increased chance of a random outcome based on a certain stat.
 */
 
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IncreasedSuccess
{
    public enum ResourceType
    {
        HULL,
        ENERGY,
        CREW,
        FOOD,
        WEAPONS,
        SECURITY,
        CREDITS,
        //MORALE
    }

    [Tooltip("The resource you would like to be compared")]
    [SerializeField, AllowNesting]
    private ResourceType selectedResource;

    [Tooltip("How much of this resources is required for an increased percent change to be present")]
    [SerializeField, AllowNesting]
    private int requiredAmount;

    [Tooltip("Click this if you would like to check if the ship resource is LESS than the number supplied")]
    [SerializeField, AllowNesting]
    private bool lessThan = false;

    [Tooltip("What percentage you want the increase to be by")]
    [SerializeField, AllowNesting]
    public float percentIncreasePerPoint;

    private int selectedStatAmount;

    public float GetTotalPercentIncrease()
    {
        return percentIncreasePerPoint * selectedStatAmount;
    }

    public bool MatchesSuccessChance(ShipStats thisShip)
    {
        bool result = true;
        int shipStat = 0;
        switch (selectedResource)
        {
            case ResourceType.HULL:
                shipStat = (int)thisShip.ShipHealthCurrent.x;
                break;
            case ResourceType.ENERGY:
                shipStat = (int)thisShip.EnergyRemaining.x;
                break;
            case ResourceType.CREW:
                shipStat = (int)thisShip.CrewCurrent.x;
                break;
            case ResourceType.FOOD:
                shipStat = thisShip.Food;
                break;
            case ResourceType.WEAPONS:
                shipStat = thisShip.ShipWeapons;
                break;
            case ResourceType.SECURITY:
                shipStat = thisShip.Security;
                break;
            //case ResourceType.MORALE:
            //    shipStat = thisShip.Morale;
            //    break;
            case ResourceType.CREDITS:
                shipStat = thisShip.Credits;
                break;
        }

        if (lessThan)
        {
            result = shipStat < requiredAmount;
        }
        else
        {
            result = shipStat > requiredAmount;
        }
        if(result)
        {
            selectedStatAmount = shipStat;
        }
        else
        {
            selectedStatAmount = 0;
        }
        return result;
    }
}
