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
public class ChoiceOutcomes : MonoBehaviour
{
    [Tooltip("To keep track of which choice this is")]
    public string choiceName;
    ShipStats shipStats;
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
    private float choiceThreshold = 0f;

    [System.Serializable]
    public class MultipleRandom
    {
        public List<ResourceType> resourcesChanged;
        public List<int> amountChanged;
    }

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void ChoiceChange()
    {
        if (shipStats != null)
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
                                    shipStats.UpdateCreditsAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Energy:
                                    shipStats.UpdateEnergyAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Security:
                                    shipStats.UpdateSecurityAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.ShipWeapons:
                                    shipStats.UpdateShipWeaponsAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Crew:
                                    shipStats.UpdateCrewAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.Food:
                                    shipStats.UpdateFoodAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.FoodPerTick:
                                    shipStats.UpdateFoodPerTickAmount(multipleRandomOutcomes[i].amountChanged[j]);
                                    break;
                                case ResourceType.HullDurability:
                                    shipStats.UpdateHullDurabilityAmount(multipleRandomOutcomes[i].amountChanged[j]);
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
