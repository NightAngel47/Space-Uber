/*
 * OutcomeTooltipUI.cs
 * Author(s): Sam Ferstein
 * Created on: 12/4/2020 (en-US)
 * Description: Controls the message window that appears when a player hovers over a choice
 * Shows potential stat outcomes if there
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeTooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text outcomeDescUI;
    [SerializeField] private GameObject resourceUI;
    [SerializeField] private GameObject outcomeText;
    [SerializeField] private string defaultOutcomeText;
    [SerializeField] private string unknownOutcomeText;
    [SerializeField] private string randomOutcomeText;
    [SerializeField] private string narrativeOutcomeText;

    [SerializeField] private RectTransform outcomeList;
    private CampaignManager campMan;

    public void SetOutcomeData(string description, List<ChoiceOutcomes> outcomes, bool isSecret)
    {
        if (!campMan)
        { campMan = FindObjectOfType<CampaignManager>(); }

        //if there is no supplied description, deactivate the description field
        if (description == "")
        {
            outcomeDescUI.gameObject.SetActive(false);
        }
        else
        {
            outcomeDescUI.text = description;
        }

        //if the outcome is meant to be secretive
        if(isSecret)
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = unknownOutcomeText;
        }
        else if (outcomes.Count > 0)
        {
            foreach (var outcome in outcomes)
            {
                if(outcome.isNarrativeOutcome)
                {
                    GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
                    outcomeTextGO.GetComponent<TMP_Text>().text = narrativeOutcomeText;
                }
                else
                {
                    GameObject resourceGO = Instantiate(resourceUI, outcomeList.transform);
                    resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = 
                        GameManager.instance.GetResourceData((int)outcome.resource).resourceIcon; // resource icon
                    resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = 
                        GameManager.instance.GetResourceData((int)outcome.resource).resourceName; // resource name
                    
                    if(outcome.isScaledOutcome)
                    {
                        int newAmount = Mathf.RoundToInt(outcome.amount * campMan.GetMultiplier(outcome.resource));
                        resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = newAmount.ToString(); // resource amount

                    }
                    else
                    {
                        resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = outcome.amount.ToString(); // resource amount
                    }

                    resourceGO.transform.GetChild(3).gameObject.SetActive(false); // outcome probability
                }
            }
        }
        else //Outcome effects nothing
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
    }

    /// <summary>
    /// Set outcome data for random events 
    /// </summary>
    /// <param name="description">The description of the event</param>
    /// <param name="randomOutcomes">List of random outcomes possible</param>
    /// <param name="isSecret">Whether or not the result of this choice is secret</param>
    public void SetOutcomeData(string description, List<EventChoice.MultipleRandom> randomOutcomes, bool isSecret)
    {
        if(description == "")
        {
            outcomeDescUI.gameObject.SetActive(false);
        }
        else
        {
            outcomeDescUI.text = description;
        }

        if (isSecret) //outcome is secretive
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = unknownOutcomeText;
        }
        else if (randomOutcomes.Count > 0) //uses a random outcome
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = randomOutcomeText;
        }
        else //this does not does effect any sort of stats
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
    }
}
