/*
 * ChoiceRequirements.cs
 * Author(s): Scott Acker
 * Created on: 11/1/2020 (en-US)
 * Description: Stores all data required for an event choice, such as the requirements
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChoice : MonoBehaviour
{
    [SerializeField]private Requirements choiceRequirements;
    [SerializeField] private ChoiceOutcomes outcomes;
    [HideInInspector] public bool selectable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnChoice(ShipStats ship)
    {
        if(choiceRequirements.MatchesRequirements(ship))
        {
            selectable = true;
        }
        else
        {
            selectable = false;
        }
    }

    public void SelectChoice(ShipStats ship)
    {
        outcomes.ChoiceChange(ship);
    }
}
