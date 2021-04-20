/*
 * CharacterEvent.cs
 * Author(s): Scott Acker
 * Created on: 2/4/2021 (en-US)
 * Description: Inherits from InkDriverBase to add extra functionality such as the type of room this is attached to
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterEvent : InkDriverBase
{
    [SerializeField,Tooltip("The character that this event focuses on")]
    private CharacterStats.Characters thisCharacter = CharacterStats.Characters.None;

    [Tooltip("The total approval from this character")]
    private int characterApproval = 0;

    [Tooltip("Stat boost towards security if the event has a positive outcome"),
        SerializeField, ShowIf("IsKuon")]
    private int securityBoost = 10;
    [Tooltip("Stat loss towards security if the event has a negative outcome"),
        SerializeField, ShowIf("IsKuon")]
    private int securityLoss = -10;

    [Tooltip("Stat boost towards weapons if the event has a positive outcome"),
    SerializeField, ShowIf("IsKuon")]
    private int weaponsBoost = 10;
    [Tooltip("Stat loss towards weapons if the event has a negative outcome"),
    SerializeField, ShowIf("IsKuon")]
    private int weaponsLoss = -10;

    [Tooltip("Stat boost towards energy if the event has a positive outcome"),
        SerializeField, ShowIf("IsMateo")]
    private int energyBoost = 20;
    [Tooltip("Stat loss towards energy if the event has a negative outcome"),
        SerializeField, ShowIf("IsMateo")]
    private int energyLoss = -20;

    [Tooltip("Stat boost towards food if the event has a positive outcome"),
        SerializeField, ShowIf("IsLanri")]
    private int foodBoost = 10;
    [Tooltip("Stat loss towards food if the event has a negative outcome"),
        SerializeField, ShowIf("IsLanri")]
    private int foodLoss = -10;

    [Tooltip("Stat boost towards morale if the event has a positive outcome"),
        SerializeField]
    private int positiveMoraleBoost = 10;
    [Tooltip("Stat boost towards morale if the event has a neutral outcome"),
        SerializeField]
    private int neutralMoraleBoost = 5;
    [Tooltip("Stat loss towards morale if the event has a negative outcome"),
        SerializeField]
    private int moraleLoss = -10;

    [Tooltip("The types of room this is attached to. Used to determine if this event can be played")]
    private List<RoomStats.RoomType> roomTypes;
    public enum AnswerState
    {
        POSITIVE,
        NEUTRAL,
        NEGATIVE
    }

    private AnswerState answersState = AnswerState.NEUTRAL;

    /// <summary>
    /// Whether or not the supplied room type is applicable to this event
    /// </summary>
    /// <param name="thisRoom"></param>
    /// <returns></returns>
    public bool MatchesRoomType(RoomStats.RoomType thisRoom)
    {
        foreach (RoomStats.RoomType type in roomTypes)
        {
            if(type == thisRoom)
            {
                return true;
            }
        }
        return false;
    }

    public override void Start()
    {
        base.Start();
        isCharacterEvent = true;
        isStoryEvent = false;
        characterApproval = 0;
    }
    public void Awake()
    {
        SetCharacterRooms();

    }

    private void SetCharacterRooms()
    {
        if(thisCharacter == CharacterStats.Characters.Kuon)
        {
            roomTypes = new List<RoomStats.RoomType>
            { 
                RoomStats.RoomType.EnergyCanon, 
                RoomStats.RoomType.PhotonTorpedoes, 
                RoomStats.RoomType.Armory, 
                RoomStats.RoomType.Brig
            };
        }
        if (thisCharacter == CharacterStats.Characters.Lanri)
        {
            roomTypes = new List<RoomStats.RoomType>
            {
                RoomStats.RoomType.Pantry,
                RoomStats.RoomType.HydroponicsStation
            };
        }
        if (thisCharacter == CharacterStats.Characters.Mateo)
        {
            roomTypes = new List<RoomStats.RoomType>
            {
                RoomStats.RoomType.PowerCore,
                RoomStats.RoomType.CoreChargingTerminal
            };
        }
        if (thisCharacter == CharacterStats.Characters.Lexa)
        {
            roomTypes = new List<RoomStats.RoomType>
            {
                RoomStats.RoomType.Bunks
            };
        }
        if (thisCharacter == CharacterStats.Characters.Ripley)
        {
            roomTypes = new List<RoomStats.RoomType>
            {
                RoomStats.RoomType.Medbay
            };
        }
        print("This list for " + thisCharacter.ToString() +" now includes: ");
        foreach(RoomStats.RoomType room in roomTypes)
        {
            print(room.ToString());
        }
    }

    public List<RoomStats.RoomType> GetRoomRequirements()
    {

    }

    public void ChangeEventApproval(int change)
    {
        characterApproval += change;
    }

    /// <summary>
    /// Applies proper boost to the required room if the player gives the correct responses in a character event
    /// Also marks this event as having been played once already
    /// </summary>
    public void EndCharacterEvent()
    {
        print("Ending this character event");

        if(characterApproval >= 0)
        {
            answersState = AnswerState.POSITIVE;
        }
        else if (characterApproval < 0)
        {
            answersState = AnswerState.NEGATIVE;
        }
        else
        {
            answersState = AnswerState.NEUTRAL;
        }

        switch(answersState)
        {
            case AnswerState.POSITIVE:
                MoraleManager.instance.CrewMorale += positiveMoraleBoost;
                switch (thisCharacter)
                {
                    case CharacterStats.Characters.Kuon: //Kuon boosts security and weapons 
                        thisShip.Security += Mathf.RoundToInt(securityBoost * campMan.GetMultiplier(ResourceDataTypes._Security));
                        thisShip.ShipWeapons += Mathf.RoundToInt(weaponsBoost * campMan.GetMultiplier(ResourceDataTypes._ShipWeapons));

                        SpawnStatChangeText(securityBoost, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                        SpawnStatChangeText(weaponsBoost, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                        break;
                    case CharacterStats.Characters.Mateo: //Boosts energy
                        int newEnergy = Mathf.RoundToInt(energyBoost * campMan.GetMultiplier(ResourceDataTypes._Energy));
                        thisShip.Energy += new Vector3(newEnergy, 0, 0);
                        SpawnStatChangeText(newEnergy, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                        break;
                    case CharacterStats.Characters.Lanri: //boosts Food

                        thisShip.Food += Mathf.RoundToInt(foodBoost * campMan.GetMultiplier(ResourceDataTypes._Food));
                        SpawnStatChangeText(foodBoost, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                        break;
                }
                break;
            case AnswerState.NEUTRAL:
                MoraleManager.instance.CrewMorale += neutralMoraleBoost;
                break;

            case AnswerState.NEGATIVE:
                MoraleManager.instance.CrewMorale += moraleLoss;
                switch (thisCharacter)
                {
                    case CharacterStats.Characters.Kuon: //loses security and weapons by 10
                        thisShip.Security += Mathf.RoundToInt(securityLoss * campMan.GetMultiplier(ResourceDataTypes._Security));
                        thisShip.ShipWeapons += Mathf.RoundToInt(weaponsLoss * campMan.GetMultiplier(ResourceDataTypes._ShipWeapons));

                        SpawnStatChangeText(securityLoss, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                        SpawnStatChangeText(weaponsLoss, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                        break;
                    case CharacterStats.Characters.Mateo: //Loses energy
                        int newEnergy = Mathf.RoundToInt(energyLoss * campMan.GetMultiplier(ResourceDataTypes._Energy));
                        thisShip.Energy += new Vector3(newEnergy, 0, 0);
                        SpawnStatChangeText(newEnergy, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                        break;
                    case CharacterStats.Characters.Lanri: //loses Food
                        thisShip.Food += Mathf.RoundToInt(foodLoss * campMan.GetMultiplier(ResourceDataTypes._Food));
                        SpawnStatChangeText(foodLoss, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                        break;
                }
                break;

        }
        
    }

    /// <summary>
    /// Shows the text that will appear over stats whenever the player receives an update to it
    /// Credits - Security - Weapons - Food - Crew - Power - Hull
    /// </summary>
    /// <param name="value">The actual number that shows how much the value changed</param>
    /// <param name="icon">Which icon will be used. </param>
    private void SpawnStatChangeText(int value, Sprite icon = null)
    {
        GameObject statChangeText = thisShip.GetComponent<ShipStatsUI>().statChangeText;
        GameObject instance = Instantiate(statChangeText);

        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        instance.transform.parent = thisShip.GetComponent<ShipStatsUI>().canvas;

        MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, +75);
        moveAndFadeBehaviour.SetValue(value, icon);
    }


    private bool IsMateo()
    {
        return thisCharacter == CharacterStats.Characters.Mateo;
    }
    private bool IsLexa()
    {
        return thisCharacter == CharacterStats.Characters.Lexa;
    }
    private bool IsRipley()
    {
        return thisCharacter == CharacterStats.Characters.Ripley;
    }
    private bool IsKuon()
    {
        return thisCharacter == CharacterStats.Characters.Kuon;
    }
    private bool IsLanri()
    {
        return thisCharacter == CharacterStats.Characters.Lanri;
    }
}
