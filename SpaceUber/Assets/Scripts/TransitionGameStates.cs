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
    public void ChangeToJobSelect()
    {
        GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
    }
    
    public void ChangeToShipBuilding()
    {
        GameManager.instance.ChangeInGameState(InGameStates.ShipBuilding);
    }

    public void ChangeToEvents()
    {
        GameManager.instance.ChangeInGameState(InGameStates.Events);
    }
}
