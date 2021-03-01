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
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject menuButtonsWithContinue;
    
    private void Start()
    {
        if(FindObjectOfType<SpotChecker>()) Destroy(FindObjectOfType<SpotChecker>().gameObject);

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
            //TODO: ask if they want to delete.
        }
    }

    public void DeleteSave()
    {
        SavingLoadingManager.instance.DeleteSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
