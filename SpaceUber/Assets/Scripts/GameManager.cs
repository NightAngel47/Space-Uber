/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the game states. Loads scenes.
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InGameStates { JobSelect, ShipBuilding, Events, Ending }

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

    private void Start()
    {
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }

    public void ChangeInGameState(InGameStates state)
    {
        if (state != currentGameState)
        {
            currentGameState = state;
            
            AdditiveSceneManager asm = FindObjectOfType<AdditiveSceneManager>();

            switch (state)
            {
                case InGameStates.JobSelect:
                    // unload ending screen if replaying
                    // TODO remove when we have menus
                    if (SceneManager.GetSceneByName("EndingScreen").isLoaded)
                    {
                        asm.UnloadScene("EndingScreen");
                    }
                    asm.LoadSceneSeperate("PromptScreen"); // TODO Change to Job List when we have it
                    break;
                case InGameStates.ShipBuilding:
                    asm.LoadSceneSeperate("ShipBuilding");
                    asm.UnloadScene("PromptScreen");
                    break;
                case InGameStates.Events:
                    asm.UnloadScene("ShipBuilding");
                    StartCoroutine(EventSystem.instance.Travel());
                    break;
                case InGameStates.Ending:
                    asm.LoadSceneSeperate("EndingScreen");
                    break;
            }
        }
    }
}
