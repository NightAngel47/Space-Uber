/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the state of the game while the player is playing.
 *              Uses the AdditiveSceneManager to Load/Unload scenes for each state.
 */

using System.Collections;
using UnityEngine;

/// <summary>
/// The states that the game can be in:
///     JobSelect       player is picking a job.
///     ShipBuilding    player is editing their ship layout.
///     Events          player can run into story and random events.
///     Ending          player has reached a narrative ending.
/// </summary>
public enum InGameStates { None, JobSelect, ShipBuilding, CrewManagement, Events, Ending, Mutiny, Death ,CrewPayment }

/// <summary>
/// Manages the state of the game while the player is playing.
/// Calls the AdditiveSceneManager to manage scenes on state changes.
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
    public InGameStates currentGameState { get; private set; } = InGameStates.None;

    /// <summary>
    /// Reference to the AdditiveSceneManager used to
    /// Load/Unload scenes additvely when the game state changes.
    /// </summary>
    private AdditiveSceneManager additiveSceneManager;

    /// <summary>
    /// Reference to the JobManager used to refresh the job list in job select state
    /// </summary>
    private JobManager jobManager;
    
    private ShipStats ship;

    private Tick tick;

    [SerializeField] private int secondsPerTick = 5;

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

        // Sets the reference to the JobManager in the active scene
        jobManager = FindObjectOfType<JobManager>();
        
        ship = FindObjectOfType<ShipStats>();

        tick = FindObjectOfType<Tick>();
        tick.SecondsPerTick = secondsPerTick;
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    /// <summary>
    /// Delay starting the game when loaded in.
    /// This give the time for the additive scene manager to clear, before loading new scenes.
    /// </summary>
    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        ChangeInGameState(InGameStates.JobSelect);
    }

    private void Update()
    {
        // I was getting errors in scripts trying to access GameManager.instance.  Hopefully this fixes it.
        if (instance == null)
        {
            instance = this;
        }
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
            case InGameStates.JobSelect: // Loads Jobpicker for the player to pick their job
                // unload ending screen if replaying
                additiveSceneManager.UnloadScene("PromptScreen_End");
                additiveSceneManager.UnloadScene("PromptScreen_Death");
                additiveSceneManager.UnloadScene("PromptScreen_Mutiny");

                additiveSceneManager.UnloadScene("CrewPayment");

                additiveSceneManager.LoadSceneSeperate("Interface_JobList");
                additiveSceneManager.LoadSceneSeperate("Starport BG");
                jobManager.RefreshJobList();
                break;
            case InGameStates.ShipBuilding: // Loads ShipBuilding for the player to edit their ship
                additiveSceneManager.UnloadScene("CrewManagement");

                additiveSceneManager.LoadSceneSeperate("ShipBuilding");
                break;
            case InGameStates.CrewManagement:
                additiveSceneManager.UnloadScene("ShipBuilding");

                additiveSceneManager.LoadSceneSeperate("CrewManagement");
                break;
            case InGameStates.Events: // Unloads ShipBuilding and starts the Travel coroutine for the event system.
                additiveSceneManager.UnloadScene("PromptScreen_End");
                additiveSceneManager.UnloadScene("PromptScreen_Death");
                additiveSceneManager.UnloadScene("PromptScreen_Mutiny");
                additiveSceneManager.UnloadScene("CrewPayment");
                
                additiveSceneManager.UnloadScene("Starport BG");
                
                ship.SaveStats();

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
                //StartCoroutine(EventSystem.instance.Travel());
                StartCoroutine(EventSystem.instance.PlayIntro());
                break;

            case InGameStates.CrewPayment:
                additiveSceneManager.UnloadScene("CrewManagement");
                additiveSceneManager.LoadSceneSeperate("CrewPayment");
                break;
            case InGameStates.Ending: // Loads the PromptScreen_End when the player reaches a narrative ending.
                additiveSceneManager.UnloadScene("CrewPayment");
                additiveSceneManager.LoadSceneSeperate("PromptScreen_End");
                break;
            case InGameStates.Mutiny: // Loads the PromptScreen_Mutiny when the player reaches a mutiny.
                additiveSceneManager.LoadSceneSeperate("PromptScreen_Mutiny");
                break;
            case InGameStates.Death: // Loads the PromptScreen_Death when the player reaches a death.
                additiveSceneManager.LoadSceneSeperate("PromptScreen_Death");
                break;
            default: // Output Warning when the passed in game state doesn't have a transition setup.
                Debug.LogWarning($"The passed in game state, {state.ToString()}, doesn't have a transition setup.");
                break;
        }
    }
}
