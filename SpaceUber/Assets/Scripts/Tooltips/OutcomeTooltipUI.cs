/*
 * OutcomeTooltipUI.cs
 * Author(s): Sam Ferstein
 * Created on: 12/4/2020 (en-US)
 * Description:
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
    [SerializeField] private RectTransform outcomeList;
    [SerializeField] private List<Sprite> icons = new List<Sprite>();

    public void SetOutcomeData(string description, List<ChoiceOutcomes> outcomes, bool isSecret)
    {
        if(description == "")
        {
            outcomeDescUI.gameObject.SetActive(false);
        }
        else
        {
            outcomeDescUI.text = description;
        }

        if(isSecret)
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
        else if (outcomes.Count > 0)
        {
            foreach (var outcome in outcomes)
            {
                if(outcome.isNarrativeOutcome)
                {
                    GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
                    outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
                }
                else
                {
                    GameObject resourceGO = Instantiate(resourceUI, outcomeList.transform);
                    resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)outcome.resource).resourceIcon; // resource icon
                    resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = outcome.resource.ToString(); // resource name
                    resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = outcome.amount.ToString(); // resource amount
                    resourceGO.transform.GetChild(3).gameObject.SetActive(false); // outcome probability
                }
            }
        }
        else
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
    }

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

        if (isSecret)
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
        else if (randomOutcomes.Count > 0)
        {
            foreach (var randomOutcome in randomOutcomes)
            {
                for (int i = 0; i < randomOutcome.outcomes.Count; i++)
                {
                    if (randomOutcome.outcomes[i].isNarrativeOutcome)
                    {
                        GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
                        outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
                    }
                    else
                    {
                        GameObject resourceGO = Instantiate(resourceUI, outcomeList.transform);
                        resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.GetResourceData((int)randomOutcome.outcomes[i].resource).resourceIcon; // resource icon
                        resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = randomOutcome.outcomes[i].resource.ToString(); // resource name
                        resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = randomOutcome.outcomes[i].amount.ToString(); // resource amount

                        if (i == 0)
                        {
                            resourceGO.transform.GetChild(3).GetComponent<TMP_Text>().text = randomOutcome.probability + "%"; // outcome probability
                        }
                        else
                        {
                            resourceGO.transform.GetChild(3).gameObject.SetActive(false); // outcome probability
                        }
                    }
                }
            }
        }
        else
        {
            GameObject outcomeTextGO = Instantiate(outcomeText, outcomeList.transform);
            outcomeTextGO.GetComponent<TMP_Text>().text = defaultOutcomeText;
        }
    }
}
