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

    [Tooltip("If the requirement is stat-based")]
    [SerializeField, AllowNesting]
    private bool isStatRequirement = false;

    [Tooltip("The resource you would like to be compared")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    private ResourceType selectedResource;
    
    [Tooltip("How much of this resources is required for an event to run")]
    [SerializeField, ShowIf("isStatRequirement"), AllowNesting]
    private int requiredAmount;

    [Tooltip("Click this if you would like to check if the ship resource is LESS than the number supplied")]
    [SerializeField, ShowIf("isStatRequirement"),AllowNesting]
    private bool lessThan = false;
    #endregion

    #region Narrative Requirement Variables
    [Tooltip("If the requirement is narrative-based")]
    [SerializeField, AllowNesting]
    private bool isNarrativeRequirement = false;

    [FormerlySerializedAs("ctrBoolRequirements")]
    [Tooltip("Select one item from this dropdown list. The selected variable must be true for this event to run"),
     SerializeField, ShowIf("isNarrativeRequirement"), AllowNesting]
    private CampaignManager.CateringToTheRich.NarrativeOutcomes ctrNarrativeOutcomes;

    [Tooltip("Click this if you would like to check trust variables for Catering to the Rich")]
    [SerializeField, ShowIf("isNarrativeRequirement"), AllowNesting]
    private bool ctrTrustRequirements = false;

    [Tooltip("The minimum trust the clones must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting]
    private int cloneTrustRequirement;

    [Tooltip("The minimum trust the VIPS must have in the player")]
    [SerializeField, ShowIf("ctrTrustRequirements"), AllowNesting] 
    private int VIPTrustRequirement;
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
                case ResourceType.HULL:
                    shipStat = (int)thisShip.ShipHealthCurrent.x;
                    break;
                case ResourceType.ENERGY:
                    shipStat = (int)thisShip.EnergyRemaining.x;
                    break;
                case ResourceType.CREW:
                    shipStat = (int)thisShip.CrewCurrent.x;
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
            //check if the selected bool is true or not
            switch(ctrNarrativeOutcomes)
            {
                case CampaignManager.CateringToTheRich.NarrativeOutcomes.SideWithScientist:
                    result = campMan.cateringToTheRich.ctr_sideWithScientist;
                    break;
                case CampaignManager.CateringToTheRich.NarrativeOutcomes.KillBeckett:
                    result = campMan.cateringToTheRich.ctr_killBeckett;
                    break;
                case CampaignManager.CateringToTheRich.NarrativeOutcomes.LetBalePilot:
                    result = campMan.cateringToTheRich.ctr_letBalePilot;
                    break;
                case CampaignManager.CateringToTheRich.NarrativeOutcomes.KilledAtSafari:
                    result = campMan.cateringToTheRich.ctr_killedAtSafari;
                    break;
                case CampaignManager.CateringToTheRich.NarrativeOutcomes.TellVIPsAboutClones:
                    result = campMan.cateringToTheRich.ctr_tellVIPsAboutClones;
                    break;
                default:
                    break;
            }
            if(ctrTrustRequirements)
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
        

        return result;
    }
}
