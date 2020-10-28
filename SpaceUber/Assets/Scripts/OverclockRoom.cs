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
    float cooldownTime = 5;

	private void OnMouseDown()
    { 
        if (!OverclockController.instance.overclocking && !OverclockController.instance.miniGameInProgress)
        {

            OverclockController.instance.StartMiniGame(miniGame);
        }
    }

    IEnumerator Cooldown()
	{
        //OverclockController.instance.cooldownTime
        yield return new WaitForSeconds(cooldownTime);
	}
}
