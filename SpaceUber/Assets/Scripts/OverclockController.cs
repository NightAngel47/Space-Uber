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
    [SerializeField] float hullDurabilityBaseAdjustment = 1;
    [SerializeField] float hullRepairBaseAdjustment = 5;
    [SerializeField] float failHullDurabilityBaseAdjustment = -5;
    public float cooldownTime = 5;

    //If a room is already being overclocked
    public bool overclocking = false;
    OverclockRoom activeRoom;
    bool winSound = false;

	private void Awake()
	{
		if (!instance) { instance = this; }
        else { Destroy(gameObject); }
	}

    void Start()
    {
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
            if(miniGame == MiniGameType.Security)
            {
                shipStats.UpdateSecurityAmount(Mathf.RoundToInt(securityBaseAdjustment * statModification));
                SpawnStatChangeText(Mathf.RoundToInt(securityBaseAdjustment * statModification));
            }
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.UpdateShipWeaponsAmount(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification));
                SpawnStatChangeText(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification));
            }
            if(miniGame == MiniGameType.CropHarvest)
            {
                shipStats.UpdateFoodAmount(Mathf.RoundToInt(foodBaseAdjustment * statModification));
                SpawnStatChangeText(Mathf.RoundToInt(foodBaseAdjustment * statModification));
            }
            if(miniGame == MiniGameType.StabilizeEnergyLevels)
            {
                shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(hullDurabilityBaseAdjustment * statModification));
                SpawnStatChangeText(Mathf.RoundToInt(hullDurabilityBaseAdjustment * statModification));
            }
            if(miniGame == MiniGameType.SlotMachine)
            {
                shipStats.UpdateCreditsAmount(Mathf.RoundToInt(statModification));
                SpawnStatChangeText(Mathf.RoundToInt(statModification));
            }
            if(miniGame == MiniGameType.HullRepair)
            {
                shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification));
                SpawnStatChangeText(Mathf.RoundToInt(hullRepairBaseAdjustment * statModification));
            }
        }
        else
        {
            if(miniGame == MiniGameType.Asteroids)
            {
                shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(failHullDurabilityBaseAdjustment * statModification));
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
        FindObjectOfType<CrewManagement>().crewManagementText.SetActive(true);
	}
    
    private void SpawnStatChangeText(int value)
    {
        GameObject statChangeText = shipStats.GetComponent<ShipStatsUI>().statChangeText;
        GameObject instance = GameObject.Instantiate(statChangeText);
        
        RectTransform rect = instance.GetComponent<RectTransform>();
        
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(activeRoom.transform.GetChild(0).position);
        rect.anchoredPosition = new Vector2(spawnPos.x, spawnPos.y);
        
        instance.transform.parent = shipStats.GetComponent<ShipStatsUI>().canvas;
        
        MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
        moveAndFadeBehaviour.offset = new Vector2(0, 25 + activeRoom.transform.GetChild(0).localPosition.y * 100);
        moveAndFadeBehaviour.SetValue(value);
    }

    public void UnloadScene(MiniGameType miniGame) 
    {
        overclocking = false;
        additiveSceneManager.UnloadScene(miniGame.ToString()); 
    }
}
