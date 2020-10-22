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
                    if (SceneManager.GetSceneByName("PromptScreen_End").isLoaded)
                    {
                        asm.UnloadScene("PromptScreen_End");
                    }
                    if (SceneManager.GetSceneByName("Space BG").isLoaded)
                    {
                        asm.UnloadScene("Space BG");
                    }
                    asm.LoadSceneMerged("JobPicker");
                    break;
                case InGameStates.ShipBuilding:
                    asm.LoadSceneSeperate("ShipBuilding");
                    asm.UnloadScene("JobPicker");
                    break;
                case InGameStates.Events:
                    if (!ObjectMover.hasPlaced) // Remove left over room from ship building before moving to events
                    {
                        ObjectMover.hasPlaced = true;
                        Destroy(FindObjectOfType<ObjectMover>().gameObject);
                    }
                    asm.UnloadScene("ShipBuilding");
                    asm.LoadSceneSeperate("Space BG");
                    StartCoroutine(EventSystem.instance.Travel());
                    break;
                case InGameStates.Ending:
                    asm.LoadSceneSeperate("PromptScreen_End");
                    break;
            }
        }
    }
}
