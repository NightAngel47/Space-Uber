/*
 * ProgressBarUI.cs
 * Author(s): Sam Ferstein
 * Created on: 3/9/2021 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBarUI : MonoBehaviour
{
    public RectTransform line;
    public GameObject currentPoint;
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
                currentPoint.transform.localPosition = new Vector2(67.5f, 15 + 15);
                StartCoroutine(Scale(110));
                return;
            case 1:
                randEvent1.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 135 + 15);
                StartCoroutine(Scale(230));
                return;
            case 2:
                jobEvent1.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 255 + 15);
                StartCoroutine(Scale(350));
                return;
            case 3:
                randEvent2.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 375 + 15);
                StartCoroutine(Scale(470));
                return;
            case 4:
                jobEvent2.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 495 + 15);
                StartCoroutine(Scale(590));
                return;
            case 5:
                randEvent3.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 615 + 15);
                StartCoroutine(Scale(710));
                return;
            case 6:
                jobEvent3.SetActive(true);
                currentPoint.transform.localPosition = new Vector2(67.5f, 735 + 15);
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