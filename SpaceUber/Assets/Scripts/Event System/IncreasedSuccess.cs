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
    [Tooltip("The resource you would like to be compared")]
    [SerializeField, AllowNesting]
    private ResourceDataTypes selectedResource;

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
            case ResourceDataTypes._HullDurability:
                shipStat = (int)thisShip.ShipHealthCurrent.x;
                break;
            case ResourceDataTypes._Energy:
                shipStat = (int)thisShip.EnergyRemaining.x;
                break;
            case ResourceDataTypes._Crew:
                shipStat = (int)thisShip.CrewCurrent.x;
                break;
            case ResourceDataTypes._Food:
                shipStat = thisShip.Food;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipStat = thisShip.ShipWeapons;
                break;
            case ResourceDataTypes._Security:
                shipStat = thisShip.Security;
                break;
            case ResourceType.MORALE:
                shipStat = MoraleManager.instance.CrewMorale;
                break;
            case ResourceDataTypes._Credits:
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
