using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewViewManager : Singleton<CrewViewManager>
{
    private bool crewViewEnabled = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (crewViewEnabled == false) EnableCrewView();
            else DisableCrewView();
        }
    }

    public void EnableCrewView()
    {
        crewViewEnabled = true;
    }
    public void DisableCrewView()
    {
        crewViewEnabled = false;
    }
    public bool GetCrewViewStatus()
    {
        return crewViewEnabled;
    }

}
