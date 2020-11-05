/*
 * EventSonar.cs
 * Author(s): Scott Acker
 * Created on: 10/20/2020 (en-US)
 * Description: Controls the sonar function of the event timer. When an event is chosen, a ping appears on the sonar.
 * It revolves at the same rate that events are chosen
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventSonar : MonoBehaviour
{
    public float spinRate = 5f;

    public List<GameObject> dots;
    [HideInInspector] public int dotsShown = 0;

    public GameObject bar;
    public Transform anchor;

    private void Start()
    {
        bar.transform.localPosition = Vector3.zero;
        HideAllDots();
    }

    private void FixedUpdate()
    {

        //bar.transform.RotateAround(anchor.localPosition, Vector3.back, Time.deltaTime * spinRate);
        bar.transform.Rotate(Vector3.back * Time.deltaTime * (360/spinRate));
    }

    /// <summary>
    /// Resets sonar to default state
    /// </summary>
    public void ResetSonar()
    {
        HideAllDots();
        StopCoroutine(Spin());
        StartCoroutine(Spin());
    }

    /// <summary>
    /// Hides the entire sonar UI object
    /// </summary>
    public void HideSonar()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the entire sonar UI object
    /// </summary>
    public void ShowSonar()
    {
        gameObject.SetActive(true);
    }


    /// <summary>
    /// Shows the next dot, randomly sets its position, and activates the DotPulse method
    /// </summary>
    public void ShowNextDot()
    {
        if(dotsShown != dots.Count)
        {
            GameObject dot = dots[dotsShown];
            dot.SetActive(true);
            dotsShown++;
            StartCoroutine(DotPulse(dot));
        }
    }

    /// <summary>
    /// Hides all existing dots on the sonar
    /// </summary>
    public void HideAllDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }
        dotsShown = 0;
    }

    /// <summary>
    /// Makes the visible dots start to pulse.
    /// </summary>
    /// <param name="dot"></param>
    private IEnumerator DotPulse(GameObject dot)
    {
        dot.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        
        //decrease opacity
        dot.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Lerp(1, 0, spinRate * Time.deltaTime));

        yield return new WaitForSeconds(spinRate);
        
    }


    /// <summary>
    /// Makes the bar at the center of the bar spin around
    /// </summary>
    /// <returns></returns>
    public IEnumerator Spin()
    {
        yield return new WaitForSeconds(spinRate);
    }
}
