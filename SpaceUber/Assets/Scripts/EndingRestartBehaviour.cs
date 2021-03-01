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
    //TODO: provide an option to go back to ship building upon death
    
    public void RestartGame() // TODO: change to different state vs wiping save (probably need to check with design)
    {
        SavingLoadingManager.DeleteSave();
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
    
    public void ResetJob()
    {
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.instance.PlayMusicWithTransition("General Theme");
    }
}
