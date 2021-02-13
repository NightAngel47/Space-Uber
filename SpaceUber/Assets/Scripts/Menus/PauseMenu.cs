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

public class PauseMenu : Singleton<PauseMenu>
{
    public Canvas pauseCanvas;

    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown("tab"))
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
            pauseCanvas.gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        else if (pauseCanvas.gameObject.activeSelf == false)
        {
            pauseCanvas.gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }

    }
}