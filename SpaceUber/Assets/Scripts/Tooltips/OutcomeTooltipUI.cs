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
    [SerializeField] private string narrativeOutcomeText;

    [SerializeField] private RectTransform outcomeList;
    private CampaignManager campMan;

    public void SetOutcomeData(string description, string secretOutcomeDescription, List<ChoiceOutcomes> outcomes)
    {
        //if there is no supplied description, use the secret description for it
        if (description == "")
        {
            outcomeDescUI.text = secretOutcomeDescription;
        }
        else
        {
            outcomeDescUI.text = description;
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = secretOutcomeDescription;
        }

        
        
    }
    public void SetOutcomeData(string description, List<ChoiceOutcomes> outcomes)
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

        if (outcomes.Count > 0)
        {
            foreach (var outcome in outcomes)
            {
                if(outcome.isNarrativeOutcome)
                {
                    GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
                    outcomeTextGO.GetComponent<TMP_Text>().text = narrativeOutcomeText;
                }
                else //if resource related, but not morale
                {
                    GameObject resourceGO = Instantiate(resourceUI, outcomeList.transform);
                    resourceGO.transform.GetChild(0).GetComponent<Image>().sprite =
                        GameManager.instance.GetResourceData((int)outcome.resource).resourceIcon; // get resource icon
                    

                    resourceGO.transform.GetChild(3).gameObject.SetActive(false); // outcome probability
                    if (outcome.resource != ResourceDataTypes._CrewMorale) 
                    {
                        resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text =
                        GameManager.instance.GetResourceData((int)outcome.resource).resourceName; // get resource name
                        if (outcome.isScaledOutcome)
                        {
                            int newAmount = Mathf.RoundToInt(outcome.amount * campMan.GetMultiplier(outcome.resource));
                            resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = newAmount.ToString(); // resource amount

                        }
                        else
                        {
                            resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = outcome.amount.ToString(); // resource amount
                        }
                    }
                    else //if morale, just add a plus or minus sign to it
                    {
                        resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = ""; //show no numbers here
                        if (outcome.amount >= 0)
                        {
                            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "+" + 
                                GameManager.instance.GetResourceData((int)outcome.resource).resourceName; // get resource name
                        }
                        
                        else
                        {
                            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = "-" +
                                GameManager.instance.GetResourceData((int)outcome.resource).resourceName; // get resource name
                        }
                    }
                    

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
    /// Set outcome data for random events that will be secret
    /// </summary>
    /// <param name="description">The description of the event</param>
    /// <param name="randomOutcomes">List of random outcomes possible</param>
    /// <param name="isSecret">Whether or not the result of this choice is secret</param>
    public void SetOutcomeData(string description, string secretEffectDescription, List<EventChoice.MultipleRandom> randomOutcomes)
    {
        if (description == "") //if no description supplied, use secret effect description for first spot
        {
            outcomeDescUI.gameObject.SetActive(false);
            outcomeDescUI.GetComponent<TMP_Text>().text = randomOutcomes.Count > 0 ? secretEffectDescription : defaultOutcomeText;
        }
        else
        {
            outcomeDescUI.text = description;

            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = randomOutcomes.Count > 0 ? secretEffectDescription : defaultOutcomeText;
        }
    }
}
