/*
 * OverclockRoom.cs
 * Author(s): Grant Frey
 * Created on: 9/25/2020
 * Description: 
 */

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverclockRoom : MonoBehaviour
{
    [Tooltip("Name of mini game scene")]
    [SerializeField] MiniGameType miniGame;

    public bool hasEvents = false;

    [SerializeField, Tooltip("All events that can happen with this room"),ShowIf("hasEvents")]
    private List<GameObject> roomEvents;

    private bool minigameCooledDown = true;

    public void PlayMiniGame()
    {
	    if (GameManager.instance.currentGameState == InGameStates.Events 
	        && !EventSystem.instance.eventActive && !OverclockController.instance.overclocking && minigameCooledDown)
	    {
		    OverclockController.instance.StartMiniGame(miniGame, this);
	    }
    }

    public void StartMinigameCooldown() { StartCoroutine(MinigameCooldown()); }

    private IEnumerator MinigameCooldown()
	{
        minigameCooledDown = false;
        yield return new WaitForSeconds(OverclockController.instance.cooldownTime);
        minigameCooledDown = true;
	}

    public MiniGameType GetMiniGame()
    {
        return miniGame;
    }

    public List<GameObject> GetEvents()
    {
        return roomEvents;
    }
}
