using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSettingsUpdator : MonoBehaviour
{
    [SerializeField] private VideoSettingsManager videoSettingsManager;
    
    void Start()
    {
        videoSettingsManager.UpdateSettingsEarly();
    }
}
