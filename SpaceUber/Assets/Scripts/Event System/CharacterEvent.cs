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
    [SerializeField,Tooltip("The character that this event focuses on")] private CharacterStats.Characters thisCharacter;
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
}
