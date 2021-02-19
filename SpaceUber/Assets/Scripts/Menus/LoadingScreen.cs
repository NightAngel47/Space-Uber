/*MenuButton.cs
 * Frank Calabrese 
 * 2/19/21
 * Asynchronously loads a scene specified in the inspector
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] string toScene;
    
    void Start()
    {
        loadScene(toScene);
    }

    public void loadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    //load scene in background, switch to that scene when it's ready
    IEnumerator LoadSceneAsync(string sceneName)
    {
        //wait for one second because currently the load time is very fast,
        //and it looks jarring otherwise
        yield return new WaitForSeconds(1.0f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone) yield return null;

    }
}
