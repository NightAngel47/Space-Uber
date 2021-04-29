/*
 * MoneyEndingBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 4/28/2021 (en-US)
 * Description: 
 */

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class MoneyEndingBehaviour : MonoBehaviour
{
    [SerializeField, Foldout("UI Info")] private string titleName = "Ending";
    [SerializeField, Foldout("UI Info")] private Sprite backgroundImage;

    [SerializeField, Foldout("Threshold Stats")] private StatThresholdToggle statThreshold;
    [SerializeField, Foldout("Threshold Stats"), TextArea] private string meetsThresholdText;
    [SerializeField, Foldout("Threshold Stats"), TextArea] private string belowThresholdText;
    
    [SerializeField, Foldout("Scene Transition")] private InGameStates stateToTransition;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FindObjectOfType<EventCanvas>());
        
        SetUIInfo(FindObjectOfType<EventCanvas>(), statThreshold.MeetsThreshold(FindObjectOfType<ShipStats>()));
    }

    /// <summary>
    /// Set the info for the UI based on if the player meets the stats threshold
    /// </summary>
    /// <param name="eventCanvas">The UI canvas being used to display the ending screen.</param>
    /// <param name="meetsThreshold">The check of whether the player meets the stats threshold using StatThresholdToggle.MeetsThreshold</param>
    protected virtual void SetUIInfo(EventCanvas eventCanvas, bool meetsThreshold)
    {
        // set UI
        eventCanvas.titleBox.text = titleName;
        eventCanvas.backgroundImage.sprite = backgroundImage;
        
        // set next button to say continue
        FindObjectOfType<PageController>().UpdateNextPageText();

        // set text based on threshold
        eventCanvas.textBox.text = meetsThreshold ? meetsThresholdText : belowThresholdText;
    }

    /// <summary>
    /// Called by the next button to transition to the next scene
    /// </summary>
    public void TransitionToScene()
    {
        GameManager.instance.ChangeInGameState(stateToTransition);
    }
}
