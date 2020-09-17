/*
 * InkExample.cs
 * Author(s): Scott Acker
 * Created on: 9/11/2020
 * Description: An example file to show the most-common functions of Ink-related Code
 * Runs a simple path of choices by creating clickable UI buttons. 
 */

using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class InkExample : MonoBehaviour
{
    [Tooltip("Attach the.JSON file you want read to this")]
    public TextAsset inkJSONAsset;
    
    /// <summary>
    /// The story itself being read
    /// </summary>
    private Story story;

    [Tooltip("Attach the prefab of a choice button to this")]
    /// <summary>
    /// A prefab of the button we will generate every time a choice is needed
    /// </summary>
    public Button buttonPrefab;

    [Tooltip("Attach the connected TextBox to this")]
    public Text textBox;
    [Tooltip("How many characters you will allow to fit in one text box")]
    public int textBoxMaxChar = 70;
    [Tooltip("How fast text will scroll")]
    public float textPrintSpeed = 0.07f;

    /// <summary>
    /// Whether the latest bit of text is done printing so it can show the choices
    /// </summary>
    public bool donePrinting = true;
    public bool showingChoices = false;


    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file
        Refresh();
    }

    private void Update()
    {
        //Save for potential implementation of story.Continue() instead of continueMaximally()
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && donePrinting && !showingChoices)
        {
            Refresh();
        }
    }

    /// <summary>
    /// Prints text into the textbox character by character
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private IEnumerator PrintText(string text)
    {
        donePrinting = false;

        string tempString = "";
        textBox.text = tempString;
        int runningIndx = 0;

        while (tempString.Length < text.Length)
        {
            //This causes a crash. Beware
            ////if the length is getting too close to the text box size and the current char is a space or line
            //if((tempString.Length > textBoxMaxChar - 10) 
            //    && (text[runningIndx] == ' ' || text[runningIndx] == '\n'))
            //{
            //    //wait for player to click or press space to continue
            //    bool wait = true;
            //    while(wait)
            //    {
            //        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            //        {
            //            wait = false;
            //            tempString = "";
            //        }
            //    }

            //}
            tempString += text[runningIndx];
            runningIndx++;
            textBox.text = tempString;
            yield return new WaitForSeconds(textPrintSpeed);
        }

        donePrinting = true;
        ShowChoices();
    }

    /// <summary>
    /// Instantiates each choice as a button and tells the game which one has been chosen when clicked
    /// </summary>
    void ShowChoices()
    {
        if(story.currentChoices.Count > 0)
        {
            showingChoices = true;
            foreach (Choice choice in story.currentChoices)
            {
                //instantiate a button 
                Button choiceButton = Instantiate(buttonPrefab) as Button;
                choiceButton.transform.SetParent(this.transform, false);

                // Gets the text from the button prefab
                Text choiceText = choiceButton.GetComponentInChildren<Text>();
                choiceText.text = " " + (choice.index + 1) + ". " + choice.text;

                // Set listener
                choiceButton.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
                //The delegate keyword is used to pass a method as a parameter to the AddListenerer() function.
                //Whenever a button is clicked, the function onClickChoiceButton() function is used.
            }
        }

        
    }

    /// <summary>
    /// Refreshes the UI elements connected to this object whenever a button is clicked to change it
    /// Clears any current elements and starts the function to print out the next story chunk
    /// </summary>
    void Refresh()
    {
        // Clear the UI
        ClearUI();

        // Set the text from new story block
        string text = GetNextStoryBlock();
        StartCoroutine(PrintText(text));
        

        //// Get the tags from the current story lines (if any)
        //List<string> tags = story.currentTags;

        //// If there are tags for character names specifically, use the first one in front of the text. 
        ////Otherwise, just show the text.
        //if (tags.Count > 0)
        //{
        //    textBox.text = tags[0] + ": " + text;
        //}
        //else
        //{
        //    textBox.text = text;
        //}

        
    }

    /// <summary>
    /// When one of the buttons gets clicked, supply that given choice to the system.
    /// </summary>
    /// <param name="choice"></param>
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        Refresh();
        showingChoices = false;
    }

    /// <summary>
    /// Returns a string of the next line of text, if there is story left
    /// </summary>
    string GetNextStoryBlock()
    {
        string text = "This should not be printing :D";

        if (story.canContinue) //ALWAYS do this check before using story.Continue() to avoid errors
        {
            text = story.Continue();  //reads text until there is another choice 
            print("The next block is: " + text);
            
        }
        
        return text;
    }

    // Clear out all of the UI, calling Destory() in reverse
    // Currently causes a stackoverflow error
    void ClearUI()
    {
        int childCount = this.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}
