using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnScreenBehaviour : MonoBehaviour
{
    private RectTransform rect;
    private Vector2 pos;
    private Camera cam;
    
    //bounds are in viewport space
    public float topBound;
    public float bottomBound;
    public float leftBound;
    public float rightBound;
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
        pos = rect.anchoredPosition;
        cam = Camera.main;
    }
    
    void Update()
    {
        rect.anchoredPosition = pos;
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        Vector3 bottomLeft = cam.WorldToViewportPoint(corners[0]);
        Vector3 topRight = cam.WorldToViewportPoint(corners[2]);
        
        if(bottomLeft.x < leftBound)
        {
            bottomLeft.x = leftBound;
        }
        
        if(bottomLeft.y < bottomBound)
        {
            bottomLeft.y = bottomBound;
        }
        
        if(topRight.x > rightBound)
        {
            topRight.x = rightBound;
        }
        
        if(topRight.y > topBound)
        {
            topRight.y = topBound;
        }
        
        Vector3 change = (cam.ViewportToWorldPoint(bottomLeft) - corners[0]) + (cam.ViewportToWorldPoint(topRight) - corners[2]);
        rect.anchoredPosition = new Vector2(pos.x + change.x, pos.y + change.y);
    }
}
