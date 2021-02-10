/* MainMenuButton.cs
 * Frank Calabrese 
 * 2/1/21
 * Contains methods used by the buttons of the main menu
 * such as loading scenes and opening other panels
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string goToSceneName;
    
    public void GoToScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    //load scene specified in the button's inspector window
    //change to that scene once it has loaded successfully
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(goToSceneName);

        while (!asyncLoad.isDone) yield return null;
    }


}
