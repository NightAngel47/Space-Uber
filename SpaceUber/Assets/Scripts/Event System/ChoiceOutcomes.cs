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
    [SerializeField] public bool isResourceOutcome;
    [SerializeField] public bool isApprovalOutcome;


    [SerializeField, ShowIf("isResourceOutcome"), AllowNesting] public ResourceType resource;
    [SerializeField, ShowIf("isResourceOutcome"), AllowNesting] public int resourceChange;

    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public ShipStats.Characters character;
    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public int approvalChange;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private CampaignManager.CateringToTheRich.NarrativeOutcomes ctrBoolOutcomes;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int cloneTrustChange;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int VIPTrustChange;

    public void StatChange(ShipStats ship, CampaignManager campMan, bool hasSubsequentChoices)
    {
        if (ship != null)
        {
            if (!isResourceOutcome)
            {
                switch (resource)
                {
                    case ResourceType.Credits:
                        ship.UpdateCreditsAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 0);

                        if(resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " credits";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " credits";
                        }

                        break;
                    case ResourceType.Energy:
                        ship.UpdateEnergyAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 5);

                        if (resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " energy";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " energy";
                        }
                        break;
                    case ResourceType.Security:
                        ship.UpdateSecurityAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 1);

                        if (resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " security";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " security";
                        }

                        break;
                    case ResourceType.ShipWeapons:
                        ship.UpdateShipWeaponsAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 2);

                        if (resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " weapons";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " weapons";
                        }
                        break;
                    case ResourceType.Crew:
                        if(resourceChange < 0)
                        {
                            int amountFromAssigned;
                            int amountFromUnassigned;
                            if(ship.CrewCurrent - ship.CrewUnassigned >= -resourceChange)
                            {
                                amountFromAssigned = -resourceChange;
                                amountFromUnassigned = 0;
                            }
                            else
                            {
                                amountFromAssigned = ship.CrewCurrent - ship.CrewUnassigned;
                                amountFromUnassigned = -resourceChange - amountFromAssigned;
                            }
                            ship.RemoveRandomCrew(amountFromAssigned);
                            ship.UpdateCrewAmount(-amountFromUnassigned, resourceChange);
                            SpawnStatChangeText(ship, resourceChange);
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " crew";
                        }
                        else
                        {
                            ship.UpdateCrewAmount(resourceChange, resourceChange);
                            SpawnStatChangeText(ship, resourceChange);
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " crew";
                        }
                        break;
                    case ResourceType.Food:
                        ship.UpdateFoodAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 3);
                        if (resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " food";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " food";
                        }
                        break;
                    case ResourceType.FoodPerTick:
                        ship.UpdateFoodPerTickAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 3);

                        if (resourceChange < 0)
                        {
                            resultText += "\nFood Per Tick decreased by " + Math.Abs(resourceChange);
                        }
                        else
                        {
                            resultText += "\nFood Per Tick increased by " + Math.Abs(resourceChange);
                        }
                        break;
                    case ResourceType.HullDurability:
                        ship.UpdateHullDurabilityAmount(resourceChange, 0, hasSubsequentChoices);
                        SpawnStatChangeText(ship, resourceChange, 6);

                        if (resourceChange < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(resourceChange) + " hull durability";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(resourceChange) + " hull durability";
                        }
                        break;
                    case ResourceType.Payout:
                        ship.UpdatePayoutAmount(resourceChange);
                        SpawnStatChangeText(ship, resourceChange, 0);
                        if (resourceChange < 0)
                        {
                            resultText += "\nYour payout decreased by " + Math.Abs(resourceChange);
                        }
                        else
                        {
                            resultText += "\nYour payout increased by " + Math.Abs(resourceChange);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if(isNarrativeOutcome)
            {
                //alter the trust variables
                campMan.cateringToTheRich.ctr_cloneTrust += cloneTrustChange;
                campMan.cateringToTheRich.ctr_VIPTrust += VIPTrustChange;

                //the selected bool will become true
                switch (ctrBoolOutcomes)
                {
                    case CampaignManager.CateringToTheRich.NarrativeOutcomes.SideWithScientist:
                        campMan.cateringToTheRich.ctr_sideWithScientist = true;
                        resultText += "\nYou sided with the scientist";
                        break;
                    case CampaignManager.CateringToTheRich.NarrativeOutcomes.KillBeckett:
                        campMan.cateringToTheRich.ctr_killBeckett = true;
                        campMan.cateringToTheRich.ctr_killedOnce = true;

                        resultText += "\nYou killed Beckett";
                        break;
                    case CampaignManager.CateringToTheRich.NarrativeOutcomes.LetBalePilot:
                        campMan.cateringToTheRich.ctr_letBalePilot = true;
                        resultText += "\nYou let Bale pilot";
                        break;
                    case CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledAtSafari:
                        campMan.cateringToTheRich.ctr_killedAtSafari = true;
                        
                        if(campMan.cateringToTheRich.ctr_killedOnce == true) //killed beckett as well
                        {
                            campMan.cateringToTheRich.ctr_killedOnce = false;
                        }
                        else //no kills yet
                        {
                            campMan.cateringToTheRich.ctr_killedOnce = true;
                        }

                        

                        resultText += "\nYou killed at the safari";
                        break;
                    case CampaignManager.CateringToTheRich.NarrativeOutcomes.TellVIPsAboutClones:
                        campMan.cateringToTheRich.ctr_tellVIPsAboutClones = true;
                        resultText += "\nYou told the VIPs about the clones";
                        break;
                    default:
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
            else
            {
                ship.UpdateCrewMemberApproval(character, approvalChange);
            }
            if(!hasSubsequentChoices)
            {
                narrativeResultsBox.SetActive(true);
            }
            
            //Debug.Log("Adding: " + resultText);
            narrativeResultsBox.transform.GetChild(0).GetComponent<TMP_Text>().text += resultText;
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
