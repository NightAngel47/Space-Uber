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
    private CharacterStats.Characters thisCharacter;
    [Tooltip("How many correct answers have been given")]
    private int correctAnswers;
    [SerializeField, Tooltip("How many correct responses the player needs to get a boost")] 
    private int requiredCorrectAnswers;

    

    public override void Start()
    {
        base.Start();
        isCharacterEvent = true;
        isStoryEvent = false;
    }

    public bool SucceededEvent()
    {
        return correctAnswers >= requiredCorrectAnswers;
    }

    public void AnswerCorrectly()
    {
        ++correctAnswers;
    }

    /// <summary>
    /// Applies proper boost to the required room if the player gives the correct responses in a character event
    /// </summary>
    public void EndCharacterEvent()
    {
        switch (thisCharacter)
        {
            case CharacterStats.Characters.KUON: //Kuon boosts security and weapons by 10%
                int newSecurityValue = thisShip.Security + Mathf.RoundToInt(thisShip.Security * .1f);
                int newWeaponsValue = thisShip.ShipWeapons + Mathf.RoundToInt(thisShip.ShipWeapons * .1f);

                thisShip.Security = newSecurityValue;
                thisShip.ShipWeapons = newWeaponsValue;

                SpawnStatChangeText(newSecurityValue, 1);
                SpawnStatChangeText(newWeaponsValue, 2);
                break;
            case CharacterStats.Characters.MATEO: //Boosts energy

                break;
            case CharacterStats.Characters.LANRI: //boosts Food

                break;
            case CharacterStats.Characters.LEXA: //gives +10 to morale

                break;
            case CharacterStats.Characters.RIPLEY: //gives +10 morale

                break;
        }
    }

        /// <summary>
        /// Shows the text that will appear over stats whenever the player receives an update to it
        /// Credits - Security - Weapons - Food - Crew - Power - Hull
        /// </summary>
        /// <param name="value">The actual number that shows how much the value changed</param>
        /// <param name="iconNumber">Which icon will be used. </param>
        private void SpawnStatChangeText(int value, int icon = -1)
    {
        GameObject statChangeText = thisShip.GetComponent<ShipStatsUI>().statChangeText;
        GameObject instance = GameObject.Instantiate(statChangeText);

        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        instance.transform.parent = thisShip.GetComponent<ShipStatsUI>().canvas;

        MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, +75);
        moveAndFadeBehaviour.SetValue(value, icon);
    }
}
