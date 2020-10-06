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
    [Tooltip("To keep track of which choice this is")]
    public string choiceName;
    ShipStats shipStats;
    public List<ResourceType> resourcesChanged;
    public List<int> amountChanged;

    public enum ResourceType
    {
        Credits,
        Energy,
        Security,
        ShipWeapons,
        Crew,
        Food,
        FoodPerTick,
        HullDurability,
        Stock
    }

    //private List<string> resourceTypes
    //{
    //    get
    //    {
    //        return new List<string>() { "", "Credits", "Energy", "Security",
    //    "Ship Weapons", "Crew", "Food", "Food Per Tick", "Hull Durability", "Stock" };
    //    }
    //}

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void ChoiceChange()
    {
        if(shipStats != null)
        {
            for (int i = 0; i < resourcesChanged.Count; i++)
            {
                switch (resourcesChanged[i])
                {
                    case ResourceType.Credits:
                        shipStats.UpdateCreditsAmount(amountChanged[i]);
                        break;
                    case ResourceType.Energy:
                        shipStats.UpdateEnergyAmount(amountChanged[i]);
                        break;
                    case ResourceType.Security:
                        shipStats.UpdateSecurityAmount(amountChanged[i]);
                        break;
                    case ResourceType.ShipWeapons:
                        shipStats.UpdateShipWeaponsAmount(amountChanged[i]);
                        break;
                    case ResourceType.Crew:
                        shipStats.UpdateCrewAmount(amountChanged[i]);
                        break;
                    case ResourceType.Food:
                        shipStats.UpdateFoodAmount(amountChanged[i]);
                        break;
                    case ResourceType.FoodPerTick:
                        shipStats.UpdateFoodPerTickAmount(amountChanged[i]);
                        break;
                    case ResourceType.HullDurability:
                        shipStats.UpdateHullDurabilityAmount(amountChanged[i]);
                        break;
                    default:
                        break;
                }

            }
        }
        
    }
}
