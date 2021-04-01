using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameControlDropdownBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] miniGameControls;
    [SerializeField] private int defaultIndex;
    
    void OnEnable()
    {
        GetComponent<TMP_Dropdown>().value = defaultIndex;
        UpdateMenu();
    }
    
    public void UpdateMenu()
    {
        foreach(GameObject control in miniGameControls)
        {
            control.SetActive(false);
        }
        miniGameControls[GetComponent<TMP_Dropdown>().value].SetActive(true);
    }
}
