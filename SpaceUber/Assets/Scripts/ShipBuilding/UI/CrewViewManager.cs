/* Frank Calabrese
 * 3/9/21
 * CrewViewManager.cs
 * allows player to toggle crewView on or off.
 */

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
