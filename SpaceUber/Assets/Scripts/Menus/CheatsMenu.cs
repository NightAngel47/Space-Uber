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
    private CampaignManager campMan;
    private ShipStats thisShip;
    private JobManager jm;

    private AdditiveSceneManager asm;

    [SerializeField] private GameObject myCanvas;

    [SerializeField] private GameObject cheatModeActiveText;
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
    void Start()
    {
        //Singleton pattern
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            asm = FindObjectOfType<AdditiveSceneManager>();
        }

        es = gameObject.GetComponent<EventSystem>();
        campMan = gameObject.GetComponent<CampaignManager>();
        thisShip = GameObject.FindObjectOfType<ShipStats>();
        jm = GameObject.FindObjectOfType<JobManager>();
        
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
        if(Input.GetKeyDown(KeyCode.Tilde))
        {
            ToggleCheatText();
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            ToggleHelpMenu();
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            MiniGameTest();
        }
        if(Input.GetKeyDown(KeyCode.F5))
        {
            ToggleDeath();
        }
        if(Input.GetKeyDown(KeyCode.F6))
        {
            ToggleMutiny();
        }
    }

    private void ToggleCheatText()
    {
        print("Toggling thing");
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
            Debug.LogWarning("Loading ShipBase from MiniGames Testing Menu");
            SceneManager.LoadScene("ShipBase");
            GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
        }
        else
        {
            inMinigameTest = true;
            Debug.LogWarning("Loading MiniGames Testing Menu");
            SceneManager.LoadScene("MiniGames Testing Menu");
        }
    }

    public void JumpToCampaign(int campNum)
    {

    }

    public void PlayRandomEvent(int eventNum)
    {

    }    
}
