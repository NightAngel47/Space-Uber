
/*
 * Job.cs
 * Author(s): Scott Acker
 * Created on: 10/16/2020 (en-US)
 * Description: Stores information about a particular job. Intended to be a prefab
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Job : MonoBehaviour
{
    public List<GameObject> events;

    public string jobName;
    public string description;
    public JobSelectScreen jobSelect;
    public Button buttonPrefab;
  
    public void ShowButton(Transform buttonGroup)
    {
        Button thisButton = Instantiate(buttonPrefab, buttonGroup);

        // Set listener
        thisButton.onClick.AddListener(delegate {
            jobSelect.ChoiceChosen(this);
        });
    }
}