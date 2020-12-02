using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionTracker : MonoBehaviour
{
    private Vector2 startPos;
    
    void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }
    
    public Vector2 GetPos()
    {
        return startPos;
    }
}
