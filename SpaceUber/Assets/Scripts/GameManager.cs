/*
 * GameManager.cs
 * Author(s): Steven Drovie
 * Created on: 9/19/2020 (en-US)
 * Description: Manages the game states. Loads scenes.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private void Awake()
    {
        //Singleton pattern
        if(instance) { Destroy(gameObject); }
        else { instance = this; }
        
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// Loads the passed in scene name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
