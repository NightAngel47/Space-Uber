/*
 * ShipStatsHoverUI.cs
 * Author(s): Sydney Foe []
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTooltipBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    private void Start()
    {
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}
