/*
 * TestInkStory.cs
 * Author(s): #Greg Brandt#
 * Created on: 9/17/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TestInkStory : MonoBehaviour
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

    List<string> actionRequirementTags = new List<string>();
    public List<string> currentTags = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file
        //Refresh();
    }

    private void Update()
    {
        currentTags = story.currentTags;
        if (!story.canContinue && story.currentChoices.Count == 0)
        {
            EventSystem.instance.ConcludeEvent();
        }
    }

    public void Continue() 
    {
        if (story.canContinue) { textBox.text = story.Continue(); } else textBox.text =  "Can't Continue"; 
        if(story.currentChoices.Count > 0) { Debug.Log("Choices"); foreach(Choice choice in story.currentChoices) { Debug.Log(choice.text); } }
    }
    public void TakeAction1() { story.ChooseChoiceIndex(0); }
    public void TakeAction2() { story.ChooseChoiceIndex(1); }


    /// <summary>
    /// Instantiates each choice as a button and tells the game which one has been chosen when clicked
    /// </summary>
    void ShowChoices()
    {
        if (story.currentChoices.Count > 0)
        {
            showingChoices = true;
            foreach(Choice choice in story.currentChoices)
            {
                bool actionRequirementsMet = true;
                Debug.Log("Does Requirements contain Choice " + choice.index +1);
                if(actionRequirementTags.Contains("Choice " + choice.index +1))
				{
                    //evaluate requirement Condition
                    int tagIndex = actionRequirementTags.IndexOf("Choice " + choice.index + 1) + 1;
                    string requirement = actionRequirementTags[tagIndex];
                    while (tagIndex < actionRequirementTags.Count && actionRequirementTags[tagIndex] != "Choice " + choice.index + 2) 
                     {
                        requirement += " " + actionRequirementTags[tagIndex];
                        tagIndex++;
			         }
                    Debug.Log("Requirement: ");
                    Debug.Log(requirement);
				}
                if (actionRequirementsMet)
                {
                    //instantiate a button 
                    Button choiceButton = Instantiate(buttonPrefab) as Button;
                    choiceButton.transform.SetParent(this.transform, false);

                    // Gets the text from the button prefab
                    Text choiceText = choiceButton.GetComponentInChildren<Text>();
                    choiceText.text = choice.text;
                    // Set listener
                    choiceButton.onClick.AddListener(delegate { OnClickChoiceButton(choice); });
                    //The delegate keyword is used to pass a method as a parameter to the AddListenerer() function.
                    //Whenever a button is clicked, the function onClickChoiceButton() function is used.
                }
            }
        }
    }

    /// <summary>
    /// Refreshes the UI elements connected to this object whenever a button is clicked to change it
    /// Clears any current elements and starts the function to print out the next story chunk
    /// </summary>
    void Refresh()
    {
        ClearUI();

        textBox.text = GetNextStoryBlock();

        List<string> tags = story.currentTags;

		if (tags.Count > 0)
		{
            string debug = "tags:";
            foreach(string tag in tags) { debug += " " + tag; }
            Debug.Log(debug);
        }
        ShowChoices();
    }

    /// <summary>
    /// When one of the buttons gets clicked, supply that given choice to the system.
    /// </summary>
    /// <param name="choice"></param>
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        Refresh();
        if (story.currentChoices.Count > 0) { story.ChooseChoiceIndex(0); }
        
        
        Refresh(); Refresh();
        actionRequirementTags = story.currentTags;
        if (actionRequirementTags.Count > 0)
        {
            string debug = "Action reqs";
            foreach (string tag in actionRequirementTags) { debug += " " + tag; }
            Debug.Log(debug);
        }
        showingChoices = false;
    }


    /// <summary>
    /// Returns a string of the next line of text, if there is story left
    /// </summary>
    string GetNextStoryBlock()
    {
        string text = "This should not be printing :D"; //error check

        if (story.canContinue) //ALWAYS do this check before using story.Continue() to avoid errors
        {
            text = story.Continue();  //reads text until there is another choice 
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
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

}
