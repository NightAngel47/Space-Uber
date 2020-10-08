/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the game states. Loads scenes.
 */

using System;
using UnityEngine;

public enum InGameStates { JobSelect, ShipBuilding, Events }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    /// <summary>
    /// The current Game State
    /// </summary>
    public static InGameStates currentGameState { get; private set; } = InGameStates.JobSelect;

    private void Awake()
    {
        //Singleton pattern
        if(instance) { Destroy(gameObject); }
        else { instance = this; }
    }

    public void ChangeInGameState(InGameStates state)
    {
        if (state != currentGameState)
        {
            currentGameState = state;
            
            AdditiveSceneManager asm = FindObjectOfType<AdditiveSceneManager>();

            switch (state)
            {
                case InGameStates.ShipBuilding:
                    asm.LoadSceneSeperate("ShipBuilding");
                    asm.UnloadScene("PromptScreen");
                    break;
                case InGameStates.Events:
                    asm.UnloadScene("ShipBuilding");
                    StartCoroutine(EventSystem.instance.Travel());
                    break;
            }
        }
    }
}
