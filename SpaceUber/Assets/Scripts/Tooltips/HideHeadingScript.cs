using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideHeadingScript : MonoBehaviour
{
    public int headerIndex = 0;
    public int listIndex = 1;
    
    void Update()
    {
        if((transform.childCount <= listIndex || transform.GetChild(listIndex).childCount == 0) && transform.childCount > headerIndex)
        {
            transform.GetChild(headerIndex).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(headerIndex).gameObject.SetActive(true);
        }
    }
}
