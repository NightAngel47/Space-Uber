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
    

    [SerializeField] private string choiceName;
    [SerializeField] private bool hasRequirements;

    [SerializeField, ShowIf("hasRequirements")] private List<Requirements> choiceRequirements;

    [SerializeField] private bool hasRandomEnding;
    [SerializeField, HideIf("hasRandomEnding")] private List<ChoiceOutcomes> outcomes;
    [SerializeField, ShowIf("hasRandomEnding")] private List<ChoiceOutcomes> randomEndingOutcomes;
    [SerializeField, ShowIf("hasRandomEnding"), AllowNesting] private List<float> probabilities;
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
        }
        else
        {
            myButton.interactable = false;
        }
    }

    /// <summary>
    /// Tells the outcome object to start applying changes.
    /// </summary>
    /// <param name="ship"></param>
    public void SelectChoice(ShipStats ship)
    {
        if (hasRandomEnding)
        {
            int rng = RandomizeEnding();
            randomEndingOutcomes[rng].StatChange(ship);
        }
        else
        {
            foreach (ChoiceOutcomes outcome in outcomes)
            {
                outcome.StatChange(ship);
            }
        }

        

    }

    /// <summary>
    /// Randomly choose which ending that the choice will end with. Requires an Ink variable called numberOfRandomEndings
    /// as well as knot variables called "endingOne", "endingTwo" and so on.
    /// </summary>
    private int RandomizeEnding()
    {
        //var numberOfEndingsRaw = story.variablesState["numberOfRandomEndings"];
        //int endingNum = (int)numberOfEndingsRaw;

        
        int rng = Random.Range(1, randomEndingOutcomes.Count);
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
        return rng;
    }
}
