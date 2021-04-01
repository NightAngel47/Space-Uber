/*
 * MenuTabBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 3/31/2021 (en-US)
 * Description: 
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuTabBehaviour : MonoBehaviour
{
    [SerializeField] private Color activeChildColor = Color.white;
    [SerializeField] private Color disabledChildColor;

    private Button button;
    private Image icon;
    private TMP_Text label;

    private void Awake()
    {
        button = GetComponent<Button>();
        icon = GetComponentInChildren<Image>();
        label = GetComponentInChildren<TMP_Text>();
    }

    // toggle button interactable
    public void ToggleInteractable()
    {
        button.interactable = !button.interactable;
        ChangeChildColors(button.interactable);
    }

    public void SetInteractableState(bool state)
    {
        button.interactable = state;
        ChangeChildColors(state);
    }

    // change child colors based on new state
    private void ChangeChildColors(bool state)
    {
        if (state)
        {
            icon.color = activeChildColor;
            label.color = activeChildColor;
        }
        else
        {
            icon.color = disabledChildColor;
            label.color = disabledChildColor;
        }
    }
}
