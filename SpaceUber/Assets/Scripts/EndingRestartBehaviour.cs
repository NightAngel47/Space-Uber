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
    
    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene("ShipBase");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void ResetStats()
    {
        ship.ResetStats();
    }
}
