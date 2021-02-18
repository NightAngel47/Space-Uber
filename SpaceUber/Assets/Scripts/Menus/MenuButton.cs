/*MenuButton.cs
 * Frank Calabrese 
 * 2/1/21
 * Contains methods used by the buttons of the menus
 * such as loading scenes and opening other panels
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    //load scene specified in the button's inspector window
    //change to that scene once it has loaded successfully
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone) yield return null;
    }


}
