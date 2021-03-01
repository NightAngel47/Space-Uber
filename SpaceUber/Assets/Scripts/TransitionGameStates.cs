/*
 * TransitionGameStates.cs
 * Author(s): Steven Drovie []
 * Created on: 9/20/2020 (en-US)
 * Description: 
 */

using System;
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
        AnalyticsManager.OnLeavingStarport(ship);
        //TODO add overclock button turn on, currently adding it so it appears but needs to be better can remove tag when updated
        FindObjectOfType<CrewManagement>().FinishWithCrewAssignment();

        GameManager.instance.ChangeInGameState(InGameStates.Events);
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }

    public void ChangeToCrewManagement()
    {
        ship.cantPlaceText.SetActive(false);
        GameManager.instance.ChangeInGameState(InGameStates.CrewManagement);
    }
    
    public void ChangeToEnd()
    {
        GameManager.instance.ChangeInGameState(InGameStates.MoneyEnding);
    }
}
