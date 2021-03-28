/*
 * InkDriverBase.cs
 * Author(s): Scott Acker
 * Created on: 9/11/2020
 * Description: An example file to show the most-common functions of Ink-related Code
 * Runs a simple path of choices by creating clickable UI buttons.
 */

using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using TMPro;

public class InkDriverBase : MonoBehaviour
{
    [Tooltip("Attach the.JSON file you want read to this")]
    public TextAsset inkJSONAsset;

    [SerializeField] private string eventName;
    public bool isStoryEvent;
    [HideInInspector] public bool isCharacterEvent;
    public bool isMutinyEvent;

    [Tooltip("Whether or not this event scales its stat outcomes with the campaign"),HideInInspector]
    public bool isScalableEvent;

    [ShowIf("isStoryEvent")] public int storyIndex;
    [SerializeField] private bool hasAnimatedBG = false;
    [SerializeField, HideIf("hasAnimatedBG")] private Sprite backgroundImage;
    [SerializeField, ShowIf("hasAnimatedBG")] private GameObject backgroundAnimation;
    public string EventName => eventName; 

    //A prefab of the button we will generate every time a choice is needed
    [SerializeField, Tooltip("Attach the prefab of a choice button to this")]
    private Button buttonPrefab;

    [HideInInspector] public CampaignManager campMan;
    private Transform buttonGroup;
    private TMP_Text titleBox;
    private TMP_Text textBox;
    [HideInInspector] public GameObject resultsBox;
    private Image backgroundUI;
    protected ShipStats thisShip;

    [SerializeField, Tooltip("Controls how fast text will scroll. It's the seconds of delay between words, so less is faster.")]
    private float textPrintSpeed = 0.0001f;
    public int pageNumber;
    
    public bool isAtPageLimit;
    private string storyBlock;
    private int prevCharIndex;
    private int nextCharIndex;
    private int textBoxLineHeight = 9;

    public string eventIntroSFX;

    [Dropdown("eventMusicTracks")]
    public string eventBGM;

    private List<string> eventMusicTracks
    {
        get
        {
            return new List<string>() { "", "General Theme", "Wormhole", "Engine Malfunction", "Engine Delivery", "Black Market", "Clone Ambush Intro", "Safari Tampering", "Clone Ambush Negotiation", "Clone Ambush Fight", "Ejection", "Asteroid Mining", "Blockade", "Crop Blight", "Door Malfunction", "Drug Overdose", "Escaped Convicts", "Septic Malfunction", "Soothing Light", "Spatial Aurora", "Food Poisoning", "Hostage Situation", "Hull Maintenance" };
        }
    }

    [SerializeField] public List<Requirements> requiredStats = new List<Requirements>();

    [SerializeField, Tooltip("The first set of choices that a player will reach.")]
    public List<EventChoice> nextChoices = new List<EventChoice>();

    [SerializeField] bool hasSubsequentChoices;
    [ShowIf("hasSubsequentChoices"), Tooltip("Sets of subsequent choices that can be accessed by index by an event choice.")]
    public List<SubsequentChoices> subsequentChoices = new List<SubsequentChoices>();


    /// <summary>
    /// The story itself being read
    /// </summary>
    protected Story story;

    /// <summary>
    /// Whether the latest bit of text is done printing so it can show the choices
    /// </summary>
    public bool donePrinting = true;
    private bool showingChoices = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isAtPageLimit = true;
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file

        Refresh(); //starts the dialogue
        titleBox.text = eventName;
        backgroundUI.sprite = backgroundImage;
        if (hasAnimatedBG)
        {
            Instantiate(backgroundAnimation, backgroundUI.transform.parent);
            backgroundUI.enabled = false;
        }
        AudioManager.instance.PlayMusicWithTransition(eventBGM);
        AudioManager.instance.PlaySFX(eventIntroSFX);

        isScalableEvent = !isStoryEvent && !isMutinyEvent;
    }

    /// <summary>
    /// Assigns necessary UI elements
    /// </summary>
    /// <param name="title"></param>
    /// <param name="text"></param>
    /// <param name="results"></param>
    /// <param name="background"></param>
    /// <param name="buttonSpace"></param>
    /// <param name="ship"></param>
    /// <param name="campaignManager"></param>
    public void AssignStatusFromEventSystem(TMP_Text title, TMP_Text text, GameObject results, Image background, Transform buttonSpace,
        ShipStats ship, CampaignManager campaignManager)
    {
        titleBox = title;
        textBox = text;
        resultsBox = results;
        backgroundUI = background;
        buttonGroup = buttonSpace;
        thisShip = ship;
        campMan = campaignManager;

        resultsBox.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        resultsBox.SetActive(false);
    }

    public void ConcludeEvent()
    {
        if (!showingChoices && donePrinting)
        {
            if (!story.canContinue && story.currentChoices.Count == 0)
            {
                EventSystem.instance.ConcludeEvent();
            }
        }
    }

    /// <summary>
    /// Prints text into the textbox character by character
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrintText(bool previousPage = false)
    {
        donePrinting = false;

        string tempString = "";
        textBox.text = tempString;
        isAtPageLimit = false;

        // if going backwards to previous page
        if (previousPage)
        {
            nextCharIndex = prevCharIndex; // save char index
        }
        
        prevCharIndex = nextCharIndex; // save char index

        yield return new WaitForEndOfFrame();
        
        // print text until no more text or at page limit
        while (nextCharIndex < storyBlock.Length && !isAtPageLimit)
        {
            // print next character
            tempString += CheckChar(storyBlock[nextCharIndex]);
            textBox.text = tempString;
            nextCharIndex++;

            // check if go to next page
            if (textBox.textInfo.lineCount >= textBoxLineHeight)
            {
                isAtPageLimit = true;
            }

            // check if go to choices / conclude
            if (nextCharIndex >= storyBlock.Length)
            {
                isAtPageLimit = true;
                ShowChoices();
                if(!showingChoices)
                {
                    donePrinting = true;
                }
            }

            //click to instantly finish text
            // if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || 
            //     Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            //     Input.GetMouseButtonDown(0))
            // {
            //     tempString = storyBlock;
            //     isAtPageLimit = false;
            //     // TODO: need to save nextCharIndex for end of page
            // }

            yield return new WaitForSeconds(textPrintSpeed);
        }

        //donePrinting = true;
    }

    private char CheckChar(char nextChar)
    {
        if (nextChar == '�')
        {
            nextChar = '\'';
        }
        if(nextChar == '�' || nextChar == '�')
        {
            nextChar = '\"';
        }
        return nextChar;
    }

    /// <summary>
    /// Instantiates each choice as a button and tells the game which one has been chosen when clicked
    /// </summary>
    public void ShowChoices()
    {
        if (showingChoices || story.currentChoices.Count <= 0) return; // || !donePrinting

        showingChoices = true;
        foreach (Choice choice in story.currentChoices)
        {
            //instantiate a button
            Button choiceButton = Instantiate(buttonPrefab, buttonGroup);

            // Gets the text from the button prefab
            TMP_Text choiceText = choiceButton.GetComponentInChildren<TMP_Text>();
            choiceText.text = " " + (choice.index + 1) + ". " + choice.text;

            // Set listener for the sake of knowing when to refresh
            choiceButton.onClick.AddListener(delegate {
                OnClickChoiceButton(choice);
            });

            if (choice.index < nextChoices.Count)
            {
                nextChoices[choice.index].CreateChoice(thisShip,choiceButton, story,this, choiceButton.transform.GetChild(1).GetComponent<OutcomeTooltipUI>());

                // Have on click also call the outcome choice to update the ship stats
                choiceButton.onClick.AddListener(delegate {
                    nextChoices[choice.index].SelectChoice(thisShip);
                });
            }
            else
            {
                Debug.LogWarning($"There was not EventChoice available for choice index: {choice.index}");
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
        GetNextStoryBlock();
        StartCoroutine(PrintText());
    }

    /// <summary>
    /// When one of the buttons gets clicked, supply that given choice to the system.
    /// </summary>
    /// <param name="choice"></param>
    private void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        Refresh();
        showingChoices = false;
        FindObjectOfType<PageController>().UpdateMadeChoice();
    }

    /// <summary>
    /// Takes any subsequent choices from the EventChoice selected and applies them to the file
    /// </summary>
    /// <param name="subsequent"></param>
    public void TakeSubsequentChoices(List<EventChoice> subsequent)
    {
        nextChoices = subsequent;
    }

    /// <summary>
    /// Sets the next block of the story
    /// </summary>
    void GetNextStoryBlock()
    {
        storyBlock = String.Empty;
        
        //Allows the story to add different paragraphs up until the next choice
        while (story.canContinue)
        {
            storyBlock += story.Continue() + "\n";
        }
    }

    // Clear out all of the UI, calling Destory() in reverse
    // Currently causes a stackoverflow error
    public void ClearUI()
    {
        if (buttonGroup != null)
        {
            foreach (var button in buttonGroup.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        foreach (var text in transform.GetComponentsInChildren<TMP_Text>())
        {
            Destroy(text.gameObject);
        }
        FindObjectOfType<PageController>().ResetPages();
    }

    IEnumerator Test(string text)
    {
        //textBox.text = text;
        PageController pageController = FindObjectOfType<PageController>();
        // Force and update of the mesh to get valid information.
        textBox.ForceMeshUpdate();

        var totalVisibleCharacters = textBox.textInfo.characterCount; // Get # of Visible Character in text object
        var counter = 0;
        var visibleCount = 0;

        while (true)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            textBox.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
            
            //if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow)))
            //{
            //    textBox.textInfo.pageInfo[pageNumber].lastCharacterIndex = textBox.maxVisibleCharacters;
            //}

            if (textBox.textInfo.pageInfo[pageNumber].lastCharacterIndex >= visibleCount)
            {
                isAtPageLimit = true;
                counter += 1;
            }
            else
            {
                isAtPageLimit = false;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
