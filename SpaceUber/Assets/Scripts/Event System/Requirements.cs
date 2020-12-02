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
                    shipStat = thisShip.ShipHealthCurrent;
                    break;
                case ResourceType.ENERGY:
                    shipStat = thisShip.EnergyRemaining;
                    break;
                case ResourceType.CREW:
                    shipStat = thisShip.CrewCurrent;
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
            switch(campMan.currentCamp)
            {
                //for catering to the rich campaign
                case Campaigns.CateringToTheRich:
                    CampaignManager.CateringToTheRich campaign = CampaignManager.Campaign.ToCateringToTheRich(campMan.campaigns[(int)Campaigns.CateringToTheRich]);
                    
                    //check if the selected bool is true or not
                    switch(ctrBoolRequirements)
                    {
                        case "Side With Scientist":
                            result = campaign.ctr_sideWithScientist;
                            break;
                        case "Kill Beckett":
                            result = campaign.ctr_killBeckett;
                            break;
                        case "Killed At Safari":
                            result = campaign.ctr_killedAtSafari;
                            break;
                        case "Tell VIPs About Clones":
                            result = campaign.ctr_tellVIPsAboutClones;
                            break;
                        case "N_A":
                            break;
                    }
                    if(ctrTrustRequirements)
                    {
                        bool VIPResult = campaign.ctr_VIPTrust > VIPTrustRequirement;
                        bool cloneResult = campaign.ctr_cloneTrust > cloneTrustRequirement;
                        result = VIPResult && cloneResult;
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
        

        return result;
    }

    private List<string> cateringToRichBools
    {
        get
        {
            return new List<string>() { "N_A", "Side With Scientist", "Kill Beckett", "Let Bale Pilot", "Killed At Safari", "Tell VIPs About Clones" };
        }
    }
}
