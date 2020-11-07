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
    public string outcomeName;
    public bool isNarrativeOutcome;

    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] private ResourceType resource;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] private int amount;

    [Dropdown("cateringToRichBools"), SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private string ctrBoolOutcomes;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private int cloneTrustChange;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting]
    private int VIPTrustChange;


    public void StatChange(ShipStats ship, CampaignManager campMan)
    {
        if (ship != null)
        {
            
           if (!isNarrativeOutcome)
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
            else 
            {
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
