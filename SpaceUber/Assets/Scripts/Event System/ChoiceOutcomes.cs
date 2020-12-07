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
using TMPro;

[System.Serializable]
public class ChoiceOutcomes
{
    [SerializeField] private string outcomeName;

    [HideInInspector] public GameObject narrativeResultsBox;
    string resultText = "";

    [HideInInspector] public bool hasSubsequentChoices;

    [SerializeField] public bool isNarrativeOutcome;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] public ResourceType resource;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] public int amount;
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
                        SpawnStatChangeText(ship, amount, 0);

                        if(amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " credits";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " credits";
                        }

                        break;
                    case ResourceType.Energy:
                        ship.UpdateEnergyAmount(amount);
                        SpawnStatChangeText(ship, amount, 5);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " energy";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " energy";
                        }
                        break;
                    case ResourceType.Security:
                        ship.UpdateSecurityAmount(amount);
                        SpawnStatChangeText(ship, amount, 1);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " security";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " security";
                        }

                        break;
                    case ResourceType.ShipWeapons:
                        ship.UpdateShipWeaponsAmount(amount);
                        SpawnStatChangeText(ship, amount, 2);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " weapons";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " weapons";
                        }
                        break;
                    case ResourceType.Crew:
                        if(amount < 0)
                        {
                            int amountFromAssigned;
                            int amountFromUnassigned;
                            if(ship.CrewCurrent - ship.CrewUnassigned >= -amount)
                            {
                                amountFromAssigned = -amount;
                                amountFromUnassigned = 0;
                            }
                            else
                            {
                                amountFromAssigned = ship.CrewCurrent - ship.CrewUnassigned;
                                amountFromUnassigned = -amount - amountFromAssigned;
                            }
                            ship.RemoveRandomCrew(amountFromAssigned);
                            ship.UpdateCrewAmount(-amountFromUnassigned, amount);
                            SpawnStatChangeText(ship, amount);
                            resultText += "\nYou lost " + Math.Abs(amount) + " crew";
                        }
                        else
                        {
                            ship.UpdateCrewAmount(amount, amount);
                            SpawnStatChangeText(ship, amount);
                            resultText += "\nYou gained " + Math.Abs(amount) + " crew";
                        }
                        break;
                    case ResourceType.Food:
                        ship.UpdateFoodAmount(amount);
                        SpawnStatChangeText(ship, amount, 3);
                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " food";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " food";
                        }
                        break;
                    case ResourceType.FoodPerTick:
                        ship.UpdateFoodPerTickAmount(amount);
                        SpawnStatChangeText(ship, amount, 3);

                        if (amount < 0)
                        {
                            resultText += "\nFood Per Tick decreased by " + Math.Abs(amount);
                        }
                        else
                        {
                            resultText += "\nFood Per Tick increased by " + Math.Abs(amount);
                        }
                        break;
                    case ResourceType.HullDurability:
                        ship.UpdateHullDurabilityAmount(amount, 0, hasSubsequentChoices);
                        SpawnStatChangeText(ship, amount, 6);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " hull durability";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " hull durability";
                        }
                        break;
                    case ResourceType.Payout:
                        ship.AddPayout(amount);
                        SpawnStatChangeText(ship, amount, 0);
                        if (amount < 0)
                        {
                            resultText += "\nYour payout decreased by " + Math.Abs(amount);
                        }
                        else
                        {
                            resultText += "\nYour payout increased by " + Math.Abs(amount);
                        }
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
                        CampaignManager.CateringToTheRich campaign = CampaignManager.Campaign.ToCateringToTheRich(campMan.campaigns[(int)Campaigns.CateringToTheRich]);

                        //alter the trust variables
                        campaign.ctr_cloneTrust += cloneTrustChange;
                        campaign.ctr_VIPTrust += VIPTrustChange;

                        //the selected bool will become true
                        switch (ctrBoolOutcomes)
                        {
                            case "Side With Scientist":
                                campaign.ctr_sideWithScientist = true;
                                resultText += "\nYou sided with the scientist";
                                break;
                            case "Kill Beckett":
                                campaign.ctr_killBeckett = true;
                                resultText += "\nYou killed Beckett";
                                break;
                            case "Let Bale Pilot":
                                campaign.ctr_letBalePilot = true;
                                resultText += "\nYou let Bale pilot";
                                break;
                            case "Killed At Safari":
                                campaign.ctr_killedAtSafari = true;
                                resultText += "\nYou killed at the safari";
                                break;
                            case "Tell VIPs About Clones":
                                campaign.ctr_tellVIPsAboutClones = true;
                                resultText += "\nYou told the VIPs about the clones";
                                break;
                        }
                        break;

                }
                //TODO: Make resultText show up on the textbox somehow
                narrativeResultsBox.gameObject.SetActive(true);

                if(cloneTrustChange < 0)
                {
                    resultText += "\n The clones have " + cloneTrustChange + "% less trust in you";
                }
                else if(cloneTrustChange > 0)
                {
                    resultText += "\n The clones have " + cloneTrustChange + "% more trust in you";
                }

                if (VIPTrustChange < 0)
                {
                    resultText += "\n The VIPs have " + VIPTrustChange + "% less trust in you";
                }
                else if (VIPTrustChange > 0)
                {
                    resultText += "\n The VIPs have " + VIPTrustChange + "% more trust in you";
                }
                
            }
            if(!hasSubsequentChoices)
            {
                narrativeResultsBox.SetActive(true);
            }
            
            Debug.Log("Adding: " + resultText);
            narrativeResultsBox.transform.GetChild(0).GetComponent<TMP_Text>().text += resultText;
        }

    }

    private List<string> cateringToRichBools
    {
        get
        {
            return new List<string>() { "N_A", "Side With Scientist", "Kill Beckett", "Let Bale Pilot", "Killed At Safari", "Tell VIPs About Clones" };
        }
    }

    private void SpawnStatChangeText(ShipStats ship, int value, int icon = -1)
    {
        GameObject statChangeText = ship.GetComponent<ShipStatsUI>().statChangeText;
        GameObject instance = GameObject.Instantiate(statChangeText);

        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        instance.transform.parent = ship.GetComponent<ShipStatsUI>().canvas;

        MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, +75);
        moveAndFadeBehaviour.SetValue(value, icon);
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
