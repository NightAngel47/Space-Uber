/*
 * ChoiceRequirements.cs
 * Author(s): Scott Acker
 * Created on: 11/1/2020 (en-US)
 * Description: Stores all data required for an event choice, such as the requirements
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using NaughtyAttributes;

public class EventChoice : MonoBehaviour
{
    [Dropdown("thisCampaign")]
    public string campaign;
    private List<string> thisCampaign
    {
        get
        {
            return new List<string>() { "NA", "Catering to the Rich" };
        }
    }

    [SerializeField] private string choiceName;
    [SerializeField] private bool hasRequirements;
    [SerializeField, ShowIf("hasRequirements")] private List<Requirements> choiceRequirements;
    [SerializeField] private ChoiceOutcomes outcome;
    private bool hasRandomOutcome;
    protected Story story;

    // Start is called before the first frame update

    /// <summary>
    /// Extra code to determine if a choice is actually available
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="myButton"></param>
    public void ControlChoice(ShipStats ship, Button myButton, Story thisStory)
    {
        bool requirementMatch = true;

        if(hasRequirements)
        {
            //if anything in choiceRequirements does not match, this bool is automatically false
            for (int i = 0; i < choiceRequirements.Count; i++)
            {
                if (!choiceRequirements[i].MatchesRequirements(ship))
                {
                    requirementMatch = false;
                }

            }
        }
        

        if(requirementMatch)
        {
            myButton.interactable = true;
            story = thisStory;
            hasRandomOutcome = outcome.isRandomOutcome;
        }
        else
        {
            myButton.interactable = false;
        }
    }

    public void SelectChoice(ShipStats ship)
    {
        //if(hasRandomOutcome)
        //{
        //    RandomizeEnding();
        //}

        outcome.StatChange(ship);
       
    }

    private void RandomizeEnding()
    {
        var numberOfEndingsRaw = story.variablesState["numberOfEndings"];
        int endingNum = (int)numberOfEndingsRaw;

        //This is hardcoded for the wormhole event until I can get it to work universally
        int rng = Random.Range(1, endingNum);
        switch (rng)
        {
            case 1:
                story.variablesState["randomEnd"] = story.variablesState["endingOne"];
                break;
            case 2:
                story.variablesState["randomEnd"] = story.variablesState["endingTwo"];
                break;
            case 3:
                story.variablesState["randomEnd"] = story.variablesState["endingThree"];
                break;
            case 4:
                story.variablesState["randomEnd"] = story.variablesState["endingFour"];
                break;
        }
    }
}
