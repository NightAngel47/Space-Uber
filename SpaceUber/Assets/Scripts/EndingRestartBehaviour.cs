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
        Destroy(FindObjectOfType<SpotChecker>().gameObject);
        SceneManager.LoadScene("ShipBase");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void ResetJob()
    {
        ship.ResetStats();
        campaignManager.cateringToTheRich.ResetEventChoicesToJobStart();
        EventSystem.instance.ResetJob();
    }
}
