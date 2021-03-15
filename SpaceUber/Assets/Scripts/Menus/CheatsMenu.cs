/*
 * CheatsMenu.cs
 * Author(s): Scott, Lachlan
 * Created on: 3/8/2021 (en-US)
 * Description: Cheats
 */

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheatsMenu : MonoBehaviour
{
    public static CheatsMenu instance;
    private EventSystem es;
    [SerializeField] private CampaignManager campMan;
    private ShipStats thisShip;
    private JobManager jm;

    private AdditiveSceneManager asm;

    public GameObject myCanvas;

    public GameObject cheatModeActiveText;
    private bool showingActiveText = true;

    public GameObject helpMenu;
    private bool showingHelpMenu = false;

    private TMP_InputField inputField;

    [SerializeField] private List<GameObject> randomEvents;
    [SerializeField] private List<GameObject> characterEvents;

    private bool inMinigameTest = false;
    
    [HideInInspector] public bool deathDisabled = false;
    [HideInInspector] public bool mutinyDisabled = false;

    // Start is called before the first frame update
    void Awake()
    {
        //Singleton pattern
        if (instance)
        {
            instance.myCanvas = myCanvas;
            instance.cheatModeActiveText = cheatModeActiveText;
            instance.helpMenu = helpMenu;
            
            instance.myCanvas.SetActive(false);
            instance.cheatModeActiveText.SetActive(instance.showingActiveText);
            instance.helpMenu.SetActive(instance.showingHelpMenu);
            
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        es = FindObjectOfType<EventSystem>();
        campMan = FindObjectOfType<CampaignManager>();
        thisShip = FindObjectOfType<ShipStats>();
        jm = FindObjectOfType<JobManager>();
        asm = FindObjectOfType<AdditiveSceneManager>();

        myCanvas.SetActive(false);

        cheatModeActiveText.SetActive(true);
        showingActiveText = true;

        helpMenu.SetActive(false);
        showingHelpMenu = false;

        inMinigameTest = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            ToggleCheatText();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ToggleHelpMenu();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            MiniGameTest();
        }

        if (Input.GetKey(KeyCode.F4)
            && GameManager.instance.currentGameState == InGameStates.JobSelect)
        {
            CycleCampaignJobs();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ToggleDeath();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            ToggleMutiny();
        }
        if(Input.GetKeyDown(KeyCode.F7) 
            && GameManager.instance.currentGameState == InGameStates.Events)
        {
            es.SkipToEvent();
            Debug.Log("Skipping to event");
        }

        if(Input.GetKey(KeyCode.F9))
        {
            ChangeNarrativeVariables();
        }
        else //only allow these if not holding F9 to avoid conflicts
        {
            #region StatMods
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
            {
                ModifyResource(0);
            }
            if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
            {
                ModifyResource(2);
            }
            if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
            {
                ModifyResource(3);
            }
            if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
            {
                ModifyResource(4);
            }
            if (Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5))
            {
                ModifyResource(5);
            }
            if (Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6))
            {
                ModifyResource(7);
            }
            if (Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7))
            {
                ModifyResource(9);
            }
            if (Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8))
            {
                ModifyResource(11);
            }
            #endregion
        }


    }

    private void ToggleCheatText()
    {
        if (showingActiveText)
        {
            cheatModeActiveText.SetActive(false);
            showingActiveText = false;
        }
        else
        {
            cheatModeActiveText.SetActive(true);
            showingActiveText = true;
        }
    }

    private void ToggleHelpMenu()
    {
        if (showingHelpMenu)
        {
            helpMenu.SetActive(false);
            showingHelpMenu = false;
        }
        else
        {
            helpMenu.SetActive(true);
            showingHelpMenu = true;
        }
    }
    
    private void ToggleDeath()
    {
        deathDisabled = !deathDisabled;

        Debug.Log(deathDisabled ? "Death Disabled" : "Death Enabled");
    }
    
    private void ToggleMutiny()
    {
        mutinyDisabled = !mutinyDisabled;

        Debug.Log(mutinyDisabled ? "Mutiny Disabled" : "Mutiny Enabled");
    }

    private void MiniGameTest()
    {
        if (inMinigameTest)
        {
            inMinigameTest = false;
            Debug.Log("Loading ShipBase from MiniGames Testing Menu");
            SceneManager.LoadScene("ShipBase");
            GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
        }
        else
        {
            inMinigameTest = true;
            Debug.Log("Loading MiniGames Testing Menu");
            SceneManager.LoadScene("MiniGames Testing Menu");
        }
    }

    private void CycleCampaignJobs()
    {
        if (campMan)
        {            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                campMan.CycleCampaignsCheat(1);
                jm.RefreshJobList();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                campMan.CycleCampaignsCheat(-1);
                jm.RefreshJobList();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                campMan.CycleJobIndexCheat(1);
                jm.RefreshJobList();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                campMan.CycleJobIndexCheat(-1);
                jm.RefreshJobList();
            }
        }
        else
        {
            Debug.LogError("Campaign manager unassigned in cheats menu");
        }
    }

    public void PlayRandomEvent(int eventNum)
    {

    }
    
    private void ChangeNarrativeVariables()
    {
        #region Alter narrative booleans with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            campMan.AlterNarrativeBoolCheat(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            campMan.AlterNarrativeBoolCheat(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            campMan.AlterNarrativeBoolCheat(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            campMan.AlterNarrativeBoolCheat(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            campMan.AlterNarrativeBoolCheat(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            campMan.AlterNarrativeBoolCheat(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            campMan.AlterNarrativeBoolCheat(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            campMan.AlterNarrativeBoolCheat(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            campMan.AlterNarrativeBoolCheat(9);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            campMan.AlterNarrativeBoolCheat(0);
        }
        #endregion

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            campMan.AlterNarrativeNumbersCheat(1, false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            campMan.AlterNarrativeNumbersCheat(-1, false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            campMan.AlterNarrativeNumbersCheat(-1, true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            campMan.AlterNarrativeNumbersCheat(1, true);
        }
    }

    private void ModifyResource(int resourceID)
    {
        bool canUseSideWaysArrows = resourceID == 0 || resourceID == 5 || resourceID == 7 || resourceID == 9 || resourceID == 11;
        
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(canUseSideWaysArrows)
            {
                resourceID++;
            }
        }
        
        int amount = 0;
        if(Input.GetKey(KeyCode.UpArrow) || (canUseSideWaysArrows && Input.GetKey(KeyCode.RightArrow)))
        {
            amount = 1;
        }
        else if(Input.GetKey(KeyCode.DownArrow) || (canUseSideWaysArrows && Input.GetKey(KeyCode.LeftArrow)))
        {
            amount = -1;
        }
        else
        {
            return;
        }
        
        int morale = MoraleManager.instance.CrewMorale;
        
        switch (resourceID)
        {
            case 0:
                thisShip.Credits += amount;
                break;
            case 1:
                thisShip.Payout += amount;
                break;
            case 2:
                MoraleManager.instance.CrewMorale += amount;
                break;
            case 3:
                thisShip.Security += amount;
                break;
            case 4:
                thisShip.ShipWeapons += amount;
                break;
            case 5:
                thisShip.Food += amount;
                break;
            case 6:
                thisShip.FoodPerTick += amount;
                break;
            case 7:
                thisShip.CrewCurrent += new Vector3(amount, 0, 0);
                MoraleManager.instance.CrewMorale = morale;
                break;
            case 8:
                thisShip.CrewCurrent += new Vector3(0, amount, 0);
                MoraleManager.instance.CrewMorale = morale;
                break;
            case 9:
                thisShip.EnergyRemaining += new Vector2(amount, 0);
                break;
            case 10:
                thisShip.EnergyRemaining += new Vector2(0, amount);
                break;
            case 11:
                thisShip.ShipHealthCurrent += new Vector2(amount, 0);
                break;
            case 12:
                thisShip.ShipHealthCurrent += new Vector2(0, amount);
                break;
            default:
                break;
        }
    }
}
