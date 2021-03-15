/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description:
 */

using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum MiniGameType { None, CropHarvest, Security, Asteroids, StabilizeEnergyLevels, SlotMachine, HullRepair }

public class OverclockController : MonoBehaviour
{
    public static OverclockController instance;
    private ShipStats shipStats;
    private RoomStats roomStats;
    private AdditiveSceneManager additiveSceneManager;

    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float foodBaseAdjustment = 1;
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float securityBaseAdjustment = 1;
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float shipWeaponsBaseAdjustment = 1;
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float energyBaseAdjustment = 1;
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float hullRepairBaseAdjustment = 5;
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame."), Foldout("Mini-Game Adjustments")] 
    float failHullDurabilityBaseAdjustment = -5;
    public float cooldownTime = 5;

    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float cropPercentIncrease = 5;
    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float securityPercentIncrease = 5;
    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float asteroidPercentIncrease = 5;
    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float energyPercentIncrease = 5;
    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float hullRepairPercentIncrease = 5;
    [SerializeField, Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame."), Foldout("Mini-Game Event Spawn Increase Chances")] 
    float slotPercentIncrease = 5;

    //If a room is already being overclocked
    public bool overclocking = false;
    OverclockRoom activeRoom;
    bool winSound = false;
    private Camera cam;

    private void Awake()
	{
		if (!instance) { instance = this; }
        else { Destroy(gameObject); }
	}

    void Start()
    {
        cam = Camera.main;
        shipStats = FindObjectOfType<ShipStats>();
        roomStats = FindObjectOfType<RoomStats>();
        additiveSceneManager = FindObjectOfType<AdditiveSceneManager>();
        winSound = false;
    }

    public ShipStats ShipStats() { return shipStats; }

    public void StartMiniGame(MiniGameType miniGame, OverclockRoom room)
    {
        if (miniGame == MiniGameType.None) return; // check for implemented mini-game
        overclocking = true;
        activeRoom = room;
        AudioManager.instance.PlaySFX("Overclock");
        additiveSceneManager.LoadSceneMerged(miniGame.ToString());

        AnalyticsManager.OnMiniGameStarted(miniGame);
    }

    public void EndMiniGame(MiniGameType miniGame, bool success, float statModification = 0)
	{
        if(success)
		{
            float moraleModifier = MoraleManager.instance.GetMoraleModifier();

            if(miniGame == MiniGameType.Security)
            {
                shipStats.Security += Mathf.RoundToInt(securityBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.Security));
                SpawnStatChangeText(Mathf.RoundToInt(securityBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.Security)), GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                EventSystem.instance.chanceOfEvent += securityPercentIncrease;
            }
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.ShipWeapons += Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.Asteroids));
                SpawnStatChangeText(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.Asteroids)), GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                EventSystem.instance.chanceOfEvent += asteroidPercentIncrease;
            }
            if(miniGame == MiniGameType.CropHarvest)
            {
                shipStats.Food += Mathf.RoundToInt(foodBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.CropHarvest));
                SpawnStatChangeText(Mathf.RoundToInt(foodBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.CropHarvest)), GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                EventSystem.instance.chanceOfEvent += cropPercentIncrease;
            }
            if(miniGame == MiniGameType.StabilizeEnergyLevels)
            {
                shipStats.EnergyRemaining += new Vector2(Mathf.RoundToInt(energyBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.StabilizeEnergyLevels)), 0);
                SpawnStatChangeText(Mathf.RoundToInt(energyBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.StabilizeEnergyLevels)), GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                EventSystem.instance.chanceOfEvent += energyPercentIncrease;
            }
            if(miniGame == MiniGameType.SlotMachine)
            {
                shipStats.Credits += Mathf.RoundToInt(statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.SlotMachine));
                SpawnStatChangeText(Mathf.RoundToInt(statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.SlotMachine)), GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon);
                EventSystem.instance.chanceOfEvent += slotPercentIncrease;
            }
            if(miniGame == MiniGameType.HullRepair)
            {
                shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.HullRepair)), 0);
                SpawnStatChangeText(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification * moraleModifier * RoomLevelMultiplier(MiniGameType.HullRepair)), GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
                EventSystem.instance.chanceOfEvent += hullRepairPercentIncrease;
            }
        }
        else
        {
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(failHullDurabilityBaseAdjustment * statModification), 0);
                SpawnStatChangeText(Mathf.RoundToInt(failHullDurabilityBaseAdjustment * statModification), GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
            }
        }
        if(success && activeRoom)
        {
           activeRoom.StartMinigameCooldown();
            if(winSound == false)
            {
                AudioManager.instance.PlaySFX("De-Overclock");
                winSound = true;
            }
        }
        activeRoom = null;
        //FindObjectOfType<CrewManagement>().crewManagementText.SetActive(true);
        
        AnalyticsManager.OnMiniGameFinished(miniGame, success, statModification);
	}


    private void SpawnStatChangeText(int value, Sprite icon = null)
    {
        ShipStatsUI shipStatsUI = shipStats.GetComponent<ShipStatsUI>();
        GameObject statChangeUI = Instantiate(shipStatsUI.statChangeText);

        RectTransform rect = statChangeUI.GetComponent<RectTransform>();

        Vector3 spawnPos = cam.WorldToScreenPoint(activeRoom.transform.GetChild(0).position);
        rect.anchoredPosition = new Vector2(spawnPos.x, spawnPos.y);

        statChangeUI.transform.parent = shipStats.GetComponent<ShipStatsUI>().canvas; // you have to set the parent after you change the anchored position or the position gets messed up.  Don't set it in the instantiation.  I don't know why someone decided to change that.

        MoveAndFadeBehaviour moveAndFadeBehaviour = statChangeUI.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, 25 + activeRoom.transform.GetChild(0).localPosition.y * 100);
        moveAndFadeBehaviour.SetValue(value, icon);
    }

    /// <summary>
    /// Unloads this current minigame scene
    /// </summary>
    /// <param name="miniGame"></param>
    public void UnloadScene(MiniGameType miniGame)
    {
        overclocking = false;
        additiveSceneManager.UnloadScene(miniGame.ToString());
    }

    private float RoomLevelMultiplier(MiniGameType minigame)
    {
        float multiplier = 1;

        if ((roomStats.GetRoomLevel() - 1) == 1)
        {
            multiplier = 1;
        }
        else if ((roomStats.GetRoomLevel() - 1) == 2)
        {
            switch (minigame)
            {
                case MiniGameType.Asteroids:
                    multiplier = 2;
                    break;
                case MiniGameType.CropHarvest:
                    multiplier = 2;
                    break;
                case MiniGameType.HullRepair:
                    multiplier = 2;
                    break;
                case MiniGameType.Security:
                    multiplier = 2;
                    break;
                case MiniGameType.SlotMachine:
                    multiplier = 2;
                    break;
                case MiniGameType.StabilizeEnergyLevels:
                    multiplier = 2;
                    break;
            }
        }
        else if ((roomStats.GetRoomLevel() - 1) == 3)
        {
            switch (minigame)
            {
                case MiniGameType.Asteroids:
                    multiplier = 3;
                    break;
                case MiniGameType.CropHarvest:
                    multiplier = 3;
                    break;
                case MiniGameType.HullRepair:
                    multiplier = 3;
                    break;
                case MiniGameType.Security:
                    multiplier = 3;
                    break;
                case MiniGameType.SlotMachine:
                    multiplier = 3;
                    break;
                case MiniGameType.StabilizeEnergyLevels:
                    multiplier = 3;
                    break;
            }
        }
        else if ((roomStats.GetRoomLevel() - 1) == 4)
        {
            switch (minigame)
            {
                case MiniGameType.Asteroids:
                    multiplier = 4;
                    break;
                case MiniGameType.CropHarvest:
                    multiplier = 4;
                    break;
                case MiniGameType.HullRepair:
                    multiplier = 4;
                    break;
                case MiniGameType.Security:
                    multiplier = 4;
                    break;
                case MiniGameType.SlotMachine:
                    multiplier = 4;
                    break;
                case MiniGameType.StabilizeEnergyLevels:
                    multiplier = 4;
                    break;
            }
        }

        return multiplier;
    }
}
