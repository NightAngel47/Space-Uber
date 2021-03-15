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
    [HideInInspector] public bool cheatModeActive;
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
        if(Input.GetKeyDown(KeyCode.F1))
        {
            OpenCloseCheats();
        }

        // TODO: add back speed up via cheats (later)
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
        if (cheatModeActive)
        {
            cheatModeActive = false;
            Destroy(FindObjectOfType<CheatsMenu>().gameObject);
            asm.UnloadScene("Cheats");
        }
        else
        {
            cheatModeActive = true;
            asm.LoadSceneMerged("Cheats");
        }
    }
}
