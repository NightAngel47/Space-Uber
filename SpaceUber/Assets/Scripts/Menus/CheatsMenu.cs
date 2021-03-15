/*
 * CheatsMenu.cs
 * Author(s): 
 * Created on: 3/8/2021 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class CheatsMenu : MonoBehaviour
{
    public static CheatsMenu instance;
    private EventSystem es;
    [SerializeField]private CampaignManager campMan;
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

        es = GameObject.FindObjectOfType<EventSystem>();
        campMan = GameObject.FindObjectOfType<CampaignManager>();
        thisShip = GameObject.FindObjectOfType<ShipStats>();
        jm = GameObject.FindObjectOfType<JobManager>();
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

        #region StatMods
        if (Input.GetKey("1"))
        {
            ModifyResource(0);
        }
        if (Input.GetKey("2"))
        {
            ModifyResource(2);
        }
        if (Input.GetKey("3"))
        {
            ModifyResource(3);
        }
        if (Input.GetKey("4"))
        {
            ModifyResource(4);
        }
        if (Input.GetKey("5"))
        {
            ModifyResource(5);
        }
        if (Input.GetKey("6"))
        {
            ModifyResource(7);
        }
        if (Input.GetKey("7"))
        {
            ModifyResource(9);
        }
        if (Input.GetKey("8"))
        {
            ModifyResource(11);
        }
        #endregion
        
    }

    public void ActivateCanvas()
    {
        myCanvas.SetActive(true);
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
        
        if(deathDisabled)
        {
            Debug.Log("Death Disabled");
        }
        else
        {
            Debug.Log("Death Enabled");
        }
    }
    
    private void ToggleMutiny()
    {
        mutinyDisabled = !mutinyDisabled;
        
        if(mutinyDisabled)
        {
            Debug.Log("Mutiny Disabled");
        }
        else
        {
            Debug.Log("Mutiny Enabled");
        }
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

    public void CycleCampaignJobs()
    {
        if (campMan)
        {            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                campMan.CycleCampaigns(1);
                jm.RefreshJobList();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                campMan.CycleCampaigns(-1);
                jm.RefreshJobList();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                campMan.CycleJobIndex(1);
                jm.RefreshJobList();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                campMan.CycleJobIndex(-1);
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
