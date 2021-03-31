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
    private AdditiveSceneManager additiveSceneManager;

    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] foodBaseAdjustments = new float[3];
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] securityBaseAdjustments = new float[3];
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] shipWeaponsBaseAdjustments = new float[3];
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] energyBaseAdjustments = new float[3];
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] hullRepairBaseAdjustments = new float[3];
    [SerializeField, Tooltip("Adjustment value multiplied by minigame output after finishing a minigame based on room level."), Foldout("Mini-Game Adjustments")] 
    float[] failHullDurabilityBaseAdjustments = new float[3];
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
    [HideInInspector] public OverclockRoom activeRoom;
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
        int roomLevel = activeRoom.GetComponent<RoomStats>().GetRoomLevel() - 1;
        
        if(success)
		{
            float moraleModifier = MoraleManager.instance.GetMoraleModifier();

            switch (miniGame)
            {
                case MiniGameType.Security:
                    shipStats.Security += Mathf.RoundToInt(securityBaseAdjustments[roomLevel] * statModification * moraleModifier);
                    SpawnStatChangeText(Mathf.RoundToInt(securityBaseAdjustments[roomLevel] * statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon);
                    EventSystem.instance.chanceOfEvent += securityPercentIncrease;
                    break;
                case MiniGameType.Asteroids:
                    shipStats.ShipWeapons += Mathf.RoundToInt(shipWeaponsBaseAdjustments[roomLevel] * statModification * moraleModifier);
                    SpawnStatChangeText(Mathf.RoundToInt(shipWeaponsBaseAdjustments[roomLevel] * statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon);
                    EventSystem.instance.chanceOfEvent += asteroidPercentIncrease;
                    break;
                case MiniGameType.CropHarvest:
                    shipStats.Food += Mathf.RoundToInt(foodBaseAdjustments[roomLevel] * statModification * moraleModifier);
                    SpawnStatChangeText(Mathf.RoundToInt(foodBaseAdjustments[roomLevel] * statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon);
                    EventSystem.instance.chanceOfEvent += cropPercentIncrease;
                    break;
                case MiniGameType.StabilizeEnergyLevels:
                    shipStats.Energy += new Vector3(Mathf.RoundToInt(energyBaseAdjustments[roomLevel] * statModification * moraleModifier), 0, 0);
                    SpawnStatChangeText(Mathf.RoundToInt(energyBaseAdjustments[roomLevel] * statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon);
                    EventSystem.instance.chanceOfEvent += energyPercentIncrease;
                    break;
                case MiniGameType.SlotMachine:
                    shipStats.Credits += Mathf.RoundToInt(statModification * moraleModifier); // slots doesn't have a room level scaler?
                    SpawnStatChangeText(Mathf.RoundToInt(statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon);
                    EventSystem.instance.chanceOfEvent += slotPercentIncrease;
                    break;
                case MiniGameType.HullRepair:
                    shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(hullRepairBaseAdjustments[roomLevel] * statModification * moraleModifier), 0);
                    SpawnStatChangeText(Mathf.RoundToInt(hullRepairBaseAdjustments[roomLevel] * statModification * moraleModifier), GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
                    EventSystem.instance.chanceOfEvent += hullRepairPercentIncrease;
                    break;
            }
        }
        else
        {
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(failHullDurabilityBaseAdjustments[roomLevel] * statModification), 0);
                SpawnStatChangeText(Mathf.RoundToInt(failHullDurabilityBaseAdjustments[roomLevel] * statModification), GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon);
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
        EndingStats.instance.AddToStat(1, EndingStatTypes.MinigamesPlayed);
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
}
