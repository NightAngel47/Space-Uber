/*
 * PromptScreenBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 9/20/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class PromptScreenBehaviour : MonoBehaviour
{
    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        GameManager.instance.LoadScene("SampleJob");
    }
}
