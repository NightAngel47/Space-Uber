/*
 * CheatsMenu.cs
 * Author(s): 
 * Created on: 3/8/2021 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class CheatsMenu : MonoBehaviour
{
    private EventSystem es;
    private CampaignManager campMan;
    private ShipStats thisShip;
    private JobManager jm;

    private bool cheatModeActive;
    [SerializeField] private GameObject myCanvas;
    [SerializeField] private TMP_Text cheatTextBox;
    private TMP_InputField inputField;

    [SerializeField] private List<GameObject> randomEvents;
    [SerializeField] private List<GameObject> characterEvents;


    // Start is called before the first frame update
    void Start()
    {
        es = gameObject.GetComponent<EventSystem>();
        campMan = gameObject.GetComponent<CampaignManager>();
        thisShip = GameObject.FindObjectOfType<ShipStats>();
        jm = GameObject.FindObjectOfType<JobManager>();
        
        myCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ActivateCheatMode();
        if(cheatModeActive && Input.GetKeyDown(KeyCode.Return))
        {
            string inputText = inputField.text;
            
            if(inputText != null)
                CheatCheck(inputText);
        }
        
        
    }

    void CheatCheck(string cheatText)
    {
        print("Entered " + cheatText);
    }
    void ActivateCheatMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)
        && Input.GetKeyDown(KeyCode.C))
        {
            if (cheatModeActive)
            {
                cheatModeActive = false;
                myCanvas.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                cheatModeActive = true;
                myCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void JumpToCampaign(int campNum)
    {
        if (cheatModeActive)
        {

        }
    }

    public void PlayRandomEvent(int eventNum)
    {
        if (cheatModeActive)
        {
            GameObject thisEvent = randomEvents[eventNum];
            StartCoroutine(es.CheatRandomEvent(thisEvent));
        }
        
    }    
}
