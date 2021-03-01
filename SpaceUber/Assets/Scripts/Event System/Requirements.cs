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
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Requirements
{
    #region Stat Requirement Stuff

    [Tooltip("If the requirement is stat-based")]
    [SerializeField, AllowNesting]
    private bool isStatRequirement = false;

    [Tooltip("The resource you would like to be compared")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    private ResourceDataTypes selectedResource;

    [Tooltip("How much of this resources is required for an event to run")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    public int requiredAmount;

    [Tooltip("Click this if you would like to check if the ship resource is LESS than the number supplied")]
    [SerializeField, ShowIf("isStatRequirement"),AllowNesting]
    private bool lessThan = false;
    #endregion

    #region Character Approval Variables

    [Tooltip("If the requirement is approval-based")]
    [SerializeField, AllowNesting]
    private bool isApprovalRequirement = false;

    [Tooltip("The character who's approval must be checked")]
    [SerializeField, ShowIf("isApprovalRequirement"), AllowNesting]
    private CharacterStats.Characters character = CharacterStats.Characters.None;

    [Tooltip("The required approval rating for this event to pass")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    private int requiredApproval;

    [Tooltip("Whether or not the approval must be LESS than the number supplied")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    private bool lessThanApproval = false;

    #endregion

    #region Narrative Requirement Variables
    
    [Tooltip("If the requirement is narrative-based")]
    [SerializeField, AllowNesting]
    private bool isNarrativeRequirement = false;

    [SerializeField, ShowIf("isNarrativeRequirement"), AllowNesting]
    private CampaignManager.Campaigns thisCampaign
        = CampaignManager.Campaigns.CateringToTheRich;

    #region Catering to the Rich


    [FormerlySerializedAs("ctrBoolRequirements")]
    [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run"),
     SerializeField, ShowIf(EConditionOperator.And, "isNarrativeRequirement", "IsCateringToTheRich"), AllowNesting]
    private CampaignManager.CateringToTheRich.NarrativeOutcomes ctrNarrativeOutcomes;

    [Tooltip("Click this if you would like to check trust variables for Catering to the Rich")]
    [SerializeField, ShowIf(EConditionOperator.And, "isNarrativeRequirement", "IsCateringToTheRich"), AllowNesting]
    private bool ctrTrustRequirements = false;

    [Tooltip("The minimum trust the clones must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting]
    private int cloneTrustRequirement;

    [Tooltip("The minimum trust the VIPS must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting]
    private int VIPTrustRequirement;
    #endregion

    #region Mysterious Entity Narrative Variables

    [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run"),
    SerializeField, ShowIf("IsMysteriousEntity"), AllowNesting]
    private CampaignManager.MysteriousEntity.NarrativeVariables meNarrativeRequirements = CampaignManager.MysteriousEntity.NarrativeVariables.NA;

    #endregion

    #region Final Test narrative variables
        [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run"),
         SerializeField, ShowIf("IsFinalTest"), AllowNesting]
        private CampaignManager.FinalTest.NarrativeVariables ftOutcomes = CampaignManager.FinalTest.NarrativeVariables.NA;

        [Tooltip("Check this if this is an asset count requirement"),
         SerializeField, ShowIf("IsFinalTest"), AllowNesting]
        private bool ftAssetCheck = false;

        [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run"),
         SerializeField, ShowIf("ftAssetCheck"), AllowNesting]
        private int requiredAssetCount = 0;

        [Tooltip("Check this if the requirement is to have an asset count LESS than the number above"),
         SerializeField, ShowIf("ftAssetCheck"), AllowNesting]
        private bool lessThanAssetCount;
    #endregion
    #endregion

    #region Room Requirement Variables
    public enum RoomType
    {
        ArmorPlating,
        Armory,
        Brig,
        Bunks,
        CoreChargingTerminal,
        EnergyCanon,
        HydroponicsStation,
        Medbay,
        PhotonTorpedoes,
        Pantry,
        PowerCore,
        ShieldGenerator,
        StorageContainer,
        TeleportationStation,
        VIPLounge,
        WarpDrive

    }

    [Tooltip("If the requirement is based on a room")]
    [SerializeField, AllowNesting]
    private bool isRoomRequirement = false;

    [Tooltip("The room that needs to exist on the ship")]
    [SerializeField, ShowIf("isRoomRequirement"), AllowNesting]
    //private GameObject necessaryRoomPrefab;
    private RoomType necessaryRoom;
    #endregion

    #region campaign checks
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
    //
    //public string campaign;
    //[HideInInspector] public List<string> PossibleCampaigns => new List<string>() { "NA", "Catering to the Rich" };

    public bool MatchesRequirements(ShipStats thisShip, CampaignManager campMan)
    {
        bool result = true;

        if (isStatRequirement)
        {
            int shipStat = 0;
            switch (selectedResource)
            {
                case ResourceDataTypes._HullDurability:
                    shipStat = (int)thisShip.ShipHealthCurrent.x;
                    break;
                case ResourceDataTypes._Energy:
                    shipStat = (int)thisShip.EnergyRemaining.x;
                    break;
                case ResourceDataTypes._Crew:
                    shipStat = (int)thisShip.CrewCurrent.x;
                    break;
                case ResourceDataTypes._Food:
                    shipStat = thisShip.Food;
                    break;
                case ResourceDataTypes._ShipWeapons:
                    shipStat = thisShip.ShipWeapons;
                    break;
                case ResourceDataTypes._Security:
                    shipStat = thisShip.Security;
                    break;
                case ResourceDataTypes._CrewMorale:
                    shipStat = MoraleManager.instance.CrewMorale;
                    break;
                case ResourceDataTypes._Credits:
                    shipStat = thisShip.Credits;
                    break;
            }

            if (lessThan)
            {
                result = shipStat < requiredAmount;
            }
            else
            {
                result = shipStat > requiredAmount;
            }
        }
        else if (isNarrativeRequirement)
        {
            switch(thisCampaign)
            {
                case CampaignManager.Campaigns.CateringToTheRich:
                    //check if the selected bool is true or not
                    
                    if (ctrTrustRequirements)
                    {

                        switch (ctrNarrativeOutcomes)
                        {
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.VIPTrust:
                                result = campMan.cateringToTheRich.ctr_VIPTrust >= VIPTrustRequirement;
                                break;
                            case CampaignManager.CateringToTheRich.NarrativeOutcomes.CloneTrust:
                                result = campMan.cateringToTheRich.ctr_cloneTrust >= cloneTrustRequirement;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        result = campMan.cateringToTheRich.GetCtrNarrativeOutcome(ctrNarrativeOutcomes);
                    }
                    break;

                case CampaignManager.Campaigns.MysteriousEntity:
                    result = campMan.mysteriousEntity.GetMeNarrativeVariable(meNarrativeRequirements);
                    break;

                case CampaignManager.Campaigns.FinalTest:
                    if(ftAssetCheck)
                    {
                        if(lessThanAssetCount)
                        {
                            result = (campMan.finalTest.assetCount < requiredAssetCount);
                        }
                        else
                        {
                            result = (campMan.finalTest.assetCount >= requiredAssetCount);
                        }
                    }
                    else
                    {
                        result = campMan.finalTest.GetFtNarrativeVariable(ftOutcomes);
                    }
                    break;
            }
            
        }
        else if (isRoomRequirement)
        {
            RoomStats[] existingRooms = GameObject.FindObjectsOfType<RoomStats>();
            List<int> roomIDs = new List<int>();

            foreach(RoomStats room in existingRooms)
            {
                roomIDs.Add( room.gameObject.GetComponent<ObjectScript>().objectNum );
            }

            int lookingFor = 0; //the ID of the room we need

            switch(necessaryRoom)
            {
                case RoomType.ArmorPlating:
                    lookingFor = 7;
                    break;
                case RoomType.Armory:
                    lookingFor = 8;
                    break;
                case RoomType.Brig:
                    lookingFor = 4;
                    break;
                case RoomType.Bunks:
                    lookingFor = 2;
                    break;
                case RoomType.CoreChargingTerminal:
                    lookingFor = 9;
                    break;
                case RoomType.EnergyCanon:
                    lookingFor = 10;
                    break;
                case RoomType.HydroponicsStation:
                    lookingFor = 1;
                    break;
                case RoomType.Medbay:
                    lookingFor = 5;
                    break;
                case RoomType.Pantry:
                    lookingFor = 12;
                    break;
                case RoomType.PhotonTorpedoes:
                    lookingFor = 11;
                    break;
                case RoomType.PowerCore:
                    lookingFor = 3;
                    break;
                case RoomType.ShieldGenerator:
                    lookingFor = 14;
                    break;
                case RoomType.StorageContainer:
                    lookingFor = 6;
                    break;
                case RoomType.TeleportationStation:
                    lookingFor = 15;
                    break;
                case RoomType.VIPLounge:
                    lookingFor = 13;
                    break;
                case RoomType.WarpDrive:
                    lookingFor = 16;
                    break;
            }

            result = roomIDs.Contains(lookingFor);
        }
        else if(isApprovalRequirement)
        {
            int approvalRating = thisShip.cStats.GetCharacterApproval(character);

            if (lessThanApproval && approvalRating < requiredApproval) //Match
            {
                result = true;
            }
            else if(!lessThanApproval && approvalRating > requiredApproval) //another match
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }

        return result;
    }
}
