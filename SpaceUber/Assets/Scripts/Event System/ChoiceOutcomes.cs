/*
 * ChoiceOutcomes.cs
 * Author(s): Sam Ferstein
 * Created on: 9/18/2020 (en-US)
 * Description:
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChoiceOutcomes
{
    public bool isRandomOutcome;

    [HideIf("isRandomOutcome")]
    public List<ResourceType> resourcesChanged;
    [HideIf("isRandomOutcome")]
    public List<int> amountChanged;

    [ShowIf("isRandomOutcome")]
    public List<float> probabilities;
    [ShowIf("isRandomOutcome")]
    public MultipleRandom[] multipleRandomOutcomes;

    private float outcomeChance;
    private float choiceThreshold ;

    [System.Serializable]
    public class MultipleRandom
    {
        public List<ResourceType> resourcesChanged;
        public List<int> amountChanged;
    }

    void Start()
    {
        //shipStats = FindObjectOfType<ShipStats>();
    }

    public void ChoiceChange(ShipStats ship)
    {
        if (ship != null)
        {
            if (isRandomOutcome)
            {
                outcomeChance = Random.Range(0f, 100f);
                for (int i = 0; i < probabilities.Count; i++)
                {
                    choiceThreshold += probabilities[i];
                    if (outcomeChance <= choiceThreshold)
                    {
                        for(int j = 0; j < multipleRandomOutcomes[i].resourcesChanged.Count; j++)
                        {
                            switch (multipleRandomOutcomes[i].resourcesChanged[j])
                            {
                                case ResourceType.Credits:
                                    ship.UpdateCreditsAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Energy:
                                    ship.UpdateEnergyAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Security:
                                    ship.UpdateSecurityAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.ShipWeapons:
                                    ship.UpdateShipWeaponsAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Crew:
                                    ship.UpdateCrewAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Food:
                                    ship.UpdateFoodAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.FoodPerTick:
                                    ship.UpdateFoodPerTickAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.HullDurability:
                                    ship.UpdateHullDurabilityAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        return;
                    }
                }
            }
            else if (!isRandomOutcome)
            {
                for (int i = 0; i < resourcesChanged.Count; i++)
                {
                    switch (resourcesChanged[i])
                    {
                        case ResourceType.Credits:
                            ship.UpdateCreditsAmount(amountChanged[i]);
                            break;
                        case ResourceType.Energy:
                            ship.UpdateEnergyAmount(amountChanged[i]);
                            break;
                        case ResourceType.Security:
                            ship.UpdateSecurityAmount(amountChanged[i]);
                            break;
                        case ResourceType.ShipWeapons:
                            ship.UpdateShipWeaponsAmount(amountChanged[i]);
                            break;
                        case ResourceType.Crew:
                            ship.UpdateCrewAmount(amountChanged[i]);
                            break;
                        case ResourceType.Food:
                            ship.UpdateFoodAmount(amountChanged[i]);
                            break;
                        case ResourceType.FoodPerTick:
                            ship.UpdateFoodPerTickAmount(amountChanged[i]);
                            break;
                        case ResourceType.HullDurability:
                            ship.UpdateHullDurabilityAmount(amountChanged[i]);
                            break;
                        default:
                            break;
                    }

                }
            }
        }

    }
}

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
