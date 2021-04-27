/*
 * EndingRestartBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 10/10/2020 (en-US)
 * Description: 
 */

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingRestartBehaviour : MonoBehaviour
{
    //TODO: provide an option to go back to ship building upon death

    public TextMeshProUGUI causeOfDeathText;
    public string mutinyText;
    public string hullDeathText;

    public void Start()
    {
        if(GameManager.instance.currentGameState == InGameStates.Death)
        {
            causeOfDeathText.text = hullDeathText;
        }

        if (GameManager.instance.currentGameState == InGameStates.Mutiny)
        {
            causeOfDeathText.text = mutinyText;
        }
    }

    public void RestartGame() // TODO: change to different state vs wiping save (probably need to check with design)
    {
        SavingLoadingManager.instance.SetHasSaveFalse();
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void ResetJob()
    {
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);
        GameManager.instance.ChangeInGameState(InGameStates.ShipBuilding);
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void GoToMainMenu()
    {
        SavingLoadingManager.instance.SetHasSaveFalse();
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);
        SceneManager.LoadScene("Menu_Main");
    }
    
    public void GoToCredits()
    {
        GameManager.instance.ChangeInGameState(InGameStates.EndingCredits);
    }
}
