/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description: 
 */

using System.Collections;
using UnityEngine;

public enum MiniGameType { None, CropHarvest, Security, Asteroids, StabilizeEnergyLevels}

public class OverclockController : MonoBehaviour
{
    public static OverclockController instance;
    private ShipStats shipStats;
    private AdditiveSceneManager additiveSceneManager;

    [SerializeField] float foodBaseAdjustment = 1;
    [SerializeField] float securityBaseAdjustment = 1;
    [SerializeField] float shipWeaponsBaseAdjustment = 1;
    [SerializeField] float hullDurabilityBaseAdjustment = 1;
    public float cooldownTime = 5;

    //If a room is already being overclocked
    public bool overclocking = false;
    OverclockRoom activeRoom;

	private void Awake()
	{
		if (!instance) { instance = this; }
        else { Destroy(gameObject); }
	}

    void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
        additiveSceneManager = FindObjectOfType<AdditiveSceneManager>();
    }

    public void StartMiniGame(MiniGameType miniGame, OverclockRoom room)
    {
        if (miniGame == MiniGameType.None) return; // check for implemented mini-game
        overclocking = true;
        activeRoom = room;
        additiveSceneManager.LoadSceneMerged(miniGame.ToString()); 
    }

    public void EndMiniGame(MiniGameType miniGame, bool succsess, float statModification = 0)
	{
        overclocking = false;
        if(succsess)
		{
            if (miniGame == MiniGameType.Security) { shipStats.UpdateSecurityAmount( Mathf.RoundToInt(securityBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.Asteroids) { shipStats.UpdateShipWeaponsAmount(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.CropHarvest) { shipStats.UpdateFoodAmount(Mathf.RoundToInt(foodBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.StabilizeEnergyLevels) { shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(hullDurabilityBaseAdjustment * statModification)); }
        }
        additiveSceneManager.UnloadScene(miniGame.ToString());
		if (succsess && activeRoom) { activeRoom.StartCoolDown(); }
        activeRoom = null;
	}
}
