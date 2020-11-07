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
    bool cooledDown = true;

	private void OnMouseDown()
    {
	    // TODO replace with proper UI call
	    PlayMiniGame();
    }

    public void PlayMiniGame()
    {
	    if (GameManager.instance.currentGameState == InGameStates.Events 
	        && !EventSystem.instance.eventActive && !OverclockController.instance.overclocking && cooledDown)
	    {
		    OverclockController.instance.StartMiniGame(miniGame, this);
	    }
    }

    public void StartCoolDown() { StartCoroutine(Cooldown()); }

    IEnumerator Cooldown()
	{
        cooledDown = false;
        yield return new WaitForSeconds(OverclockController.instance.cooldownTime);
        cooledDown = true;
	}

    public MiniGameType GetMiniGame()
    {
        return miniGame;
    }
}
