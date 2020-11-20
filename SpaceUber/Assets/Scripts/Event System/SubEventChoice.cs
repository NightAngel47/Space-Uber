/*
 * SubEventChoice.cs
 * Author(s): 
 * Created on: 11/15/2020 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using NaughtyAttributes;

[System.Serializable]
public class SubEventChoice
{
    public string choiceName;
    public bool hasRequirements;
    [SerializeField, ShowIf("hasRequirements"), AllowNesting] private bool choiceRequirements;

    public EventChoice ConvertToEventChoice()
    {
        EventChoice newChoice = new EventChoice();
        return newChoice;
    }
}
