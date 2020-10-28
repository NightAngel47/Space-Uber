/*
 * OverclockController.cs
 * Author(s): Grant Frey
 * Created on: 9/24/2020
 * Description: 
 */

using System.Collections;
using UnityEngine;

public enum MiniGameType { NONE, CropHarvest, Security, Asteroids, StabilizeEnergyLevels}

public class OverclockController : MonoBehaviour
{
    public static OverclockController instance;
    ShipStats shipStats;

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

    public void StartMiniGame(MiniGameType miniGame) { FindObjectOfType<AdditiveSceneManager>().LoadSceneMerged(miniGame.ToString()); }

    public void EndMiniGame(MiniGameType miniGame, bool succsess, float statModification)
	{
        if(succsess)
		{
            if (miniGame == MiniGameType.Security) { shipStats.UpdateSecurityAmount( Mathf.RoundToInt(securityBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.Asteroids) { shipStats.UpdateShipWeaponsAmount(Mathf.RoundToInt(shipWeaponsBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.CropHarvest) { shipStats.UpdateFoodAmount(Mathf.RoundToInt(foodBaseAdjustment * statModification)); }
            if (miniGame == MiniGameType.StabilizeEnergyLevels) { shipStats.UpdateHullDurabilityAmount(Mathf.RoundToInt(hullDurabilityBaseAdjustment * statModification)); }
        }
        FindObjectOfType<AdditiveSceneManager>().UnloadScene(miniGame.ToString());
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
        if (resourceType == MiniGameType.Security.ToString()){ shipStats.UpdateSecurityAmount(resourceAmount); }
        if (resourceType == MiniGameType.Asteroids.ToString()){ shipStats.UpdateShipWeaponsAmount(resourceAmount); }
        if (resourceType == MiniGameType.CropHarvest.ToString()){ shipStats.UpdateFoodAmount(resourceAmount); }
        if (resourceType == "Food Per Tick") {shipStats.UpdateFoodPerTickAmount(resourceAmount); }
        if (resourceType == MiniGameType.StabilizeEnergyLevels.ToString()) { shipStats.UpdateHullDurabilityAmount(resourceAmount); }
        
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
