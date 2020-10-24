/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the state of the game while the player is playing.
 *              Uses the AdditiveSceneManager to Load/Unload scenes for each state.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The states that the game can be in:
///     JobSelect       player is picking a job.
///     ShipBuilding    player is editing their ship layout.
///     Events          player can run into story and random events.
///     Ending          player has reached a narrative ending.
/// </summary>
public enum InGameStates { JobSelect, ShipBuilding, CrewManagement, Events, Ending }

/// <summary>
/// Manages the state of the game while the player is playing.
/// Calls the AdditiveSceneManager to mange scens on state chagnes.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The current instance of the GameManager.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// The current state that the player is at within the game.
    /// </summary>
    public static InGameStates currentGameState { get; private set; } = InGameStates.JobSelect;

    /// <summary>
    /// Reference to the AdditiveSceneManager used to
    /// Load/Unload scenes additvely when the game state changes.
    /// </summary>
    private AdditiveSceneManager additiveSceneManager;

    /// <summary>
    /// Sets the instance of the GameManager using the Singleton pattern.
    /// Finds the AdditiveSceneManager and sets it to additiveSceneManager.
    /// </summary>
    private void Awake()
    {
        // Singleton pattern that makes sure that there is only one GameManager
        if (instance) { Destroy(gameObject); }
        else { instance = this; }

        // Sets the reference to the AdditiveSceneManager in the active scene.
        additiveSceneManager = FindObjectOfType<AdditiveSceneManager>();
    }

    /// <summary>
    /// Changes the current game state to the passed in game state.
    /// Uses the AdditiveSceneManager to Load/Unload scenes.
    /// </summary>
    /// <param name="state">The InGameStates state to transition to</param>
    public void ChangeInGameState(InGameStates state)
    {
        // Checks if the passed in state is the same as the current state.
        if (state == currentGameState) return;
        // Otherwise it sets the current state to the passed state.
        currentGameState = state;
        // Handles what scenes to Load/Unload using the AdditiveSceneManager, along with additional scene cleanup.
        switch (state)
        {
          case InGameStates.JobSelect: // Loads PromptScreen_Start for the player to pick their job
              // unload ending screen if replaying
              if (SceneManager.GetSceneByName("PromptScreen_End").isLoaded) // TODO remove when we have menus
              {
                  additiveSceneManager.UnloadScene("PromptScreen_End");
              }
              if (SceneManager.GetSceneByName("Space BG").isLoaded)
              {
                  additiveSceneManager.UnloadScene("Space BG");
              }

              additiveSceneManager.LoadSceneSeperate("JobPicker");
              break;
          case InGameStates.ShipBuilding: // Loads ShipBuilding for the player to edit their ship
              additiveSceneManager.LoadSceneSeperate("ShipBuilding");
              additiveSceneManager.UnloadScene("JobPicker");
              break;
          case InGameStates.CrewManagement:
              additiveSceneManager.UnloadScene("ShipBuilding");
              additiveSceneManager.LoadSceneSeperate("CrewManagement");
              ObjectScript[] os = FindObjectsOfType<ObjectScript>();
              break;
            case InGameStates.Events: // Unloads ShipBuilding and starts the Travel coroutine for the event system.
              // Remove unplaced rooms from the ShipBuilding state
              if (!ObjectMover.hasPlaced)
              {
                  ObjectMover.hasPlaced = true;
                  Destroy(FindObjectOfType<ObjectMover>().gameObject);
              }
              foreach(RoomStats room in FindObjectsOfType<RoomStats>())
              {
                  room.UpdateUsedRoom();
              }
              additiveSceneManager.UnloadScene("ShipBuilding");
              additiveSceneManager.LoadSceneSeperate("Space BG");
              StartCoroutine(EventSystem.instance.Travel());
              break;
          case InGameStates.Ending: // Loads the PromptScreen_End when the player reaches a narrative ending.
              additiveSceneManager.LoadSceneSeperate("PromptScreen_End");
              break;
          default: // Output Warning when the passed in game state doesn't have a transition setup.
              Debug.LogWarning($"The passed in game state, {state.ToString()}, doesn't have a transition setup.");
              break;
        }
    }
}
