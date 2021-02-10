/*
 * EventPromptButton.cs
 * Author(s): Sam Ferstein
 * Created on: 2/6/2021 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class EventPromptButton : MonoBehaviour
{
    public GameObject backDrop;
    public Button eventButton;

    private void Start()
    {
        backDrop.SetActive(false);
        if(OverclockController.instance.overclocking)
        {
            eventButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(eventButton.gameObject.activeSelf && OverclockController.instance.overclocking)
        {
            eventButton.gameObject.SetActive(false);
        }
        else if(!eventButton.gameObject.activeSelf && !OverclockController.instance.overclocking)
        {
            eventButton.gameObject.SetActive(true);
        }
    }
}
