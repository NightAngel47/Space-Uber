/*
 * MiniGameTool.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class MiniGameTool : MonoBehaviour
{
    public MiniGameToolType toolType = MiniGameToolType.Clippers;
    Vector3 originalPosition = Vector3.zero;
    bool isBeingDraged = false;
    int originalLayer = 0;

    void Start()
    {
        originalLayer = gameObject.layer;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isBeingDraged)
        {
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            transform.position = mousePosition;
        }
        if(CropHarvestMiniGame.selectedTool != this) 
        {
            isBeingDraged = false;
            transform.position = originalPosition;
        }
        if (Input.GetMouseButtonDown(1))
        {
            CropHarvestMiniGame.selectedTool = null;
            gameObject.layer = originalLayer;
        }
    }

    private void OnMouseDown() 
    {
            isBeingDraged = true;
            CropHarvestMiniGame.selectedTool = this;
            gameObject.layer = 2;
    }
}

public enum MiniGameToolType { WateringCan, Fertilizer, Clippers, Seed }