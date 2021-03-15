using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelChangeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool isMouseOverLevel = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverLevel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverLevel = false;
    }

    
}
