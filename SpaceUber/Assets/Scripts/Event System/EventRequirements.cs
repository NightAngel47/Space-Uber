/*
 * EventRequirements.cs
 * Author(s): Scott Acker
 * Created on: 9/25/2020 
 * Description: Stores information about the requirements for each job
 */

using Boo.Lang;
using System;
using UnityEngine;

[Serializable]
public struct EventRequirements
{

    public enum ResourceType
    {
        HULL,
        ENERGY,
        CREW,
        FOOD,
        WEAPONS,
        SECURITY,
        CREDITS
    }

    [Tooltip("The resource you would like to be compared")]
    public ResourceType selectedResource;
    
    [Tooltip("How much of this resources is required for an event to run")]
    public int requiredAmount;

    [Tooltip("Check this if you would like to check if the ship resource is LESS than the number supplied")]
    public bool lessThan;
    

    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int hullRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int energyRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int crewRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int foodRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int weaponsRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int securityRequired;
    //[Tooltip("Positive to check if greater than x, negative to check if less than x")]
    //public int creditsRequired;

    public bool MatchesRequirements(ShipStats thisShip)
    {
        bool result;
        int shipStat = 0;
        switch (selectedResource)
        {
            case ResourceType.HULL:
                shipStat = thisShip.ShipHealthCurrent;
                break;
            case ResourceType.ENERGY:
                shipStat = thisShip.EnergyRemaining;
                break;
            case ResourceType.CREW:
                shipStat = thisShip.CrewRemaining;
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
            case ResourceType.CREDITS:
                shipStat = thisShip.Credits;
                break;
        }

        if (!lessThan)
        {
            result = shipStat > requiredAmount;
        }
        else
        {
            result = shipStat < requiredAmount;
        }

        return result;
    }
}
