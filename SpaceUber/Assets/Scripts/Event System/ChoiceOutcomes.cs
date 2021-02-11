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

    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] public ResourceDataTypes resource;
    [SerializeField, HideIf("isNarrativeOutcome"), AllowNesting] public int amount;

    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public CharacterStats.Characters character = CharacterStats.Characters.None;
    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public int approvalChange;
    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public bool correctAnswer;
    [HideInInspector] public CharacterEvent characterDriver;

    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private CampaignManager.CateringToTheRich.NarrativeOutcomes ctrBoolOutcomes;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int cloneTrustChange;
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private int VIPTrustChange;
    [SerializeField] public bool changeGameState;
    [SerializeField, ShowIf("changeGameState"), AllowNesting] public InGameStates state;

    public void StatChange(ShipStats ship, CampaignManager campMan, bool hasSubsequentChoices)
    {
        if (ship != null)
        {
            if (!isNarrativeOutcome && !isApprovalOutcome) //Will change to "isResourceOutcome" when designers have the chance to check the box in all old events
            {
                switch (resource)
                {
                    case ResourceDataTypes._Credits:
                        ship.Credits += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon);

                        if(amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " credits";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " credits";
                        }

                        break;
                    case ResourceDataTypes._Energy:
                        ship.EnergyRemaining += new Vector2(amount, 0);
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " energy";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " energy";
                        }
                        break;
                    case ResourceDataTypes._Security:
                        ship.Security += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " security";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " security";
                        }

                        break;
                    case ResourceDataTypes._ShipWeapons:
                        ship.ShipWeapons += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " weapons";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " weapons";
                        }
                        break;
                    case ResourceDataTypes._Crew:
                        if(amount < 0)
                        {
                            int amountFromAssigned;
                            int amountFromUnassigned;
                            if(ship.CrewCurrent.x - ship.CrewCurrent.z >= -amount)
                            {
                                amountFromAssigned = -amount;
                                amountFromUnassigned = 0;
                            }
                            else
                            {
                                amountFromAssigned = (int)ship.CrewCurrent.x - (int)ship.CrewCurrent.z;
                                amountFromUnassigned = -amount - amountFromAssigned;
                            }
                            ship.RemoveRandomCrew(amountFromAssigned);
                            ship.CrewCurrent += new Vector3(amount, -amountFromUnassigned, 0);
                            SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
                            resultText += "\nYou lost " + Math.Abs(amount) + " crew";
                        }
                        else
                        {
                            ship.CrewCurrent += new Vector3(amount, amount, 0);
                            SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
                            resultText += "\nYou gained " + Math.Abs(amount) + " crew";
                        }
                        break;
                    case ResourceDataTypes._Food:
                        ship.Food += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " food";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " food";
                        }
                        break;
                    case ResourceDataTypes._FoodPerTick:
                        ship.FoodPerTick += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._FoodPerTick).resourceIcon);

                        if (amount < 0)
                        {
                            resultText += "\nFood Per Tick decreased by " + Math.Abs(amount);
                        }
                        else
                        {
                            resultText += "\nFood Per Tick increased by " + Math.Abs(amount);
                        }
                        break;
                    case ResourceDataTypes._HullDurability:
                        ship.ShipHealthCurrent += new Vector2(amount, 0);
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
                        if(hasSubsequentChoices && ship.ShipHealthCurrent.x <= 0)
                        {
                            ship.CheckForDeath();
                        }

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " hull durability";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " hull durability";
                        }
                        break;
                    case ResourceDataTypes._Payout:
                        ship.Payout += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Payout).resourceIcon);
                        if (amount < 0)
                        {
                            resultText += "\nYour payout decreased by " + Math.Abs(amount);
                        }
                        else
                        {
                            resultText += "\nYour payout increased by " + Math.Abs(amount);
                        }
                        break;
                    case ResourceDataTypes._Morale:
                        MoraleManager.instance.CrewMorale += amount;
                        SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Morale).resourceIcon);

                        if (amount < 0)
                        {
                            resultText += "\nYou lost " + Math.Abs(amount) + " crew morale";
                        }
                        else
                        {
                            resultText += "\nYou gained " + Math.Abs(amount) + " crew morale";
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
            else //approval outcomes
            {
                if(correctAnswer)
                {
                    characterDriver.AnswerCorrectly();
                }

                switch (character)
                {
                    case CharacterStats.Characters.KUON:
                        ship.cStats.KuonApproval += approvalChange;
                        break;
                    case CharacterStats.Characters.MATEO:
                        ship.cStats.MateoApproval += approvalChange;
                        break;
                    case CharacterStats.Characters.LANRI:
                        ship.cStats.LanriApproval += approvalChange;
                        break;
                    case CharacterStats.Characters.LEXA:
                        ship.cStats.LexaApproval += approvalChange;
                        break;
                    case CharacterStats.Characters.RIPLEY:
                        ship.cStats.RipleyApproval += approvalChange;
                        break;
                }
            }

            if(!hasSubsequentChoices)
            {
                narrativeResultsBox.SetActive(true);
            }

            if(changeGameState)
            {
                GameManager.instance.ChangeInGameState(state);
            }

            //Debug.Log("Adding: " + resultText);
            narrativeResultsBox.transform.GetChild(0).GetComponent<TMP_Text>().text += resultText;
        }

    }

    /// <summary>
    /// Assigns the corresponding character event driver to this choice and outcomes. Only used for character events
    /// </summary>
    public void AssignCharacterDriver(CharacterEvent driver)
    {
        characterDriver = driver;
    }

    private void SpawnStatChangeText(ShipStats ship, int value, Sprite icon)
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
