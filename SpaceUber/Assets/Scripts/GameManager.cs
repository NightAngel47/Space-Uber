/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the state of the game while the player is playing.
 *              Uses the AdditiveSceneManager to Load/Unload scenes for each state.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The states that the game can be in:
///     JobSelect       player is picking a job.
///     ShipBuilding    player is editing their ship layout.
///     Events          player can run into story and random events.
///     Ending          player has reached a narrative ending.
/// </summary>
public enum InGameStates { None, JobSelect, ShipBuilding, Events, MoneyEnding, MoraleEnding, Mutiny, Death, JobPayment, CrewPayment, RoomUnlock, EndingStats, EndingCredits, GameIntro}

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

    [SerializeField] private List<ResourceDataType> resourceDataRef = new List<ResourceDataType>();
    
    [HideInInspector] public bool hasLoadedRooms = false;

    public List<GameObject> allRoomList = new List<GameObject>();

    /// <summary>
    /// Group 1: hydroponics, Bunks, VIP Lounge, Armor plating
    /// </summary>
    private int currentMaxLvlGroup1 = 1;

    /// <summary>
    /// Group 2: Shield generator, photon torpedoes, armory, pantry
    /// </summary>
    private int currentMaxLvlGroup2 = 1;

    /// <summary>
    /// Group 3: Storage Container, energy cannon, core charging terminal, brig
    /// </summary>
    private int currentMaxLvlGroup3 = 1;

    /// <summary>
    /// Sets the instance of the GameManager using the Singleton pattern.
    /// Finds the AdditiveSceneManager and sets it to additiveSceneManager.
    /// </summary>
    private void Awake()
    {
        // Singleton pattern that makes sure that there is only one GameManager
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // Sets the reference to the AdditiveSceneManager in the active scene.
        additiveSceneManager = FindObjectOfType<AdditiveSceneManager>();

        // Sets the reference to the JobManager in the active scene
        jobManager = FindObjectOfType<JobManager>();

        ship = FindObjectOfType<ShipStats>();
    }

    /// <summary>
    /// Delay starting the game when loaded in.
    /// This give the time for the additive scene manager to clear, before loading new scenes.
    /// </summary>
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => additiveSceneManager && ship && jobManager);
        if (SavingLoadingManager.instance.GetHasSave() && SavingLoadingManager.instance.Load<bool>("hasRooms"))
        {
            LoadGameState();
            yield return new WaitForEndOfFrame();
            switch (currentGameState)
            {
                case InGameStates.ShipBuilding:
                    yield return new WaitUntil(() => FindObjectOfType<SpotChecker>());
                    yield return new WaitUntil(() => FindObjectOfType<CrewManagement>());
                    SavingLoadingManager.instance.LoadRoomLevels();
                    break;
                case InGameStates.Events:
                    yield return new WaitUntil(() => FindObjectOfType<SpotChecker>());
                    break;
                default:
                    Debug.LogWarning("In Game stat not setup for loading.");
                    break;
            }
            SavingLoadingManager.instance.LoadRooms();
            hasLoadedRooms = true;
        }
        else //new game
        {
            //ChangeInGameState(InGameStates.JobSelect);
            ChangeInGameState(InGameStates.GameIntro);
            SavingLoadingManager.instance.NewSave(); // start new save here
            hasLoadedRooms = true;
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
            case InGameStates.GameIntro:
                additiveSceneManager.LoadSceneMerged("Event_NoChoices");
                additiveSceneManager.LoadSceneMerged("Game_Intro");
                break;
            case InGameStates.JobSelect: // Loads Jobpicker for the player to pick their job
                // unload game intro 
                additiveSceneManager.UnloadScene("Event_NoChoices");
                additiveSceneManager.UnloadScene("Game_Intro");
                
                // unload normal
                additiveSceneManager.UnloadScene("Interface_Runtime");
                additiveSceneManager.UnloadScene("Interface_GameOver");
                additiveSceneManager.UnloadScene("Interface_CrewPaymentScreen");
                additiveSceneManager.UnloadScene("Interface_RoomUnlockScreen");
                
                // save game stuffs (moved from crew payment)
                SaveGameState();
                ship.SaveShipStats();
                MoraleManager.instance.SaveMorale();
                ship.cStats.SaveCharacterStats();
                SavingLoadingManager.instance.SaveRooms();
                
                additiveSceneManager.LoadSceneSeperate("Interface_JobList");
                additiveSceneManager.LoadSceneSeperate("Starport BG");
                jobManager.RefreshJobList();
                break;
            case InGameStates.ShipBuilding: // Loads ShipBuilding for the player to edit their ship
                additiveSceneManager.UnloadScene("Interface_JobList");
                additiveSceneManager.UnloadScene("Interface_Runtime");
                additiveSceneManager.UnloadScene("Interface_GameOver");

                additiveSceneManager.LoadSceneSeperate("Starport BG");
                additiveSceneManager.LoadSceneSeperate("ShipBuilding");
                additiveSceneManager.LoadSceneSeperate("CrewManagement");
                SaveGameState();
                break;
            case InGameStates.RoomUnlock:
                additiveSceneManager.UnloadScene("Interface_CrewPaymentScreen");
                additiveSceneManager.UnloadScene("Interface_JobPaycheckScreen");
                additiveSceneManager.UnloadScene("Interface_Runtime");

                additiveSceneManager.LoadSceneSeperate("Interface_RoomUnlockScreen");  
                break;
            case InGameStates.Events: // Unloads ShipBuilding and starts the Travel coroutine for the event system.
                additiveSceneManager.UnloadScene("CrewManagement");
                additiveSceneManager.UnloadScene("PromptScreen_End");
                additiveSceneManager.UnloadScene("Interface_GameOver");
                additiveSceneManager.UnloadScene("Starport BG");
                additiveSceneManager.UnloadScene("ShipBuilding");

                // if loading from continue
                if (!FindObjectOfType<SpotChecker>())
                {
                    StartCoroutine(LoadSpotCheckerInShipBuilding());
                }
                else // if coming from crew management
                {
                    SaveGameState();
                    MoraleManager.instance.SaveMorale();
                    ship.cStats.SaveCharacterStats();
                    ship.SaveShipStats();
                    SavingLoadingManager.instance.SaveRooms();
                }
                
                additiveSceneManager.LoadSceneSeperate("Interface_Runtime");

                StartCoroutine(EventSystem.instance.PlayJobIntro());
                break;
            case InGameStates.JobPayment:
                additiveSceneManager.UnloadScene("Interface_Runtime");
                additiveSceneManager.UnloadScene("Event_General");
                additiveSceneManager.UnloadScene("Event_CharacterFocused");
                
                additiveSceneManager.LoadSceneSeperate("Interface_JobPaycheckScreen");
                break;
            case InGameStates.CrewPayment:
                additiveSceneManager.UnloadScene("Interface_JobPaycheckScreen");

                additiveSceneManager.LoadSceneSeperate("Interface_CrewPaymentScreen");
                break;
            case InGameStates.MoneyEnding: // Loads the PromptScreen_Money_End when the player reaches a narrative ending.
                additiveSceneManager.UnloadScene("Interface_JobList");
                additiveSceneManager.UnloadScene("Interface_Runtime");
                additiveSceneManager.UnloadScene("Interface_Radio");
                additiveSceneManager.UnloadScene("Interface_CrewPaymentScreen");

                additiveSceneManager.LoadSceneSeperate("PromptScreen_Money_End");
                break;
            case InGameStates.MoraleEnding: // Loads the PromptScreen_Morale_End after the PromptScreen_Money_End.
                additiveSceneManager.UnloadScene("PromptScreen_Money_End");

                additiveSceneManager.LoadSceneSeperate("PromptScreen_Morale_End");
                break;
            case InGameStates.EndingStats: // Loads the Interface_EndScreen_Stats after the PromptScreen_Morale_End.
                additiveSceneManager.UnloadScene("PromptScreen_Morale_End");
                
                additiveSceneManager.LoadSceneSeperate("Interface_EndScreen_Stats");
                break;
            case InGameStates.EndingCredits: // Loads the Credits after the Interface_EndScreen_Stats.
                additiveSceneManager.UnloadScene("Interface_EndScreen_Stats");
                
                additiveSceneManager.LoadSceneSeperate("Credits");
                break;
            case InGameStates.Mutiny: // Loads the PromptScreen_Mutiny when the player reaches a mutiny.
                additiveSceneManager.UnloadScene("Event_General");
                additiveSceneManager.UnloadScene("Event_CharacterFocused");
                additiveSceneManager.UnloadScene("Event_Prompt");
                additiveSceneManager.UnloadScene("Interface_Runtime");

                additiveSceneManager.LoadSceneSeperate("Interface_GameOver");
                break;
            case InGameStates.Death: // Loads the PromptScreen_Death when the player reaches a death.
                additiveSceneManager.UnloadScene("Event_General");
                additiveSceneManager.UnloadScene("Event_CharacterFocused");
                additiveSceneManager.UnloadScene("Event_Prompt");
                additiveSceneManager.UnloadScene("Interface_Runtime");

                additiveSceneManager.LoadSceneSeperate("Interface_GameOver");
                break;
            default: // Output Warning when the passed in game state doesn't have a transition setup.
                Debug.LogWarning($"The passed in game state, {state.ToString()}, doesn't have a transition setup.");
                break;
        }
    }

    public ResourceDataType GetResourceData(int i)
    {
        return resourceDataRef[i];
    }

    private IEnumerator LoadSpotCheckerInShipBuilding()
    {
        // load ship building for spot checker to load into don't destroy on load
        additiveSceneManager.LoadSceneSeperate("ShipBuilding");
        yield return new WaitUntil(() => SceneManager.GetSceneByName("ShipBuilding").isLoaded);
        additiveSceneManager.UnloadScene("ShipBuilding"); // unload cause not needed anymore
    }

    private void SaveGameState()
    {
        SavingLoadingManager.instance.Save<InGameStates>("currentGameState", currentGameState);
    }

    private void LoadGameState()
    {
        ChangeInGameState(SavingLoadingManager.instance.Load<InGameStates>("currentGameState"));
    }

    public int GetUnlockLevel(int roomGroup)
    {
        switch(roomGroup)
        {
            case 1:
                return currentMaxLvlGroup1;
            case 2:
                return currentMaxLvlGroup2;
            case 3:
                return currentMaxLvlGroup3;
            default:
                return 0;
        }
    }

    public void SetUnlockLevel(int roomGroup, int newValue)
    {
        switch (roomGroup)
        {
            case 1:
                currentMaxLvlGroup1 = newValue;
                break;
            case 2:
                currentMaxLvlGroup2 = newValue;
                break;
            case 3:
                currentMaxLvlGroup3 = newValue;
                break;
            default:
                break;
        }
    }
}
