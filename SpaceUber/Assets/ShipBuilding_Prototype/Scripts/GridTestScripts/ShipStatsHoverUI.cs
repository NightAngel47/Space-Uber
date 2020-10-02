/*
 * ShipStatsHoverUI.cs
 * Author(s): Sydney Foe []
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.EventSystems;

public class ShipStatsHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
