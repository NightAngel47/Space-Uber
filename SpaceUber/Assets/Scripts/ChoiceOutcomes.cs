/*
 * ChoiceOutcomes.cs
 * Author(s): Sam Ferstein
 * Created on: 9/18/2020 (en-US)
 * Description: 
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceOutcomes : MonoBehaviour
{
    ShipStats shipStats;
    public int amountChanged;

    [Dropdown("resourceTypes")]
    public string resourceType;

    private List<string> resourceTypes
    {
        get
        {
            return new List<string>() { "", "Credits", "Energy", "Security",
        "Ship Weapons", "Crew", "Food", "Food Per Tick", "Hull Durability", "Stock" };
        }
    }

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void ChoiceChange()
    {
        switch (resourceType)
            {
                case "Credits":
                        shipStats.UpdateCreditsAmount(amountChanged);
                    break;
                case "Energy":
                        shipStats.UpdateEnergyAmount(amountChanged);
                    break;
                case "Security":
                        shipStats.UpdateSecurityAmount(amountChanged);
                    break;
                case "Ship Weapons":
                        shipStats.UpdateShipWeaponsAmount(amountChanged);
                    break;
                case "Crew":
                        shipStats.UpdateCrewAmount(amountChanged);
                    break;
                case "Food":
                        shipStats.UpdateFoodAmount(amountChanged);
                    break;
                case "Food Per Tick":
                        shipStats.UpdateFoodPerTickAmount(amountChanged);
                    break;
                case "Hull Durability":
                        shipStats.UpdateHullDurabilityAmount(amountChanged);
                    break;
                default:
                    break;
        }
    }
}
