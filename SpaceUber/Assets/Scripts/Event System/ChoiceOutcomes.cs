/*
 * ChoiceOutcomes.cs
 * Author(s): Sam Ferstein, Scott Acker
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
    [HideInInspector] public bool isScaledOutcome;

    [HideInInspector] public GameObject narrativeResultsBox;
    string resultText = "";

    [HideInInspector] public bool hasSubsequentChoices;

    [SerializeField] public bool isNarrativeOutcome;
    [SerializeField] public bool isResourceOutcome;
    [SerializeField] public bool isMutinyOutcome;
    [SerializeField] public bool isApprovalOutcome;

    [SerializeField, ShowIf("isResourceOutcome"), AllowNesting] public ResourceDataTypes resource;
    [SerializeField, ShowIf("isResourceOutcome"), AllowNesting] public int amount;

    //[SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public CharacterStats.Characters character = CharacterStats.Characters.None;
    [SerializeField, ShowIf("isApprovalOutcome"), AllowNesting] public CharacterEvent.AnswerState answerType;
    [HideInInspector] public CharacterEvent characterDriver;
    CampaignManager campMan;

    #region Initialized Narrative Variables
    [SerializeField, ShowIf("isNarrativeOutcome"), AllowNesting] private CampaignManager.Campaigns thisCampaign = CampaignManager.Campaigns.CateringToTheRich;

    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsCateringToTheRich"), AllowNesting] private CampaignManager.CateringToTheRich.NarrativeOutcomes ctrBoolOutcomes;
    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsCateringToTheRich"), AllowNesting] private int cloneTrustChange;
    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsCateringToTheRich"), AllowNesting] private int VIPTrustChange;

    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsMysteriousEntity"), AllowNesting] private CampaignManager.MysteriousEntity.NarrativeVariables meMainOutcomes;
    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsFinalTest"), AllowNesting] private CampaignManager.FinalTest.NarrativeVariables finalTestNarrativeOutcomes;
    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeOutcome", "IsFinalTest"), AllowNesting] private int assetCountChange = 0;

    #endregion

    #region Check for Campaign
    public bool IsCateringToTheRich()
    {
        return thisCampaign == CampaignManager.Campaigns.CateringToTheRich;
    }

    public bool IsMysteriousEntity()
    {
        return thisCampaign == CampaignManager.Campaigns.MysteriousEntity;
    }

    public bool IsFinalTest()
    {
        return thisCampaign == CampaignManager.Campaigns.FinalTest;
    }
    #endregion

    public void StatChange(ShipStats ship, CampaignManager campMan, bool hasSubsequentChoices)
    {
        if (ship != null)
        {
            if (isResourceOutcome || (!isNarrativeOutcome && !isApprovalOutcome)) //Will change to "isResourceOutcome" when designers have the chance to check the box in all old events
            {
                if(isScaledOutcome) //scalable events get a multiplier to amount
                {
                    int newAmount = Mathf.RoundToInt(amount * campMan.GetMultiplier(resource));

                    switch (resource)
                    {

                        case ResourceDataTypes._Credits:

                            ship.Credits += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " credits";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " credits";
                            }

                            break;
                        case ResourceDataTypes._Energy:
                            
                            ship.Energy += new Vector3(newAmount, 0, 0);
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " energy";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " energy";
                            }
                            break;
                        case ResourceDataTypes._Security:

                            ship.Security += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " security";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " security";
                            }

                            break;
                        case ResourceDataTypes._ShipWeapons:

                            ship.ShipWeapons += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " weapons";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " weapons";
                            }
                            break;
                        case ResourceDataTypes._Crew: //losing crew
                            if (newAmount < 0)
                            {

                                ship.CrewCurrent += new Vector3(newAmount, 0, 0);
                                SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " crew";
                            }
                            else
                            {
                                ship.CrewCurrent += new Vector3(newAmount, 0, 0);
                                SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " crew";
                            }
                            break;
                        case ResourceDataTypes._Food:

                            ship.Food += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " food";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " food";
                            }
                            break;
                        case ResourceDataTypes._FoodPerTick:

                            ship.FoodPerTick += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._FoodPerTick).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nFood Per Tick decreased by " + Math.Abs(newAmount);
                            }
                            else
                            {
                                resultText += "\nFood Per Tick increased by " + Math.Abs(newAmount);
                            }
                            break;
                        case ResourceDataTypes._HullDurability:

                            ship.ShipHealthCurrent += new Vector2(newAmount, 0);
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
                            if (hasSubsequentChoices && ship.ShipHealthCurrent.x <= 0)
                            {
                                ship.CheckForDeath();
                            }

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(newAmount) + " hull durability";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(newAmount) + " hull durability";
                            }
                            break;

                        case ResourceDataTypes._Payout:
                            ship.Payout += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Payout).resourceIcon);
                            if (newAmount < 0)
                            {
                                resultText += "\nYour payout decreased by " + Math.Abs(newAmount);
                            }
                            else
                            {
                                resultText += "\nYour payout increased by " + Math.Abs(newAmount);
                            }
                            break;

                        case ResourceDataTypes._CrewMorale:
                            MoraleManager.instance.CrewMorale += newAmount;
                            SpawnStatChangeText(ship, newAmount, GameManager.instance.GetResourceData((int)ResourceDataTypes._CrewMorale).resourceIcon);

                            if (newAmount < 0)
                            {
                                resultText += "\nYou lost crew morale";
                            }
                            else
                            {
                                resultText += "\nYou gained crew morale";
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (resource)
                    {
                        case ResourceDataTypes._Credits:
                            ship.Credits += amount;
                            SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon);

                            if (amount < 0)
                            {
                                resultText += "\nYou lost " + Math.Abs(amount) + " credits";
                            }
                            else
                            {
                                resultText += "\nYou gained " + Math.Abs(amount) + " credits";
                            }

                            break;
                        case ResourceDataTypes._Energy:
                            ship.Energy += new Vector3(amount, 0, 0);
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
                            if (amount < 0)
                            {

                                ship.CrewCurrent += new Vector3(amount, 0, 0);
                                SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
                                resultText += "\nYou lost " + Math.Abs(amount) + " crew";
                            }
                            else
                            {
                                ship.CrewCurrent += new Vector3(amount, 0, 0);
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
                            if (hasSubsequentChoices && ship.ShipHealthCurrent.x <= 0)
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
                        case ResourceDataTypes._CrewMorale:
                            MoraleManager.instance.CrewMorale += amount;
                            SpawnStatChangeText(ship, amount, GameManager.instance.GetResourceData((int)ResourceDataTypes._CrewMorale).resourceIcon);

                            if (amount < 0)
                            {
                                resultText += "\nYou lost crew morale";
                            }
                            else
                            {
                                resultText += "\nYou gained crew morale";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else if(isNarrativeOutcome)
            {
                switch(thisCampaign)
                {
                    case CampaignManager.Campaigns.CateringToTheRich:
                        
                        //alter the trust variables
                        campMan.cateringToTheRich.ctr_cloneTrust += cloneTrustChange;
                        campMan.cateringToTheRich.ctr_VIPTrust += VIPTrustChange;

                        //the selected bool will become true
                        if(ctrBoolOutcomes != CampaignManager.CateringToTheRich.NarrativeOutcomes.NA)
                            campMan.cateringToTheRich.SetCtrNarrativeOutcome(ctrBoolOutcomes, true);
                        
                        switch (ctrBoolOutcomes)
                        {
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.SideWithScientist:
                                resultText += "\nYou sided with Lanri";
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.KillBeckett:
                                campMan.cateringToTheRich.SetCtrNarrativeOutcome(CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledOnce, true);
                                resultText += "\nYou killed Beckett";
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.LetBalePilot:
                                resultText += "\nYou let Bale pilot";
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledAtSafari:
                                campMan.cateringToTheRich.SetCtrNarrativeOutcome(CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledOnce, 
                                    !campMan.cateringToTheRich.GetCtrNarrativeOutcome(CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledOnce));
                                resultText += "\nYou killed at the safari";
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.TellVIPsAboutClones:
                                resultText += "\nYou told the VIPs about the clones";
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.NA:
                                break;
                        }

                        if (cloneTrustChange < 0)
                        {
                            resultText += "\nThe clones have " + cloneTrustChange + "% less trust in you";
                        }
                        else if (cloneTrustChange > 0)
                        {
                            resultText += "\nThe clones have " + cloneTrustChange + "% more trust in you";
                        }

                        if (VIPTrustChange < 0)
                        {
                            resultText += "\nThe VIPs have " + VIPTrustChange + "% less trust in you";
                        }
                        else if (VIPTrustChange > 0)
                        {
                            resultText += "\nThe VIPs have " + VIPTrustChange + "% more trust in you";
                        }
                        break;
                        
                    case CampaignManager.Campaigns.MysteriousEntity:
                        //the selected bool will become true
                        if(meMainOutcomes != CampaignManager.MysteriousEntity.NarrativeVariables.NA)
                            campMan.mysteriousEntity.SetMeNarrativeVariable(meMainOutcomes, true);
                        
                        switch (meMainOutcomes)
                        {
                            case CampaignManager.MysteriousEntity.NarrativeVariables.KuonInvestigates:
                                resultText += "\nYou allowed Kuon to investigate";
                                break;
                            case CampaignManager.MysteriousEntity.NarrativeVariables.OpenedCargo:
                                resultText += "\nYou opened the cargo";
                                break;

                            case CampaignManager.MysteriousEntity.NarrativeVariables.Accept:
                                resultText += "\nYou accepted the offer";
                                break;
                            case CampaignManager.MysteriousEntity.NarrativeVariables.Decline_Bribe:
                                resultText += "\nYou declined the offer and bribed Loudon to stay";
                                break;
                            case CampaignManager.MysteriousEntity.NarrativeVariables.Decline_Fire:
                                resultText += "\nYou declined the offer and said good riddance to Loudon";
                                break;
                            case CampaignManager.MysteriousEntity.NarrativeVariables.NA:
                                break;
                        }
                        break;
                    case CampaignManager.Campaigns.FinalTest:
                        campMan.finalTest.assetCount += assetCountChange;

                        if (assetCountChange > 0)
                        {
                            if (assetCountChange == 1)
                                resultText += "\nYou have gained 1 asset";
                            else
                                resultText += "\nYou have gained " + assetCountChange + " assets";
                        }
                        else if (assetCountChange < 0)
                        {
                            if (assetCountChange == 1)
                                resultText += "\nYou have lost 1 asset";
                            else
                                resultText += "\nYou have lost " + assetCountChange + " assets";
                        }

                        if(finalTestNarrativeOutcomes != CampaignManager.FinalTest.NarrativeVariables.NA)
                            campMan.finalTest.SetFtNarrativeVariable(finalTestNarrativeOutcomes, true);
                        
                        switch (finalTestNarrativeOutcomes)
                        {
                            case CampaignManager.FinalTest.NarrativeVariables.KellisLoyalty:
                                resultText += "\nYou have sided with Kellis";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.LanriExperiment:
                                resultText += "\nYou have allowed Lanri to experiment";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.LexaDoomed:
                                resultText += "\nYou left Lexa to face her doom alone";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.ScienceSavior:
                                resultText += "\nYou told Lanri to fix the cataclysm";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.TruthTold:
                                resultText += "\nYou told everyone the truth";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.LexaPlan:
                                resultText += "\nYou went with Lexa's plan";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.MateoPlan:
                                resultText += "\nYou went with Mateo's plan";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.LanriRipleyPlan:
                                resultText += "\nYou went with Lanri and Ripley's plan";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.KuonPlan:
                                resultText += "\nYou went with Kuon's plan";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.ResearchShared:
                                resultText += "\nYou shared your research";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.AncientHackingDevice:
                                resultText += "\nYou bought the ancient hacking device";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.ExoSuits:
                                resultText += "\nYou bought the exosuits";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.WarpShields:
                                resultText += "\nYou bought the warp shields";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.RealityBomb:
                                resultText += "\nYou bought the reality bomb";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.DisintegrationRay:
                                resultText += "\nYou bought disintegration ray";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.ArtifactAngry:
                                resultText += "\nYou angered the alien artifact";
                                break;
                            case CampaignManager.FinalTest.NarrativeVariables.NA:
                                break;
                        }
                        break;
                }
            }
            else if (isMutinyOutcome)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Mutiny);
            }
            else //approval outcomes
            {
                int eventApprovalChange = 0;

                if(answerType == CharacterEvent.AnswerState.POSITIVE)
                {
                    eventApprovalChange = 1;
                }
                if (answerType == CharacterEvent.AnswerState.NEGATIVE)
                {
                    eventApprovalChange = -1;
                }
                if (answerType == CharacterEvent.AnswerState.NEUTRAL)
                {
                    eventApprovalChange = 0;
                }

                characterDriver.ChangeEventApproval(eventApprovalChange);

                switch (characterDriver.Character)
                {
                    case CharacterStats.Characters.Kuon:
                        ship.cStats.KuonApproval += eventApprovalChange;
                        break;
                    case CharacterStats.Characters.Mateo:
                        ship.cStats.MateoApproval += eventApprovalChange;
                        break;
                    case CharacterStats.Characters.Lanri:
                        ship.cStats.LanriApproval += eventApprovalChange;
                        break;
                    case CharacterStats.Characters.Lexa:
                        ship.cStats.LexaApproval += eventApprovalChange;
                        break;
                    case CharacterStats.Characters.Ripley:
                        ship.cStats.RipleyApproval += eventApprovalChange;
                        break;
                }
            }

            if(!hasSubsequentChoices) //do at the end of the event
            {
                narrativeResultsBox.SetActive(true);
            }

            //Debug.Log("Adding: " + resultText);
            narrativeResultsBox.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text += resultText;
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
