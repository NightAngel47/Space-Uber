/*
 * EventRequirements.cs
 * Author(s): Scott Acker
 * Created on: 9/25/2020 
 * Description: Stores information about the requirements of either a choice or a job. Serializable and meant to be applied to 
 * different classes as a variable, not a script
 */

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

[Serializable]
public class Requirements
{
    public enum ResourceType
    {
        HULL,
        ENERGY,
        CREW,
        FOOD,
        WEAPONS,
        SECURITY,
        CREDITS,
        //MORALE
    }
    [Tooltip("If the requirement is narrative-based")]
    [SerializeField, AllowNesting]
    private bool isNarrativeRequirement;

    [Tooltip("The resource you would like to be compared")]
    [SerializeField, HideIf("isNarrativeRequirement"), AllowNesting]
    private ResourceType selectedResource;
    
    [Tooltip("How much of this resources is required for an event to run")]
    [SerializeField, HideIf("isNarrativeRequirement"), AllowNesting]
    private int requiredAmount;

    [Tooltip("Click this if you would like to check if the ship resource is LESS than the number supplied")]
    [SerializeField, HideIf("isNarrativeRequirement"),AllowNesting]
    private bool lessThan;

    [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run")]
    [Dropdown("cateringToRichBools"), SerializeField, ShowIf("isNarrativeRequirement"), AllowNesting]
    private string ctrBoolRequirements;

    [Tooltip("Click this if you would like to check trust variables for Catering to the Rich")]
    [SerializeField, ShowIf("isNarrativeRequirement"), AllowNesting]
    private bool ctrTrustRequirements = false;

    [Tooltip("The minimum trust the clones must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting]
    private int cloneTrustRequirement;

    [Tooltip("The minimum trust the VIPS must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting] 
    private int VIPTrustRequirement;

    //
    //public string campaign;
    //[HideInInspector] public List<string> PossibleCampaigns => new List<string>() { "NA", "Catering to the Rich" };

    public bool MatchesRequirements(ShipStats thisShip)
    {
        bool result = true;

        if (!isNarrativeRequirement)
        {
            int shipStat = 0;
            switch (selectedResource)
            {
                case ResourceType.HULL:
                    shipStat = thisShip.ShipHealthCurrent;
                    break;
                case ResourceType.ENERGY:
                    shipStat = thisShip.EnergyRemaining;
                    break;
                case ResourceType.CREW:
                    shipStat = thisShip.CrewRemaining;
                    break;
                case ResourceType.FOOD:
                    shipStat = thisShip.Food;
                    break;
                case ResourceType.WEAPONS:
                    shipStat = thisShip.ShipWeapons;
                    break;
                case ResourceType.SECURITY:
                    shipStat = thisShip.Security;
                    break;
                //case ResourceType.MORALE:
                //    shipStat = thisShip.Morale;
                //    break;
                case ResourceType.CREDITS:
                    shipStat = thisShip.Credits;
                    break;
            }

            if (!lessThan)
            {
                result = shipStat > requiredAmount;
            }
            else
            {
                result = shipStat < requiredAmount;
            }
        }
        else
        {
            CampaignManager campMan = CampaignManager.instance;
            switch(campMan.currentCamp)
            {
                //for catering to the rich campaign
                case CampaignManager.Campaign.CateringToTheRich:
                    //check if the selected bool is true or not
                    switch(ctrBoolRequirements)
                    {
                        case "Side With Scientist":
                            result = campMan.ctr_sideWithScientist;
                            break;
                        case "Kill Beckett":
                            result = campMan.ctr_killBeckett;
                            break;
                        case "Killed At Safari":
                            result = campMan.ctr_killedAtSafari;
                            break;
                        case "Tell VIPs About Clones":
                            result = campMan.ctr_tellVIPsAboutClones;
                            break;
                        case "N_A":
                            break;
                    }
                    if(ctrTrustRequirements)
                    {
                        bool VIPResult = campMan.ctr_VIPTrust > VIPTrustRequirement;
                        bool cloneResult = campMan.ctr_cloneTrust > cloneTrustRequirement;
                        result = VIPResult && cloneResult;
                    }
                    break;
            }
        }
        

        return result;
    }

    private List<string> cateringToRichBools
    {
        get
        {
            return new List<string>() { "N_A", "Side With Scientist", "Kill Beckett", "Killed At Safari", "Tell VIPs About Clones" };
        }
    }
}
