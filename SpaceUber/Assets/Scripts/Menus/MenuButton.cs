/*MenuButton.cs
 * Frank Calabrese 
 * 2/1/21
 * Contains methods used by the buttons of the menus
 * such as starting and quitting the game
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject menuButtonsWithContinue;

    private void Awake()
    {
        menuButtons.SetActive(false);
        menuButtonsWithContinue.SetActive(false);
    }

    private IEnumerator Start()
    {
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);

        yield return new WaitUntil(() => FindObjectOfType<SavingLoadingManager>());
        if (SavingLoadingManager.instance.GetHasSave())
        {
            menuButtons.SetActive(false);
            menuButtonsWithContinue.SetActive(true);
        }
        else
        {
            menuButtons.SetActive(true);
            menuButtonsWithContinue.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void NewGame()
    {
        if (SavingLoadingManager.instance.GetHasSave())
        {
            SavingLoadingManager.instance.SetHasSaveFalse();
            StartGame();
        }
        else
        {
            StartGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
