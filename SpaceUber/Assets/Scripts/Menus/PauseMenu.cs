/* PauseMenu.cs
 * Frank Calabrese 
 * 2/1/21
 * Pauses game by setting timescale to 0
 * blocks user interaction by activating a canvas
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseCanvas;

    //private bool isPaused = false;

    void Start()
    {
        //pauseCanvas = gameObject.GetComponent<Canvas>();
    }

    
    void Update()
    {
        if(Input.GetKeyDown("tab"))
        {
            CheckPaused();
        }
    }

    private void CheckPaused()
    {
        if (pauseCanvas.gameObject.activeSelf == true)
        {
            pauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else if (pauseCanvas.gameObject.activeSelf == false)
        {
            pauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
