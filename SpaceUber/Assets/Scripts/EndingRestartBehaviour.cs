/*
 * EndingRestartBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 10/10/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingRestartBehaviour : MonoBehaviour
{
    private ShipStats ship;
    private CampaignManager campaignManager;

    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
        campaignManager = FindObjectOfType<CampaignManager>();
    }
    
    public void RestartGame()
    {
        SavingLoadingManager.DeleteSave();
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void ResetJob()
    {
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
        
        /*
        ship.ResetStats();
        campaignManager.cateringToTheRich.ResetEventChoicesToJobStart();
        EventSystem.instance.ResetJob();
        */
    }
}
