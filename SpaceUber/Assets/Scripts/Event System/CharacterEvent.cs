/*
 * CharacterEvent.cs
 * Author(s): Scott Acker
 * Created on: 2/4/2021 (en-US)
 * Description: Inherits from InkDriverBase to add extra functionality such as the type of room this is attached to
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvent : InkDriverBase
{
    [SerializeField,Tooltip("The character that this event focuses on")]
    private CharacterStats.Characters thisCharacter = CharacterStats.Characters.None;

    [Tooltip("How many opportunities are given for players to respond"),SerializeField]
    private int totalAnswers;
    
    [Tooltip("How many correct answers have been given")]
    private int correctAnswers;


    private enum AnswerState
    {
        GOOD,
        NEUTRAL,
        BAD
    }

    private AnswerState answersState = AnswerState.NEUTRAL;

    public override void Start()
    {
        base.Start();
        isCharacterEvent = true;
        isStoryEvent = false;
    }

    public void AnswerCorrectly()
    {
        ++correctAnswers;
    }

    /// <summary>
    /// Applies proper boost to the required room if the player gives the correct responses in a character event
    /// Also marks this event as having been played once already
    /// </summary>
    public void EndCharacterEvent()
    {
        print("Ending this character event");


        if(correctAnswers == Mathf.RoundToInt(totalAnswers/2))
        {
            answersState = AnswerState.NEUTRAL;
        }else if (correctAnswers >= Mathf.RoundToInt(totalAnswers / 2)) //more than half correct answers
        {
            answersState = AnswerState.GOOD;
        }
        else if (correctAnswers <= Mathf.RoundToInt(totalAnswers / 2)) //more than half wrong answers
        {
            answersState = AnswerState.BAD;
        }

        switch(answersState)
        {
            case AnswerState.GOOD:
                MoraleManager.instance.CrewMorale += 10;
                switch (thisCharacter)
                {
                    case CharacterStats.Characters.KUON: //Kuon boosts security and weapons by 10%
                        thisShip.Security += 10;
                        thisShip.ShipWeapons += 10;

                        SpawnStatChangeText(10, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                        SpawnStatChangeText(10, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                        break;
                    case CharacterStats.Characters.MATEO: //Boosts energy
                        thisShip.EnergyRemaining += new Vector2(20, 0);
                        SpawnStatChangeText(20, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                        break;
                    case CharacterStats.Characters.LANRI: //boosts Food
                        thisShip.Food += 20;
                        SpawnStatChangeText(20, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                        break;
                    case CharacterStats.Characters.LEXA: //gives +10 to morale
                        MoraleManager.instance.CrewMorale += 10;
                        break;
                    case CharacterStats.Characters.RIPLEY: //gives +10 morale
                        MoraleManager.instance.CrewMorale += 10;
                        break;
                }
                break;
            case AnswerState.NEUTRAL:
                MoraleManager.instance.CrewMorale += 10;
                break;

            case AnswerState.BAD:
                MoraleManager.instance.CrewMorale -= 10;
                switch (thisCharacter)
                {
                    case CharacterStats.Characters.KUON: //Kuon boosts security and weapons by 10%
                        thisShip.Security -= 10;
                        thisShip.ShipWeapons -= 10;

                        SpawnStatChangeText(-10, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                        SpawnStatChangeText(-10, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                        break;
                    case CharacterStats.Characters.MATEO: //Boosts energy
                        thisShip.EnergyRemaining += new Vector2(-20, 0);
                        SpawnStatChangeText(20, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                        break;
                    case CharacterStats.Characters.LANRI: //boosts Food
                        thisShip.Food -= 20;
                        SpawnStatChangeText(-20, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                        break;
                    case CharacterStats.Characters.LEXA: //gives -10 to morale
                        MoraleManager.instance.CrewMorale -= 10;
                        break;
                    case CharacterStats.Characters.RIPLEY: //gives -10 morale
                        MoraleManager.instance.CrewMorale -= 10;
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
}
