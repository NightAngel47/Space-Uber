/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description:
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiniGameType { None, CropHarvest, Security, Asteroids, StabilizeEnergyLevels, SlotMachine, HullRepair }

public class OverclockController : MonoBehaviour
{
    public static OverclockController instance;
    private ShipStats shipStats;
    private AdditiveSceneManager additiveSceneManager;

    [SerializeField] float foodBaseAdjustment = 1;
    [SerializeField] float securityBaseAdjustment = 1;
    [SerializeField] float shipWeaponsBaseAdjustment = 1;
    [SerializeField] float energyBaseAdjustment = 1;
    [SerializeField] float hullRepairBaseAdjustment = 5;
    [SerializeField] float failHullDurabilityBaseAdjustment = -5;
    public float cooldownTime = 5;

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
    }

    public void EndMiniGame(MiniGameType miniGame, bool succsess, float statModification = 0)
	{
        if(succsess)
		{
            float moraleModifier = MoraleManager.instance.GetMoraleModifier();

            if(miniGame == MiniGameType.Security)
            {
                shipStats.Security += Mathf.RoundToInt(securityBaseAdjustment * statModification * moraleModifier);
                SpawnStatChangeText(Mathf.RoundToInt(securityBaseAdjustment * statModification * moraleModifier), 1);
            }
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.ShipWeapons += Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification * moraleModifier);
                SpawnStatChangeText(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification * moraleModifier), 2);
            }
            if(miniGame == MiniGameType.CropHarvest)
            {
                shipStats.Food += Mathf.RoundToInt(foodBaseAdjustment * statModification * moraleModifier);
                SpawnStatChangeText(Mathf.RoundToInt(foodBaseAdjustment * statModification * moraleModifier), 3);
            }
            if(miniGame == MiniGameType.StabilizeEnergyLevels)
            {
                shipStats.EnergyRemaining += new Vector2(Mathf.RoundToInt(energyBaseAdjustment * statModification * moraleModifier), 0);
                SpawnStatChangeText(Mathf.RoundToInt(energyBaseAdjustment * statModification * moraleModifier), 6);
            }
            if(miniGame == MiniGameType.SlotMachine)
            {
                shipStats.Credits += Mathf.RoundToInt(statModification * moraleModifier);
                SpawnStatChangeText(Mathf.RoundToInt(statModification * moraleModifier), 0);
            }
            if(miniGame == MiniGameType.HullRepair)
            {
                shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification * moraleModifier), 0);
                SpawnStatChangeText(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification * moraleModifier), 6);
            }
        }
        else
        {
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.ShipHealthCurrent += new Vector2(Mathf.RoundToInt(failHullDurabilityBaseAdjustment * statModification), 0);
                SpawnStatChangeText(Mathf.RoundToInt(failHullDurabilityBaseAdjustment * statModification));
            }
        }
        if(succsess && activeRoom)
        {
           activeRoom.StartCoolDown();
            if(winSound == false)
            {
                AudioManager.instance.PlaySFX("De-Overclock");
                winSound = true;
            }
        }
        activeRoom = null;
        //FindObjectOfType<CrewManagement>().crewManagementText.SetActive(true);
	}

    private void SpawnStatChangeText(int value, int icon = -1)
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

    public void UnloadScene(MiniGameType miniGame)
    {
        overclocking = false;
        additiveSceneManager.UnloadScene(miniGame.ToString());
    }
}
