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
    Camera cam;

    void Start()
    {
        originalLayer = gameObject.layer;
        originalPosition = transform.position;
        cam = Camera.main;
    }

    void Update()
    {
        if (isBeingDraged)
        {
            if(toolType == MiniGameToolType.WateringCan) 
            {
				if (!GetComponentInChildren<ParticleSystem>().isPlaying) { GetComponentInChildren<ParticleSystem>().Play(); }
            }
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
            transform.SetSiblingIndex(transform.parent.childCount -1);
        }
		else
		{
            if (toolType == MiniGameToolType.WateringCan)
            {
                if (GetComponentInChildren<ParticleSystem>().isPlaying) { GetComponentInChildren<ParticleSystem>().Stop(); }
            }
        }
        if(CropHarvestMiniGame.selectedTool != this) 
        {
            isBeingDraged = false;
            transform.position = originalPosition;
            gameObject.layer = originalLayer;
        }
        if (Input.GetMouseButtonDown(1))
        {
            CropHarvestMiniGame.selectedTool = null;
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