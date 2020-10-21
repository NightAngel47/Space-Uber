/*
 * MiniGameCrop.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: 
 */

using System.Collections;
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
    [SerializeField] GameObject harvestableCropPrefab = null;
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
    public float pruneLevel = 1;

    float growth = 0;
    bool isGrowing = false;
    bool hasBeenPlanted = false;
    bool isHarvestable = false;

    //Plant image gets hidden by setting its color to transparent. This is used to restort plant image color when it is not being hidden.
    Color originalColor;
    CropHarvestMiniGame miniGameManager;

    private void Start()
	{
        originalColor = plantImage.color;
        miniGameManager = GameObject.FindGameObjectWithTag("MiniGameScoreManager").GetComponent<CropHarvestMiniGame>();

        harvestableCropPrefab = Instantiate(harvestableCropPrefab, transform.position, new Quaternion(), transform.parent);
        harvestableCropPrefab.SetActive(false);
        //Let minGameManager know how many crops are needed to beat the mini game
        miniGameManager.IncrementRequiredScore();
    }

	void Update()
    {
        if(stage == CropStage.Seedling && !isGrowing) { StartCoroutine(Grow()); }

        //Midlings do not need to be trimmed
        if(stage != CropStage.Midling) { isTrimmed = true; pruneLevel = maxPruneLevel; }

        //Expose harvestable crop so the player can drag it into the bin
        if (stage == CropStage.Harvestable && !isHarvestable) 
        {
            isHarvestable = true;
            harvestableCropPrefab.SetActive(true);
        }

        ShowHidePlantImage();

		if (isFertilized) { GetComponent<Image>().sprite = fertilizedSoil; }
        else { GetComponent<Image>().sprite = unfertilizedSoil; }

        ChoosePlantImage();
    }

    private void OnMouseDown()
    {
        if (CropHarvestMiniGame.selectedTool != null)
        {
            if (CropHarvestMiniGame.selectedTool.toolType == MiniGameToolType.Clippers && !isTrimmed) 
            {
                isTrimmed = true;
                Vector3 deadLeafPosition = transform.position;
                deadLeafPosition.y -= 1.5f;
                Instantiate(deadLeavesPrefab, deadLeafPosition, new Quaternion(), transform.parent);

                //Let mini game manager know that dead leaves are required to be thrown away in order to win.
                miniGameManager.IncrementRequiredScore();
                pruneLevel = maxPruneLevel;
            }
            if (CropHarvestMiniGame.selectedTool.toolType == MiniGameToolType.WateringCan && !isWatered) 
            { 
                isWatered = true;
                waterLevel = maxWaterLevel;
            }
            if (CropHarvestMiniGame.selectedTool.toolType == MiniGameToolType.Fertilizer && !isFertilized && !hasBeenPlanted) 
            {
                hasBeenPlanted = true;
                isFertilized = true;
            }
             if (CropHarvestMiniGame.selectedTool.toolType == MiniGameToolType.Seed && isFertilized && stage == CropStage.Unplanted)
            { 
                stage = CropStage.Seedling;
            }
        }
    }

    /// <summary>
    /// Plant image is show or hidden based off of crop stage
    /// </summary>
    void ShowHidePlantImage()
	{ 
        if (stage != CropStage.Unplanted && stage != CropStage.Harvestable)
        {
            isFertilized = true;
            plantImage.color = originalColor;
        }
        else { plantImage.color = Color.clear; }
    }

    /// <summary>
    /// Crop image is chosen based off of stage, if it needs watered and or trimmed
    /// </summary>
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

            waterLevel -= waterDecayPerTic; 
            if( waterLevel < 0) { waterLevel = 0; }

            //Prevents situations where the water and prune level drain back to back making it so the plant doesn't grow
            if (isWatered) pruneLevel -= pruneDecayerTic;
            if(pruneLevel < 0 ) { pruneLevel = 0; }

            if(waterLevel == 0f) { isWatered = false; }
            if(pruneLevel == 0f) { isTrimmed = false; }

            //Plant only grows if it is watered and trimmed
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