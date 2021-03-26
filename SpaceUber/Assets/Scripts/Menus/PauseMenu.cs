/* PauseMenu.cs
 * Frank Calabrese
 * 2/1/21
 * Pauses game by setting timescale to 0
 * blocks user interaction by activating a canvas
 * Sort order set in inspector, should probably be highest sort order in game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Singleton<PauseMenu>
{
    public Canvas pauseCanvas;

    private static bool isPaused = false;

    public static bool IsPaused => isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // && SceneManager.GetActiveScene().name != "Menu_Main"
        {
            CheckPaused();
        }
    }

    //if the game is paused, unpause. If it's unpaused, pause.
    //check performed by the activation of the pauseCanvas or lack thereof
    private void CheckPaused()
    {
        if (pauseCanvas.gameObject.activeSelf == true)
        {
            Unpause();
        }
        else if (pauseCanvas.gameObject.activeSelf == false)
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseCanvas.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Unpause()
    {
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1.0f;
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

        Unpause();

        while (!asyncLoad.isDone) yield return null;
    }

    public void CloseOutstandingTutorial()
    {
        Tutorial.Instance.CloseCurrentTutorial(false);
    }
}
