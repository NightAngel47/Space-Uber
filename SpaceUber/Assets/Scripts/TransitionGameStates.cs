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
        //TODO add overclock button turn on, currently adding it so it appears but needs to be better can remove tag when updated
        FindObjectOfType<CrewManagement>().TurnOnOverclockButton();
        GameManager.instance.ChangeInGameState(InGameStates.Events);
    }

    public void ChangeToCrewManagement()
    {
        GameManager.instance.ChangeInGameState(InGameStates.CrewManagement);
    }
    
    public void ChangeToEnd()
    {
        GameManager.instance.ChangeInGameState(InGameStates.Ending);
    }
}
