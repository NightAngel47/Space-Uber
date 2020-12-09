/*
 * EventWarning.cs
 * Author(s): Scott Acker
 * Created on: 12/1/2020 (en-US)
 * Description: Controls the Event Warning UI object, managed by EventSystem. When an event should be played, this
 * UI object will light up. If not, it will look dim
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventWarning : MonoBehaviour
{
    [SerializeField, Tooltip("The text that appears in the warning box")]
    private TMP_Text warningText;

    [SerializeField, Tooltip("The background of the warning box")]
    private Image warningBackground;

    /// <summary>
    /// Makes the Warning box and text light up by increasing alpha values
    /// </summary>
    public void ActivateWarning()
    {
        Color textColor = warningText.color;
        Color backgroundColor = warningBackground.color;

        textColor = new Color(textColor.r, textColor.g, textColor.b, 1f);
        backgroundColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, .75f);

        warningText.color = textColor;
        warningBackground.color = backgroundColor;
    }

    /// <summary>
    /// Makes the Warning box and text power down and fade slightly by decreasing alpha values
    /// </summary>
    public void DeactivateWarning()
    {
        Color textColor = warningText.color;
        Color backgroundColor = warningBackground.color;

        textColor = new Color(textColor.r, textColor.g, textColor.b, .3f);
        backgroundColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, .22f);

        warningText.color = textColor;
        warningBackground.color = backgroundColor;
    }
}
