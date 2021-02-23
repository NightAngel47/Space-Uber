/*MenuButton.cs
 * Frank Calabrese 
 * 2/1/21
 * Contains methods used by the buttons of the menus
 * such as starting and quitting the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    public void quitGame()
    {
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }


}
