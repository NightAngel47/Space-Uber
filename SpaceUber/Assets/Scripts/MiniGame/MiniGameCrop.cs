/*
 * MiniGameCrop.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: 
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCrop : MonoBehaviour
{
    public bool isWatered;
    public bool isTrimmed;
    public bool isFertilized;
    public CropStage stage = CropStage.Seedling;
    [SerializeField] Sprite unfertilizedSoil;
    [SerializeField] Sprite fertilizedSoil;
    [SerializeField] Sprite unwateredSeed;
    [SerializeField] Sprite wateredSeed;
    [SerializeField] Sprite unwateredUntrimmedMiddling;
    [SerializeField] Sprite unwateredTrimmedMiddling;
    [SerializeField] Sprite wateredUntrimmedMiddling;
    [SerializeField] Sprite wateredTrimmedMiddling;
    [SerializeField] Sprite harvestable;
    [SerializeField] GameObject deadLeavesPrefab;
    [SerializeField] Image plantImage;
    [SerializeField] string targetTag;
    Color originalColor;
    bool isBeingDraged = false;
    bool isOverTarget = false;
    MiniGameScoreManager scoreManager;
    Vector3 originalPosition;
    private void Start()
	{
        originalColor = plantImage.color;
        originalPosition = transform.position;
        scoreManager = GameObject.FindGameObjectWithTag("MiniGameScoreManager").GetComponent<MiniGameScoreManager>();
    }

	void Update()
    {
        if(stage != CropStage.Midling) { isTrimmed = true; }
        if(stage != CropStage.Unplanted) 
        { 
            isFertilized = true;
            plantImage.color = originalColor;
        }
        else { plantImage.color = Color.clear; }
        if (stage == CropStage.Harvestable) { isWatered = true; }
		if (isFertilized) { GetComponent<Image>().sprite = fertilizedSoil; }
        else { GetComponent<Image>().sprite = unfertilizedSoil; }
        switch(stage)
		{
            case CropStage.Seedling:
				if (isWatered) { plantImage.sprite = wateredSeed; }
                else { plantImage.sprite = unwateredSeed; }
                break;
            case CropStage.Midling:
                if(isWatered && isTrimmed) { plantImage.sprite = wateredTrimmedMiddling; }
                else if (!isWatered && isTrimmed) { plantImage.sprite = unwateredTrimmedMiddling; }
                else if(isWatered && !isTrimmed) { plantImage.sprite = wateredUntrimmedMiddling; }
                else if (!isWatered && !isTrimmed) { plantImage.sprite = unwateredUntrimmedMiddling; }
                break;
            case CropStage.Harvestable:
                plantImage.sprite = harvestable;
                break;
		}
        if (isBeingDraged)
        {
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            plantImage.gameObject.transform.position = mousePosition;
        }
    }
    private void OnMouseUp()
    {
        isBeingDraged = false;
        if (isOverTarget)
        {
            stage = CropStage.Unplanted;
            scoreManager.IncrementScore();
        }
        else { plantImage.transform.position = originalPosition; }
    }
    private void OnMouseDown()
    {
        if (MiniGameScoreManager.selectedTool != null)
        {
            if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.Clippers && !isTrimmed) 
            {
                isTrimmed = true;
                Vector3 deadLeafPosition = originalPosition;
                deadLeafPosition.y -= 1.5f;
                Instantiate(deadLeavesPrefab, deadLeafPosition, new Quaternion(), transform);
            }
            if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.WateringCan) { isWatered = true; }
            if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.Fertilizer) { isFertilized = true; }
            if (!isWatered && MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.WateringCan) { isWatered = true; }
             if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.Seed && isFertilized && stage == CropStage.Unplanted)
            { stage = CropStage.Seedling; }
        }
        else if (stage == CropStage.Harvestable) { isBeingDraged = true; }

    }
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = true; } }

    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = false; } }
}

public enum CropStage { Harvestable, Seedling, Midling, Unplanted}