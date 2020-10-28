/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description: 
 */

using System.Collections;
using UnityEngine;

public class OverclockController : MonoBehaviour
{
    public static OverclockController instance;
    ShipStats shipStats;

    [SerializeField] string foodMiniGameSceneName = "CropHarvest";
    [SerializeField] string securityMiniGameSceneName = "Security";
    [SerializeField] string shipWeaponsMiniGameSceneName = "Asteroids";
    [SerializeField] string hullDurabilityMiniGameSceneName = "StabilizeEnergyLevels";

    [SerializeField] float foodBaseAdjustment = 1;
    [SerializeField] float securityBaseAdjustment = 1;
    [SerializeField] float shipWeaponsBaseAdjustment = 1;
    [SerializeField] float hullDurabilityBaseAdjustment = 1;

    //If a room is already being overclocked
    public bool overclocking = false;
    public bool miniGameInProgress = false;

	private void Awake()
	{
		if (!instance) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
	}

	void Start() { shipStats = FindObjectOfType<ShipStats>(); }

    public void StartMiniGame(string miniGame) { FindObjectOfType<AdditiveSceneManager>().LoadSceneMerged(miniGame); }

    public void EndMiniGame(string miniGame, bool succsess, float statModification)
	{
        if(succsess)
		{
            if (miniGame == securityMiniGameSceneName) { shipStats.UpdateSecurityAmount( Mathf.RoundToInt(securityBaseAdjustment * statModification)); }
            if (miniGame == shipWeaponsMiniGameSceneName) { shipStats.UpdateShipWeaponsAmount(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification)); }
            if (miniGame == foodMiniGameSceneName) { shipStats.UpdateFoodAmount(Mathf.RoundToInt(foodBaseAdjustment * statModification)); }
            if (miniGame == hullDurabilityMiniGameSceneName) { shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(hullDurabilityBaseAdjustment * statModification)); }
        }
        FindObjectOfType<AdditiveSceneManager>().UnloadScene(miniGame);
	}

    public void CallStartOverclocking(int moraleAmount, string resourceType = null, int resourceAmount = 0)
    {
        StartCoroutine(StartOverclocking(moraleAmount, resourceType, resourceAmount));
    }

    public void CallStopOverClocking(string resourceType = null, int resourceAmount = 0)
    {
        StopOverclocking(resourceType, resourceAmount);
    }

    private IEnumerator StartOverclocking(int moraleAmount, string resourceType = "", int resourceAmount = 0)
    {
        overclocking = true;
        if (resourceType == securityMiniGameSceneName){ shipStats.UpdateSecurityAmount(resourceAmount); }
        if (resourceType == shipWeaponsMiniGameSceneName){ shipStats.UpdateShipWeaponsAmount(resourceAmount); }
        if (resourceType == foodMiniGameSceneName){ shipStats.UpdateFoodAmount(resourceAmount); }
        if (resourceType == "Food Per Tick") {shipStats.UpdateFoodPerTickAmount(resourceAmount); }
        if (resourceType == hullDurabilityMiniGameSceneName) { shipStats.UpdateHullDurabilityAmount(resourceAmount); }
        
        shipStats.UpdateCrewMorale(moraleAmount);
        yield return new WaitForSecondsRealtime(5.0f);
        StopOverclocking(resourceType, resourceAmount);
    }

    private void StopOverclocking(string resourceType = "", int resourceAmount = 0)
    {
        overclocking = false;
        switch (resourceType)
        {
            case "Credits":
                shipStats.UpdateCreditsAmount(-resourceAmount);
                break;
            case "Energy":
                shipStats.UpdateEnergyAmount(-resourceAmount, -resourceAmount);
                break;
            case "Security":
                shipStats.UpdateSecurityAmount(-resourceAmount);
                break;
            case "Ship Weapons":
                shipStats.UpdateShipWeaponsAmount(-resourceAmount);
                break;
            case "Food":
                shipStats.UpdateFoodAmount(-resourceAmount);
                break;
            case "Food Per Tick":
                shipStats.UpdateFoodPerTickAmount(-resourceAmount);
                break;
            case "Hull Durability":
                shipStats.UpdateHullDurabilityAmount(-resourceAmount);
                break;
            default:
                break;
        }
    }
}
