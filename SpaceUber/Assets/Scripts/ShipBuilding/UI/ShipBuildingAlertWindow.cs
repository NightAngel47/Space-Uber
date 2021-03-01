/*
 * ShipBuildingAlertWindow.cs
 * Author(s): Steven Drovie []
 * Created on: 2/28/2021 (en-US)
 * Description: 
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildingAlertWindow : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text statText;
    [SerializeField, Min(0.0f)] private float activeTime = 2f;
    
    private static readonly int isOpen = Animator.StringToHash("isOpen");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenAlert(ResourceDataType resourceDataType)
    {
        icon.sprite = resourceDataType.resourceIcon;
        statText.text = resourceDataType.resourceName;
        
        animator.SetBool(isOpen, true);
        Invoke(nameof(CloseAlert), activeTime);
    }

    private void CloseAlert()
    {
        animator.SetBool(isOpen, false);
    }
}
