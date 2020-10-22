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

    private bool inTest = false;
    
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
        }
    }

    void Update()
    {
        #if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (inTest)
            {
                inTest = false;
                SceneManager.LoadScene("ShipBase");
                GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
            }
            else
            {
                inTest = true;
                SceneManager.LoadScene("MiniGames Testing Menu");
            }
        }
    }
}
