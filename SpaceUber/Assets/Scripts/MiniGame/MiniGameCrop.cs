/*
 * MiniGameCrop.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCrop : MonoBehaviour
{
    public bool isWatered = false;
    public bool isTrimmed = false;
    public bool isFertilized = false;
    public CropStage stage = CropStage.Seedling;
    [SerializeField] Sprite unfertilizedSoil = null;
    [SerializeField] Sprite fertilizedSoil = null;
    [SerializeField] Sprite unwateredSeed = null;
    [SerializeField] Sprite wateredSeed = null;
    [SerializeField] Sprite unwateredSprout = null;
    [SerializeField] Sprite watererSprout = null;
    [SerializeField] Sprite unwateredUntrimmedMiddling = null;
    [SerializeField] Sprite unwateredTrimmedMiddling = null;
    [SerializeField] Sprite wateredUntrimmedMiddling = null;
    [SerializeField] Sprite wateredTrimmedMiddling = null;
    [SerializeField] Sprite harvestable = null;
    [SerializeField] GameObject deadLeavesPrefab = null;
    [SerializeField] Image plantImage = null;
    [Tooltip("Tag of crop container")]
    [SerializeField] string targetTag = "";
    [Tooltip("Amount of seconds a growth tic takes.")]
    [SerializeField] float growthTicTime = 1;
    [Tooltip("Amount of growth added per tic")]
    [SerializeField] float growthPerTic = 1;
    [Tooltip("Amount of growth before crop needs advanced to the next stage")]
    [SerializeField] float maxGrowthPerStage = 1;
    [Tooltip("Amount of water lost per tic")]
    [SerializeField] float waterDecayPerTic = 1;
    [Tooltip("Amount of water before crop needs to be watered again")]
    [SerializeField] float maxWaterLevel = 1;
    [Tooltip("Amount of prune level lost per tic")]
    [SerializeField] float pruneDecayerTic = 1;
    [Tooltip("Amount of prune level before crop needs to be pruned again")]
    [SerializeField] float maxPruneLevel = 1;
    float waterLevel = 1;
    float pruneLevel = 1;

    float growth = 0;
    bool isGrowing = false;
    bool isBeingDraged = false;
    bool isOverTarget = false;

    Color originalColor;
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
        if(stage == CropStage.Seedling && !isGrowing) { StartCoroutine(Grow()); }
        if(stage != CropStage.Midling) { isTrimmed = true; pruneLevel = maxPruneLevel; }
        if (stage == CropStage.Harvestable) 
        {
            isWatered = true;
            waterLevel = maxWaterLevel;
            isTrimmed = true;
            pruneLevel = maxPruneLevel;
        }
        ShowHidePlantImage();

		if (isFertilized) { GetComponent<Image>().sprite = fertilizedSoil; }
        else { GetComponent<Image>().sprite = unfertilizedSoil; }

        ChoosePlantImage();

        FollowCursor();
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
                pruneLevel = maxPruneLevel;
                scoreManager.IncrementScore();
            }
            if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.WateringCan && !isWatered) 
            { 
                isWatered = true;
                waterLevel = maxWaterLevel;
                scoreManager.IncrementScore();
            }
            if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.Fertilizer && !isFertilized) 
            {
                isFertilized = true;
                scoreManager.IncrementScore();
            }
             if (MiniGameScoreManager.selectedTool.toolType == MiniGameToolType.Seed && isFertilized && stage == CropStage.Unplanted)
            { 
                stage = CropStage.Seedling;
                scoreManager.IncrementScore();
            }
        }
        else if (stage == CropStage.Harvestable) { isBeingDraged = true; }

    }
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = true; } }

    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = false; } }

    void FollowCursor()
	{
        if (isBeingDraged)
        {
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            plantImage.gameObject.transform.position = mousePosition;
        }
    }

    void ShowHidePlantImage()
	{ 
        if (stage != CropStage.Unplanted)
        {
            isFertilized = true;
            plantImage.color = originalColor;
        }
        else { plantImage.color = Color.clear; }
    }

    void ChoosePlantImage()
	{
        switch (stage)
        {
            case CropStage.Seedling:
                if (isWatered) { plantImage.sprite = wateredSeed; }
                else { plantImage.sprite = unwateredSeed; }
                break;
            case CropStage.Sprout:
				if (isWatered) { plantImage.sprite = watererSprout; }
                else { plantImage.sprite = unwateredSprout; }
                break;
            case CropStage.Midling:
                if (isWatered && isTrimmed) { plantImage.sprite = wateredTrimmedMiddling; }
                else if (!isWatered && isTrimmed) { plantImage.sprite = unwateredTrimmedMiddling; }
                else if (isWatered && !isTrimmed) { plantImage.sprite = wateredUntrimmedMiddling; }
                else if (!isWatered && !isTrimmed) { plantImage.sprite = unwateredUntrimmedMiddling; }
                break;
            case CropStage.Harvestable:
                plantImage.sprite = harvestable;
                break;
        }
    }

    void AdvancedToNextStage()
	{
        switch(stage)
		{
            case CropStage.Seedling: stage = CropStage.Sprout; break;
            case CropStage.Sprout: stage = CropStage.Midling; break;
            case CropStage.Midling: stage = CropStage.Harvestable; break;
		}
	}

    IEnumerator Grow()
	{
        isGrowing = true;
        while(isGrowing)
		{
            yield return new WaitForSeconds(growthTicTime);

            if (isTrimmed) { waterLevel -= waterDecayPerTic; }
            if( waterLevel < 0) { waterLevel = 0; }

            if (isWatered) pruneLevel -= pruneDecayerTic;
            if(pruneLevel < 0 ) { pruneLevel = 0; }

            if(waterLevel == 0f) { isWatered = false; }
            if(pruneLevel == 0f) { isTrimmed = false; }

            if(isWatered && isTrimmed) { growth += growthPerTic; }
            if(growth > maxGrowthPerStage)
			{
                growth = 0;
                AdvancedToNextStage();
			}
            if(stage == CropStage.Harvestable) { isGrowing = false; }
		}
	}
}

public enum CropStage { Harvestable, Seedling, Sprout,  Midling, Unplanted}