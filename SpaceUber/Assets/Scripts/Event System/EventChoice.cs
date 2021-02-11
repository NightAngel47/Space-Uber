/*
 * ChoiceRequirements.cs
 * Author(s): Scott Acker
 * Created on: 11/1/2020 (en-US)
 * Description: Stores all data required for an event choice, such as the requirements
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[Serializable]
public class EventChoice
{
    private InkDriverBase driver;    
    private Story story;
    [SerializeField] private string choiceName;
    [SerializeField] public string description;

    [SerializeField] private bool changeMusic;
    [SerializeField] private bool playSFX;
    [SerializeField, ShowIf("changeMusic"), AllowNesting] private bool withoutTransition;
    [Dropdown("eventMusicTracks"), SerializeField, ShowIf("changeMusic"), AllowNesting]
    public string eventBGM;
    public string eventSFX;
    private List<string> eventMusicTracks
    {
        get
        {
            return new List<string>() { "", "General Theme", "Wormhole", "Engine Malfunction", "Engine Delivery", "Black Market", "Clone Ambush Intro", "Safari Tampering", "Clone Ambush Negotiation", "Clone Ambush Fight", "Ejection", "Asteroid Mining", "Blockade", "Crop Blight", "Door Malfunction", "Drug Overdose", "Escaped Convicts", "Septic Malfunction", "Soothing Light", "Spatial Aurora", "Food Poisoning", "Hostage Situation", "Hull Maintenance" };
        }
    }

    [SerializeField] private bool hasRequirements;
    [SerializeField, ShowIf("hasRequirements")] private List<Requirements> choiceRequirements = new List<Requirements>();
    
    [SerializeField] private bool hasPercentChange;
    [SerializeField, ShowIf("hasPercentChange")] private List<IncreasedSuccess> percentIncrease = new List<IncreasedSuccess>();
    private float percantageIncreased;
    private bool increasedPercent = false;

    public bool hasSecretOutcomes;
    [SerializeField] private bool hasRandomEnding;    
    [SerializeField, ShowIf("hasRandomEnding")] private List<MultipleRandom> randomEndingOutcomes = new List<MultipleRandom>();
    [SerializeField, HideIf("hasRandomEnding")] private List<ChoiceOutcomes> outcomes = new List<ChoiceOutcomes>();
    private int randomizedResult;
    
    [SerializeField] private bool hasSubsequentChoices;
    [SerializeField, ShowIf("hasSubsequentChoices"), AllowNesting] private int subsequentChoiceIndex;

    [Serializable]
    public class MultipleRandom
    {
        public string randomChanceName;
        public List<ChoiceOutcomes> outcomes = new List<ChoiceOutcomes>();
        public float probability;
    }

    /// <summary>
    /// Extra code to determine if a choice is actually available
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="myButton"></param>
    public void CreateChoice(ShipStats ship, Button myButton, Story thisStory, InkDriverBase thisDriver, OutcomeTooltipUI tooltip)
    {
        bool requirementMatch = true;
        driver = thisDriver;

        if (driver.isCharacterEvent)
        {
            foreach (ChoiceOutcomes outcome in this.outcomes)
            {
                outcome.AssignCharacterDriver((CharacterEvent)driver);
            }
        }

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

        if (hasPercentChange)
        {
            for (int i = 0; i < percentIncrease.Count; i++)
            {
                if (percentIncrease[i].MatchesSuccessChance(ship))
                {
                    percantageIncreased = percentIncrease[i].GetTotalPercentIncrease();
                    increasedPercent = true;
                }
            }
        }

        if (requirementMatch)
        {
            myButton.interactable = true;
            story = thisStory;
        }
        else
        {
            myButton.interactable = false;
        }
        // Tooltip stuff
        if (hasRandomEnding)
        {
            tooltip.SetOutcomeData(description, randomEndingOutcomes, hasSecretOutcomes);
        }
        else
        {
            tooltip.SetOutcomeData(description, outcomes, hasSecretOutcomes);
        }
        //randomize which ending we'll have from the start, needs to have story that matched requirements.
        if (story != null && hasRandomEnding)
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
        if (hasSubsequentChoices)
        {
            driver.TakeSubsequentChoices(driver.subsequentChoices[subsequentChoiceIndex].eventChoices);
        }

        if (changeMusic)
        {
            if(withoutTransition)
            {
                AudioManager.instance.PlayMusicWithoutTransition(eventBGM);
            }
            else
            {
                AudioManager.instance.PlayMusicWithTransition(eventBGM);
            }
        }

        if (playSFX)
        {
            AudioManager.instance.PlaySFX(eventSFX);
        }

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
                outcome.narrativeResultsBox = driver.resultsBox;
                outcome.hasSubsequentChoices = hasSubsequentChoices;
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
            
            if(increasedPercent)
            {
                choiceThreshold += percantageIncreased;
            }

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
