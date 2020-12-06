using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMouseOver : MonoBehaviour
{
    public void OnMouseOver()
    {
        transform.parent.GetComponent<ObjectScript>().OnMouseOver();
    }
    
    public void OnMouseExit()
    {
        transform.parent.GetComponent<ObjectScript>().OnMouseExit();
    }
}
