/*
 * ProgressBarUI.cs
 * Author(s): Sam Ferstein
 * Created on: 3/9/2021 (en-US)
 * Description: Controls how the progress bar moves and changes based on what event the player is on.
 */

using System.Collections;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
    public RectTransform line;
    public GameObject randEvent1;
    public GameObject jobEvent1;
    public GameObject randEvent2;
    public GameObject jobEvent2;
    public GameObject randEvent3;
    public GameObject jobEvent3;

    public void StartProgress()
    {
        switch (EventSystem.instance.overallEventIndex)
        {
            case 0:
                StartCoroutine(Scale(123));
                return;
            case 1:
                randEvent1.SetActive(true);
                StartCoroutine(Scale(243));
                return;
            case 2:
                jobEvent1.SetActive(true);
                StartCoroutine(Scale(363));
                return;
            case 3:
                randEvent2.SetActive(true);
                StartCoroutine(Scale(483));
                return;
            case 4:
                jobEvent2.SetActive(true);
                StartCoroutine(Scale(603));
                return;
            case 5:
                randEvent3.SetActive(true);
                StartCoroutine(Scale(723));
                return;
            case 6:
                jobEvent3.SetActive(true);
                return;
            default:
                randEvent1.SetActive(false);
                jobEvent1.SetActive(false);
                randEvent2.SetActive(false);
                jobEvent2.SetActive(false);
                randEvent3.SetActive(false);
                jobEvent3.SetActive(false);
                return;
        }
    }

    IEnumerator Scale(float maxSize)
    {
        while (maxSize > line.offsetMax.y)
        {
            line.offsetMax += new Vector2(0, 1) * Time.deltaTime * 6;
            yield return null;
        }
    }
}