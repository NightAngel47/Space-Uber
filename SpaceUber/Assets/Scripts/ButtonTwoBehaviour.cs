/*
 * ButtonTwoBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 2/28/2021 (en-US)
 * Description: 
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTwoBehaviour : MonoBehaviour
{
    private Button button;
    private TMP_Text[] buttonTexts = new TMP_Text[2];
    
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    
    void Awake()
    {
        button = GetComponent<Button>();
        buttonTexts = GetComponentsInChildren<TMP_Text>();
    }

    public void SetButtonInteractable(bool state)
    {
        if (state)
        {
            button.interactable = true;
            foreach (TMP_Text text in buttonTexts)
            {
                text.color = activeColor;
            }
        }
        else
        {
            button.interactable = false;
            foreach (TMP_Text text in buttonTexts)
            {
                text.color = inactiveColor;
            }
        }
    }
    
    public void GoToEvent()
    {
        button.onClick.AddListener(EventSystem.instance.SkipToEvent);
    }
    
    public void ToggleInteractable()
    {
        SetButtonInteractable(!button.interactable);
    }
}
