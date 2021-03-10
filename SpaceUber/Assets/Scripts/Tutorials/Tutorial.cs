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
    public bool tutorialFinished; 
}

public class Tutorial : Singleton<Tutorial>
{
    [SerializeField] TutorialNode[] tutorials = new TutorialNode[10];
    [SerializeField] TextMeshProUGUI tutorialTextbox;
    [SerializeField] TextMeshProUGUI tutorialTitleTextbox;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject highlightPanel;

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
        //if you're already in a tutorial, stop.
        if (tutorialPanel.activeSelf == true) return;
        //if the game tries to force a tutorial the player has already seen, stop.
        if (tutorials[tutorialID].tutorialFinished == true && forcedTutorial == true) return;

        currentTutorial = tutorials[tutorialID];

        if (tutorialPanel.activeSelf == false)
        {
            tutorialPanel.SetActive(true);
            tutorialTextbox.text = currentTutorial.tutorialMessages[0];
            tutorialTitleTextbox.text = currentTutorial.tutorialName;
            index = 1;
        }

    }
    
    public void closeCurrentTutorial()
    {
        if (tutorialPanel.activeSelf == true)
        {
            highlightPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            index = 0;

            currentTutorial.tutorialFinished = true;
        }
    }

    //X and Y of center of box, with (0,0) being screen center. Width and height of box
    public void highlightScreenLocation(int xPos, int yPos, int width, int height)
    {
        highlightPanel.SetActive(true);
        RectTransform loc = highlightPanel.GetComponent<RectTransform>();
        loc.sizeDelta = new Vector2(width, height);
        loc.localPosition = new Vector3(xPos, yPos, 1);
    }

}
