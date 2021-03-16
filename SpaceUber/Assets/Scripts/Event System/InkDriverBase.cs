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
    [Tooltip("Attach the.JSON file you want read to this")]
    public TextAsset inkJSONAsset;

    [SerializeField] private string eventName;
    public bool isStoryEvent;
    [HideInInspector] public bool isCharacterEvent;
    public bool isMutinyEvent;

    [Tooltip("Whether or not this event scales its stat outcomes with the campaign"),HideInInspector]
    public bool isScalableEvent;

    [ShowIf("isStoryEvent")] public int storyIndex;
    [SerializeField] private Sprite backgroundImage;
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
    private float textPrintSpeed = 0.1f;

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
    private bool donePrinting = true;
    private bool showingChoices = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file

        Refresh(); //starts the dialogue
        titleBox.text = eventName;
        backgroundUI.sprite = backgroundImage;
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
        if (!showingChoices && donePrinting && textBox.pageToDisplay == textBox.textInfo.pageCount)
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
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                tempString = text;
            }
            textBox.text = tempString;

            yield return new WaitForSeconds(textPrintSpeed);
        }

        donePrinting = true;
    }

    /// <summary>
    /// Instantiates each choice as a button and tells the game which one has been chosen when clicked
    /// </summary>
    public bool ShowChoices()
    {
        if(!showingChoices && donePrinting && story.currentChoices.Count > 0)
        {
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
            return true;
        }
        return false;
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
        FindObjectOfType<PageController>().UpdateNextPageText();
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
    /// Returns a string of the next line of text, if there is story left
    /// </summary>
    string GetNextStoryBlock()
    {
        string text = ""; //error check
        
        //Allows the story to add different paragraphs up until the next choice
        while (story.canContinue)
        {
            text += story.Continue() + "\n";
        }

        return text;
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
}
