/*
 * ChoiceOutcomes.cs
 * Author(s): Sam Ferstein
 * Created on: 9/18/2020 (en-US)
 * Description: Controls all outcomes of choices. When the player chooses to do something, code is directed here to determine the effects
 * Effects are written in the inspector
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoiceOutcomes
{
    public bool isSetStatOutcome;
    public bool isRandomOutcome;
    public bool isNarrativeOutcome;

    [SerializeField, ShowIf("isSetStatOutcome"), AllowNesting] private ResourceType resource;
    [SerializeField, ShowIf("isSetStatOutcome"), AllowNesting] private int amount;

    [SerializeField, ShowIf("isRandomOutcome"), AllowNesting] private List<float> probabilities;
    [SerializeField, ShowIf("isRandomOutcome"), AllowNesting] private MultipleRandom[] multipleRandomOutcomes;

    [Dropdown("cateringToRichBools"), SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private string ctrBoolOutcomes;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private int cloneTrustChange;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private int VIPTrustChange;

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

    public void StatChange(ShipStats ship)
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
                switch (resource)
                {
                    case ResourceType.Credits:
                        ship.UpdateCreditsAmount(amount);
                        break;
                    case ResourceType.Energy:
                        ship.UpdateEnergyAmount(amount);
                        break;
                    case ResourceType.Security:
                        ship.UpdateSecurityAmount(amount);
                        break;
                    case ResourceType.ShipWeapons:
                        ship.UpdateShipWeaponsAmount(amount);
                        break;
                    case ResourceType.Crew:
                        ship.UpdateCrewAmount(amount);
                        break;
                    case ResourceType.Food:
                        ship.UpdateFoodAmount(amount);
                        break;
                    case ResourceType.FoodPerTick:
                        ship.UpdateFoodPerTickAmount(amount);
                        break;
                    case ResourceType.HullDurability:
                        ship.UpdateHullDurabilityAmount(amount);
                        break;
                    default:
                        break;
                }
            }
            else if(isNarrativeOutcome)
            {
                CampaignManager campMan = CampaignManager.instance;
                switch (campMan.currentCamp)
                {
                    //for catering to the rich campaign
                    case CampaignManager.Campaign.CateringToTheRich:
                        
                        //alter the trust variables
                        campMan.ctr_cloneTrust += cloneTrustChange;
                        campMan.ctr_VIPTrust += VIPTrustChange;

                        //the selected bool will become true
                        switch (ctrBoolOutcomes)
                        {
                            case "Side With Scientist":
                                campMan.ctr_sideWithScientist = true;
                                break;
                            case "Kill Beckett":
                                campMan.ctr_killBeckett = true;
                                break;
                            case "Killed At Safari":
                                campMan.ctr_killedAtSafari = true;
                                break;
                            case "Tell VIPs About Clones":
                                campMan.ctr_tellVIPsAboutClones = true;
                                break;
                        }
                        break;
                        
                }
            }
        }

    }

    private List<string> cateringToRichBools
    {
        get
        {
            return new List<string>() { "N_A", "Side With Scientist", "Kill Beckett", "Killed At Safari", "Tell VIPs About Clones" };
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
