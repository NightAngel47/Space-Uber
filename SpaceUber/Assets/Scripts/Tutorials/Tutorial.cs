/* Frank Calabrese
 * 3/6/21
 * Tutorial.cs
 * Singleton class responsible for holding and tracking all tutorials. 
 * Will display specified tutorial when setCurrentTutorial(tutorial ID) is called
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialNode
{
    public string tutorialName;
    public string[] tutorialMessages;
    public bool tutorialFinished; //change to private later
}

public class Tutorial : Singleton<Tutorial>
{
    [SerializeField] TutorialNode[] tutorials = new TutorialNode[10];
    [SerializeField] TextMeshProUGUI tutorialTextbox;
    [SerializeField] GameObject tutorialPanel;

    private TutorialNode currentTutorial;
    private int index;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && tutorialPanel.activeSelf == true)
        {
            if (index < currentTutorial.tutorialMessages.Length)
            {
                tutorialTextbox.text = currentTutorial.tutorialMessages[index];
                index++;
            }
            else closeCurrentTutorial();
        }
    }


    //call this to display a tutorial. Tutorial IDs can be found in the inspector
    public void setCurrentTutorial(int tutorialID, bool forcedTutorial)
    {
        //if the game tries to force a tutorial the player has already seen, stop.
        if (tutorials[tutorialID].tutorialFinished == true && forcedTutorial == true) return;

        currentTutorial = tutorials[tutorialID];

        if (tutorialPanel.activeSelf == false)
        {
            tutorialPanel.SetActive(true);
            tutorialTextbox.text = currentTutorial.tutorialMessages[0];
            index = 1;
        }

    }
    
    public void closeCurrentTutorial()
    {
        if (tutorialPanel.activeSelf == true)
        {
            tutorialPanel.SetActive(false);
            index = 0;

            currentTutorial.tutorialFinished = true;
        }
    }

    //top left corner x bottom right corner of highlight
    public void highlight()
    {

    }

}
