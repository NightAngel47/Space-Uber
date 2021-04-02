/*
 * ProgressBarUI.cs
 * Author(s): Sam Ferstein
 * Created on: 3/9/2021 (en-US)
 * Description: Controls how the progress bar moves and changes based on what event the player is on.
 */

using System;
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

    [SerializeField] private int[] scales;

    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }



    public void StartProgress()
    {
        switch (EventSystem.instance.overallEventIndex)
        {
            case 0:
                break;
            case 1:
                randEvent1.SetActive(true);
                break;
            case 2:
                jobEvent1.SetActive(true);
                break;
            case 3:
                randEvent2.SetActive(true);
                break;
            case 4:
                jobEvent2.SetActive(true);
                break;
            case 5:
                randEvent3.SetActive(true);
                break;
            case 6:
                jobEvent3.SetActive(true);
                break;
            default:
                randEvent1.SetActive(false);
                jobEvent1.SetActive(false);
                randEvent2.SetActive(false);
                jobEvent2.SetActive(false);
                randEvent3.SetActive(false);
                jobEvent3.SetActive(false);
                return;
        }
        
        StartCoroutine(Scale(scales[EventSystem.instance.overallEventIndex]));
    }

    IEnumerator Scale(float maxSize)
    {
        while (maxSize > line.offsetMax.y)
        {
            yield return new WaitWhile(()=>EventSystem.instance.mutiny || Tutorial.Instance.GetTutorialActive());

            float scale = 0;
            if(EventSystem.instance.overallEventIndex > 0)
            {                
                scale = (maxSize - scales[EventSystem.instance.overallEventIndex - 1]) / (eventSystem.TimeBeforeEventRoll * (0.05f / (EventSystem.instance.overallEventIndex + 1) + 1)) * Time.deltaTime;
            }
            else
            {
                scale = maxSize / (eventSystem.TimeBeforeEventRoll * 1.135f) * Time.deltaTime;
            }
            
            line.offsetMax += new Vector2(0, 1) * scale;
            yield return new WaitForEndOfFrame();
        }
    }
}