/*
 * TransitionGameStates.cs
 * Author(s): Steven Drovie []
 * Created on: 9/20/2020 (en-US)
 * Description:
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class TransitionGameStates : MonoBehaviour
{
    private ShipStats ship;

    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
    }

    public void ChangeToJobSelect()
    {
        GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
    }

    public void ChangeToShipBuilding()
    {
        GameManager.instance.ChangeInGameState(InGameStates.ShipBuilding);

        AnalyticsManager.OnEnteringStarport(ship);
    }

    public void ChangeToEvents()
    {
        // Remove unplaced rooms from the ShipBuilding state
        if (!ObjectMover.hasPlaced)
        {
            ObjectMover.hasPlaced = true;
            Destroy(FindObjectOfType<ObjectMover>().gameObject);
        }

        AnalyticsManager.OnLeavingStarport(ship);

        GameManager.instance.ChangeInGameState(InGameStates.Events);
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }

    public void ChangeToCrewManagement()
    {
        GameManager.instance.ChangeInGameState(InGameStates.CrewManagement);
    }

    public void ChangeToEnd()
    {
        GameManager.instance.ChangeInGameState(InGameStates.MoneyEnding);
    }
}
