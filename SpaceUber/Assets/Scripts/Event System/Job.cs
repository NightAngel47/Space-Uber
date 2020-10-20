
/*
 * Job.cs
 * Author(s): Scott Acker
 * Created on: 10/16/2020 (en-US)
 * Description: Stores information about a particular job. Intended to be a prefab
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Job : MonoBehaviour
{
    public List<GameObject> storyEvents;
    public List<GameObject> randomEvents;

    public string jobName;
    public string description;
    public int payout;

    [HideInInspector] public JobSelectScreen jobSelect;
    [HideInInspector] public Button buttonPrefab;
  
    public void ShowButton(Transform buttonGroup)
    {
        Button thisButton = Instantiate(buttonPrefab, buttonGroup);
        thisButton.GetComponentInChildren<TMP_Text>().text = jobName + "\n" + payout + " credits";
        
        // Set listener
        thisButton.onClick.AddListener(delegate {
            jobSelect.SelectJob(this);
        });
    }
}