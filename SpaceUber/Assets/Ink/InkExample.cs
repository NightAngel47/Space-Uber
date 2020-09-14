/*
 * InkExample.cs
 * Author(s): Scott Acker
 * Created on: 9/11/2020
 * Description: An example file to show the most-common functions of Ink-related Code
 * Runs a simple path of choices by creating clickable UI buttons. this object must have
 */

using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class InkExample : MonoBehaviour
{
    [Tooltip("Attach the associated .JSON file to this")]
    public TextAsset inkJSONAsset;
    
    /// <summary>
    /// The story itself being read
    /// </summary>
    private Story story;

    /// <summary>
    /// A prefab of the button we will generate every time a choice is needed
    /// </summary>
    public Button buttonPrefab;

    public Text textBox;

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSONAsset.text); //this draws the text out of the JSON file
        Refresh();

        foreach (Choice choice in story.currentChoices) //Access the choices available at this specific story block
        {
            Debug.Log("Choice [ " + choice.index + "]: '" + choice.text + "'");
        }
    }

    /// <summary>
    /// Refreshes the UI elements connected to this object whenever a button is clicked to change it
    // - Clears any current elements
    // - Shows any text chunks
    // - Iterate through any choices and create buttons for them
    /// </summary>
    void Refresh()
    {
        // Clear the UI
        ClearUI();

        // Set the text from new story block
        string text = GetNextStoryBlock();

        // Get the tags from the current story lines (if any)
        List<string> tags = story.currentTags;

        // If there are tags, use the first one.
        // Otherwise, just show the text.
        if (tags.Count > 0)
        {
            textBox.text = tags[0] + ": " + text;
        }
        else
        {
            textBox.text = text;
        }

        //instantiates each choice as a button and tells the game which one has been chosen when clicked
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

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        Refresh();
    }

    /// <summary>
    /// Returns a string of the next line of text, if there is story left
    /// </summary>
    string GetNextStoryBlock()
    {
        string text = "";

        if (story.canContinue) //ALWAYS do this check before using story.Continue() to avoid errors
        {
            text = story.ContinueMaximally();  //reads text until there is another choice 
        }

        return text;
    }

    // Clear out all of the UI, calling Destory() in reverse
    void ClearUI()
    {
        int childCount = this.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}
