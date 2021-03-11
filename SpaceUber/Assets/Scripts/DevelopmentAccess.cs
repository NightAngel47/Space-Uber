/*
 * DevelopmentAccess.cs
 * Author(s): Steven Drovie []
 * Created on: 10/21/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class DevelopmentAccess : MonoBehaviour
{
    public static DevelopmentAccess instance;
    private bool cheatModeActive;

    private bool inTest = false;
    private AdditiveSceneManager asm;

    private void Awake()
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
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    if (inTest)
        //    {
        //        inTest = false;
        //        Debug.LogWarning("Loading ShipBase from MiniGames Testing Menu");
        //        SceneManager.LoadScene("ShipBase");
        //        GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
        //    }
        //    else
        //    {
        //        inTest = true;
        //        Debug.LogWarning("Loading MiniGames Testing Menu");
        //        SceneManager.LoadScene("MiniGames Testing Menu");
        //    }
        //}

        if(Input.GetKeyDown(KeyCode.F1))
        {
            OpenCloseCheats();
        }

        //#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    Debug.LogWarning("Time scale double speed");
        //    Time.timeScale = 2;
        //}
        
        //if (Input.GetKeyUp(KeyCode.F1))
        //{
        //    Debug.LogWarning("Time scale normal speed");
        //    Time.timeScale = 1;
        //}
        //#endif
    }

    void OpenCloseCheats()
    {
        //TODO: load cheat scene
        if (cheatModeActive)
        {
            cheatModeActive = false;
            asm.UnloadScene("Cheats");

        }
        else
        {
            cheatModeActive = true;
            asm.LoadSceneMerged("Cheats");
        }
    }
}
