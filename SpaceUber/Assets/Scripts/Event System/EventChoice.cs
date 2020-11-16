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

[System.Serializable]
public struct EventChoice
{
    private InkDriverBase driver;
    [SerializeField] public string choiceName;
    [SerializeField] private bool hasRequirements;

    [SerializeField, ShowIf("hasRequirements"), AllowNesting] private List<Requirements> choiceRequirements;

    [SerializeField] public bool hasRandomEnding;
    [SerializeField, HideIf("hasRandomEnding"), AllowNesting] private List<ChoiceOutcomes> outcomes;
    [SerializeField, ShowIf("hasRandomEnding"), AllowNesting] private List<MultipleRandom> randomEndingOutcomes;
    private Story story;
    private int randomizedResult;

    [SerializeField] bool hasSubsequentChoices;
    [SerializeField, ShowIf("hasSubsequentChoices")] private List<EventChoice> subsequentChoices;

    [System.Serializable]
    public class MultipleRandom
    {
        public List<ChoiceOutcomes> outcomes;
        public float probability;
    }

    /// <summary>
    /// Extra code to determine if a choice is actually available
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="myButton"></param>
    public void CreateChoice(ShipStats ship, Button myButton, Story thisStory, InkDriverBase thisDriver)
    {
        bool requirementMatch = true;
        driver = thisDriver;

        if (hasRequirements)
        {
            //if anything in choiceRequirements does not match, this bool is automatically false
            for (int i = 0; i < choiceRequirements.Count; i++)
            {
                if (!choiceRequirements[i].MatchesRequirements(ship, driver.campMan))
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

        //randomize which ending we'll have from the start
        if (hasRandomEnding)
        {
            RandomizeEnding(story);
        }

    }

    /// <summary>
    /// Tells the outcome object to start applying changes.
    /// </summary>
    /// <param name="ship"></param>
    public void SelectChoice(ShipStats ship)
    {

        driver.TakeSubsequentChoices(subsequentChoices);

        if (hasRandomEnding)
        {
            foreach(MultipleRandom multRando in randomEndingOutcomes)
            {
                MultipleRandom thisSet = randomEndingOutcomes[randomizedResult];
                foreach(ChoiceOutcomes choiceOutcome in thisSet.outcomes)
                {
                    choiceOutcome.StatChange(ship, driver.campMan, hasSubsequentChoices);
                }

            }
        }
        else
        {
            foreach (ChoiceOutcomes outcome in outcomes)
            {
                outcome.StatChange(ship, driver.campMan, hasSubsequentChoices);
            }
        }
        
    }

    /// <summary>
    /// Randomly choose which ending that the choice will end with. Requires an Ink variable called numberOfRandomEndings
    /// as well as knot variables called "endingOne", "endingTwo" and so on.
    /// Called by InkDriverBase the moment it sees a "randomizeEnding" tag, technically before a choice is chosen.
    /// </summary>
    public void RandomizeEnding(Story thisStory)
    {
        story = thisStory;

        int result = 0;
        float choiceThreshold = 0;
        float outcomeChance = Random.Range(0f, 100f);

        for (int i = 0; i < randomEndingOutcomes.Count; i++)
        {
            choiceThreshold += randomEndingOutcomes[i].probability; //adds the probability of the next element to choice threshold

            //if the outcome chance is lower than the threshold, we pick this event
            if (outcomeChance <= choiceThreshold || (i == randomEndingOutcomes.Count)) 
            {
                result = i;
                break;
            }

        }

        story.EvaluateFunction("RandomizeEnding", result);

        randomizedResult = result;
    }
}
