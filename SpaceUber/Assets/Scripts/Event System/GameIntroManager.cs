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
    private AdditiveSceneManager asm;
    private EventCanvas eventCanvas;
    [Tooltip("Attach the.JSON file you want read to this")]
    public TextAsset inkJSONAsset;
    [SerializeField] private string eventName;
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
    void Start()
    {
        asm = FindObjectOfType<AdditiveSceneManager>();
        story = new Story(inkJSONAsset.text); //this draws text out of the JSON file
        AssignStatusFromEventSystem();

        //Refresh(); //starts the dialogue
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
        titleBox = eventCanvas.titleBox;
        textBox = eventCanvas.textBox;
        resultsBox = eventCanvas.choiceResultsBox;
        backgroundUI = eventCanvas.backgroundImage;
        buttonGroup = eventCanvas.buttonGroup;

        resultsBox.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        resultsBox.SetActive(false);
    }

    public void ConcludeEvent()
    {
        if (donePrinting && textBox.pageToDisplay == textBox.textInfo.pageCount)
        {
            if (!story.canContinue && story.currentChoices.Count == 0)
            {
                //go to job select
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayGameIntro()
    {
        yield return new WaitForSeconds(2);
    }

    private void CreateEvent(GameObject newEvent)
    {
        StartCoroutine(AudioManager.instance.Fade(AudioManager.instance.GetCurrentRadioSong(), 1, false));

        eventCanvas = FindObjectOfType<EventCanvas>();

    }
}
