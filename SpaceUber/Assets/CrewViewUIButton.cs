using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewViewUIButton : MonoBehaviour
{
    CrewViewManager crewViewManager;
    void Start()
    {
        crewViewManager = FindObjectOfType<CrewViewManager>();
    }
    public void CloseCrewView()
    {
        crewViewManager.DisableCrewView();
    }
}
