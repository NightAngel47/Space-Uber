/*
 * OverclockRoom.cs
 * Author(s): Grant Frey
 * Created on: 9/25/2020
 * Description: 
 */

using System;
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
    public bool MinigameCooledDown => minigameCooledDown;

    private CrewManagementRoomDetailsMenu roomDetailsMenu;

    private void Start()
    {
	    roomDetailsMenu = FindObjectOfType<CrewManagementRoomDetailsMenu>();
    }

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
        roomDetailsMenu.SetOvertimeButtonState(minigameCooledDown);
        
        yield return new WaitForSeconds(OverclockController.instance.cooldownTime);
        
        minigameCooledDown = true;
        roomDetailsMenu.SetOvertimeButtonState(minigameCooledDown);
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
