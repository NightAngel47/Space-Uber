using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveAndFadeBehaviour : MonoBehaviour
{
    public Vector2 offset;
    [RangeAttribute(0.0f, 1.0f)]
    public float moveTime;
    public float fadeTime;
    
    public Color green;
    public Color red;
    
    private Vector2 destination;
    private RectTransform rect;
    private TMP_Text text;
    private float timeSinceFadeStart;
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
        
        destination = rect.anchoredPosition + offset;
        
        timeSinceFadeStart = 0;
    }
    
    void Update()
    {
        //determines what proportion of the distance to the destination to move the object, depending on deltaTime
        float moveProportion = Mathf.Pow(moveTime, Time.deltaTime);
        
        //move the object towards the destination
        rect.anchoredPosition = moveProportion * rect.anchoredPosition + (1.0f - moveProportion) * destination;
        
        if (Vector2.Distance(rect.anchoredPosition, destination) < 0.01)
        {
            float alpha;
            if(fadeTime <= 0)
            {
                alpha = 0;
            }
            else
            {
                alpha = 1 - (timeSinceFadeStart / fadeTime);
            }
            
            if(alpha <= 0)
            {
                Destroy(this);
            }
            
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            timeSinceFadeStart += Time.deltaTime;
        }
    }
    
    public void SetValue(int value)
    {
        string sign;
        if(value >= 0)
        {
            sign = "+";
            text.color = green;
        }
        else
        {
            sign = "-";
            text.color = red;
        }
        
        text.text = sign + " " + value;
    }
}
