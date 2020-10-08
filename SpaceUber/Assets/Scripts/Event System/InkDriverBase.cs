/*
 * InkDriverBase.cs
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
using NaughtyAttributes;
using TMPro;

public class InkDriverBase : MonoBehaviour
{
    [SerializeField, Tooltip("Attach the.JSON file you want read to this")]
    private TextAsset inkJSONAsset;

    [SerializeField] private string eventName;
    [SerializeField] private Sprite backgroundImage;
    
    //A prefab of the button we will generate every time a choice is needed
    [SerializeField, Tooltip("Attach the prefab of a choice button to this")] 
    private Button buttonPrefab;

    [SerializeField, Tooltip("The transform parent to spawn choices under")]
    [HideInInspector]public Transform buttonGroup;
    
    [HideInInspector] public TMP_Text titleBox;
    [HideInInspector] public TMP_Text textBox;
    [HideInInspector] public Image backgroundUI;

    [SerializeField, Tooltip("Controls how fast text will scroll. It's the seconds of delay between words, so less is faster.")]
    private float textPrintSpeed = 0.1f;

    [SerializeField, Tooltip("The list of choice outcomes for this event.")] 
    private List<ChoiceOutcomes> choiceOutcomes = new List<ChoiceOutcomes>();

    /// <summary>
    /// The story itself being read
    /// </summary>
    private Story story;

    /// <summary>
    /// Whether the latest bit of text is done printing so it can show the choices
    /// </summary>
    public bool donePrinting = true;
    public bool showingChoices = false;
    private bool canEnd = false; //if the event can end. Based on player clicking at end of interaction

    
    [Dropdown("eventMusicTracks")]
    public string eventBGM;
    private List<string> eventMusicTracks
    {
        get
        {
            return new List<string>()
        { "", "General Theme", "Wormhole", "Engine Malfunction", "Engine Delivery"};
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file

        Refresh(); //starts the dialogue
        titleBox.text = eventName;
        backgroundUI.sprite = backgroundImage;
        AudioManager.instance.PlayMusicWithTransition(eventBGM);


    }

    private void Update()
    {
        //Save for potential implementation of story.Continue() instead of continueMaximally()
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !showingChoices && donePrinting)
        {
            Refresh();
            if(!story.canContinue && story.currentChoices.Count == 0)
            {
                EventSystem.instance.ConcludeEvent();
            }
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
        int runningIndex = 0;

        while (tempString.Length < text.Length)
        {
            tempString += text[runningIndex];
            runningIndex++;

            //click to instantly finish text,
            if(Input.GetKeyDown(KeyCode.Return))
            {
                tempString = text;
            }
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
                Button choiceButton = Instantiate(buttonPrefab, buttonGroup);

                // Gets the text from the button prefab
                TMP_Text choiceText = choiceButton.GetComponentInChildren<TMP_Text>();
                choiceText.text = " " + (choice.index + 1) + ". " + choice.text;

                // Set listener
                choiceButton.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
                //The delegate keyword is used to pass a method as a parameter to the AddListenerer() function.
                //Whenever a button is clicked, the function onClickChoiceButton() function is used.
                
                // Have on click also call the outcome choice to update the ship stats
                choiceButton.onClick.AddListener(delegate {
                    choiceOutcomes[choice.index].ChoiceChange();
                });
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
        string text = ""; //error check

        if (story.canContinue) //ALWAYS do this check before using story.Continue() to avoid errors
        {
            text = story.Continue();  //reads text until there is another choice
        }
        return text;
    }

    // Clear out all of the UI, calling Destory() in reverse
    // Currently causes a stackoverflow error
    public void ClearUI()
    {
        foreach (var button in buttonGroup.transform.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }

        foreach (var text in transform.GetComponentsInChildren<TMP_Text>())
        {
            Destroy(text.gameObject);
        }
    }
}
