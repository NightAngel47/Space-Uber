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
    [SerializeField] private Material[] activeMaterials;
    [SerializeField] private Material[] inactiveMaterials;
    
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
            for(int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].color = activeColor;
                buttonTexts[i].fontMaterial = activeMaterials[i];
            }
        }
        else
        {
            button.interactable = false;
            for(int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].color = inactiveColor;
                buttonTexts[i].fontMaterial = inactiveMaterials[i];
            }
        }
    }
    
    public void ToggleInteractable()
    {
        SetButtonInteractable(!button.interactable);
    }
}
