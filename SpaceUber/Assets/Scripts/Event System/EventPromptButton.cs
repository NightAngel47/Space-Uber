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
    public ButtonTwoBehaviour eventButton;
    private float counter = 0;

    private void Awake()
    {
        eventButton = GetComponent<ButtonTwoBehaviour>();
        backDrop.SetActive(false);
        eventButton.SetButtonInteractable(false);
    }

    private void Update()
    {
        if(EventSystem.instance.eventButtonSpawn == true)
        {
            eventButton.SetButtonInteractable(!
                (OverclockController.instance.overclocking || EventSystem.instance.chatting ||
                EventSystem.instance.mutiny || EventSystem.instance.eventActive));
        }
    }
}
