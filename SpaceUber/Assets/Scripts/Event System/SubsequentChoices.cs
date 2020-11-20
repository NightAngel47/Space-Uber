/*
 * SubsequentChoices.cs
 * Author(s): Steven Drovie []
 * Created on: 11/16/2020 (en-US)
 * Description: 
 */

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubsequentChoices
{
    [SerializeField] private string subsequentChoicesName;
    [Tooltip("List of event choices to show when this subsequent choice is chosen")]
    public List<EventChoice> eventChoices = new List<EventChoice>();
}
