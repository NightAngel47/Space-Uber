/*
 * ChoiceOutcomes.cs
 * Author(s): Sam Ferstein
 * Created on: 9/18/2020 (en-US)
 * Description: Controls all outcomes of choices. When the player chooses to do something, code is directed here to determine the effects
 * Effects are written in the inspector
 */

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class ChoiceOutcomes
{
    [SerializeField] private string outcomeName;
    
    [SerializeField] private bool isNarrativeOutcome;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] private ResourceType resource;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] private int amount;
    [Dropdown("cateringToRichBools"), 
     SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private string ctrBoolOutcomes;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int cloneTrustChange;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int VIPTrustChange;
    
    public void StatChange(ShipStats ship, CampaignManager campMan, bool hasSubsequentChoices)
    {
        if (ship != null)
        {
            
           if (!isNarrativeOutcome)
            {
                switch (resource)
                {
                    case ResourceType.Credits:
                        ship.UpdateCreditsAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.Energy:
                        ship.UpdateEnergyAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.Security:
                        ship.UpdateSecurityAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.ShipWeapons:
                        ship.UpdateShipWeaponsAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.Crew:
                        int amountFromAssigned;
                        int amountFromUnassigned;
                        if(ship.CrewCurrent - ship.CrewUnassigned >= amount)
                        {
                            amountFromAssigned = amount;
                            amountFromUnassigned = 0;
                        }
                        else
                        {
                            amountFromAssigned = ship.CrewCurrent - ship.CrewUnassigned;
                            amountFromUnassigned = amount - amountFromAssigned;
                        }
                        ship.RemoveRandomCrew(amountFromAssigned);
                        ship.UpdateCrewAmount(amountFromUnassigned, amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.Food:
                        ship.UpdateFoodAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.FoodPerTick:
                        ship.UpdateFoodPerTickAmount(amount);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.HullDurability:
                        ship.UpdateHullDurabilityAmount(amount, 0, hasSubsequentChoices);
                        SpawnStatChangeText(ship, amount);
                        break;
                    case ResourceType.Payout:
                        ship.AddPayout(amount);
                        SpawnStatChangeText(ship, amount);
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
                    case Campaigns.CateringToTheRich:
                        CampaignManager.CateringToTheRich campaign = (CampaignManager.CateringToTheRich) campMan.campaigns[(int)Campaigns.CateringToTheRich];

                        //alter the trust variables
                        campaign.ctr_cloneTrust += cloneTrustChange;
                        campaign.ctr_VIPTrust += VIPTrustChange;

                        //the selected bool will become true
                        switch (ctrBoolOutcomes)
                        {
                            case "Side With Scientist":
                                campaign.ctr_sideWithScientist = true;
                                break;
                            case "Kill Beckett":
                                campaign.ctr_killBeckett = true;
                                break;
                            case "Let Bale Pilot":
                                campaign.ctr_letBalePilot = true;
                                break;
                            case "Killed At Safari":
                                campaign.ctr_killedAtSafari = true;
                                break;
                            case "Tell VIPs About Clones":
                                campaign.ctr_tellVIPsAboutClones = true;
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
            return new List<string>() { "N_A", "Side With Scientist", "Kill Beckett", "Let Bale Pilot", "Killed At Safari", "Tell VIPs About Clones" };
        }
    }
    
    private void SpawnStatChangeText(ShipStats ship, int value)
    {
        GameObject statChangeText = ship.GetComponent<ShipStatsUI>().statChangeText;
        GameObject instance = GameObject.Instantiate(statChangeText);
        
        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
        instance.transform.parent = ship.GetComponent<ShipStatsUI>().canvas;
        
        MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, +75);
        moveAndFadeBehaviour.SetValue(value);
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
    Stock,
    Payout
}
