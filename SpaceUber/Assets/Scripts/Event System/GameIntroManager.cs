/*
 * GameIntroManager.cs
 * Author(s): Scott Acker
 * Created on: 4/23/2021 (en-US)
 * Description: Combines inkDriverBase and EventSystem functionality in order to run one introduction event that does not require
 * the additional functionality included in the two scripts
 */

using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;
using TMPro;

public class GameIntroManager : MonoBehaviour
{
    private EventCanvas eventCanvas;
    [Tooltip("Attach the.JSON file you want read to this")]
    public TextAsset inkJSONAsset;
    [SerializeField] private string eventName;
    [SerializeField] private bool hasAnimatedBG = false;
    [SerializeField, HideIf("hasAnimatedBG")] private Sprite backgroundImage;
    [SerializeField, ShowIf("hasAnimatedBG")] private GameObject backgroundAnimation;

    private Transform buttonGroup;
    private TMP_Text titleBox;
    private TMP_Text textBox;
    private Image backgroundUI;

    [SerializeField, Tooltip("Controls how fast text will scroll. It's the seconds of delay between words, so less is faster.")]
    private float textPrintSpeed = 0.1f;

    public string eventIntroSFX;

    [Dropdown("eventMusicTracks")]
    public string eventBGM;
    
    /// <summary>
    /// The story itself being read
    /// </summary>
    protected Story story;

    /// <summary>
    /// Whether the latest bit of text is done printing so it can show the choices
    /// </summary>
    private bool donePrinting = true;

    private List<string> eventMusicTracks
    {
        get
        {
            return new List<string>() { "", "General Theme", "Wormhole", "Engine Malfunction", "Engine Delivery", "Black Market", "Clone Ambush Intro", "Safari Tampering", "Clone Ambush Negotiation", "Clone Ambush Fight", "Ejection", "Asteroid Mining", "Blockade", "Crop Blight", "Door Malfunction", "Drug Overdose", "Escaped Convicts", "Septic Malfunction", "Soothing Light", "Spatial Aurora", "Food Poisoning", "Hostage Situation", "Hull Maintenance", "Death Theme", "Shocking Situation", "Stranded Stranger", "Void Music", "Void Music [Muffled]", "Ammunition Error", "An Innocent Proposal", "Charity Donation", "Crew Fight", "Distress Signal", "Drag Race", "Frozen in Time", "Fungus Among Us", "Homesick", "Just a Comet", "Lost in Translation", "Neon Nightmare [Chill]", "Neon Nightmare", "Surprise Mechanics", "Taking a Toll", "Thumping" };
        }
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        StartCoroutine(AudioManager.instance.Fade(AudioManager.instance.GetCurrentRadioSong(), 1, false));
        
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file
        
        yield return new WaitUntil(() => FindObjectOfType<EventCanvas>());
        AssignStatusFromEventSystem();

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
    }

    public void AssignStatusFromEventSystem()
    {
        eventCanvas = FindObjectOfType<EventCanvas>();
        titleBox = eventCanvas.titleBox;
        textBox = eventCanvas.textBox;
        backgroundUI = eventCanvas.backgroundImage;
        buttonGroup = eventCanvas.buttonGroup;
    }

    public void ConcludeEvent()
    {
        if (donePrinting && textBox.pageToDisplay == textBox.textInfo.pageCount)
        {
            if (!story.canContinue && story.currentChoices.Count == 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
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
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                tempString = text;
            }
            textBox.text = tempString;

            yield return new WaitForSeconds(textPrintSpeed);
        }

        donePrinting = true;
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