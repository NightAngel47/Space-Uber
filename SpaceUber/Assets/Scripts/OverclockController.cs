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

    //If a room is already being overclocked
    public bool overclocking = false;
    public bool miniGameInProgress = false;

	private void Awake()
	{
		if (!instance) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
	}

	void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void StartMiniGame(string miniGame)
	{
        FindObjectOfType<AdditiveSceneManager>().LoadSceneSeperate(miniGame);
	}

    public void EndMiniGame(string miniGame, bool succsess)
	{
        //TODO If successful change stats
        FindObjectOfType<AdditiveSceneManager>().UnloadScene(miniGame);
	}

    public void CallStartOverclocking(int moraleAmount, string resourceType = null, int resourceAmount = 0)
    {
        StartCoroutine(StartOverclocking(moraleAmount, resourceType, resourceAmount));
    }

    public void CallStopOverClocking(string resourceType = null, int resourceAmount = 0)
    {
        StartCoroutine(StopOverclocking(resourceType, resourceAmount));
    }

    private IEnumerator StartOverclocking(int moraleAmount, string resourceType = "", int resourceAmount = 0)
    {
        overclocking = true;
        switch (resourceType)
        {
            case "Credits":
                shipStats.UpdateCreditsAmount(resourceAmount);
                break;
            case "Energy":
                shipStats.UpdateEnergyAmount(resourceAmount, resourceAmount);
                break;
            case "Security":
                shipStats.UpdateSecurityAmount(resourceAmount);
                break;
            case "Ship Weapons":
                shipStats.UpdateShipWeaponsAmount(resourceAmount);
                break;
            case "Food":
                shipStats.UpdateFoodAmount(resourceAmount);
                break;
            case "Food Per Tick":
                shipStats.UpdateFoodPerTickAmount(resourceAmount);
                break;
            case "Hull Durability":
                shipStats.UpdateHullDurabilityAmount(resourceAmount);
                break;
            default:
                break;
        }
        shipStats.UpdateCrewMorale(moraleAmount);
        yield return new WaitForSecondsRealtime(5.0f);
        StartCoroutine(StopOverclocking(resourceType, resourceAmount));
    }

    private IEnumerator StopOverclocking(string resourceType = "", int resourceAmount = 0)
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
        yield return null;
    }
}
