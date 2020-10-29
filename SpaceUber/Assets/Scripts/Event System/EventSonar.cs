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

public class EventSonar : MonoBehaviour
{
    public float spinRate = 5f;

    public List<GameObject> dots;
    private List<Vector3> dotPositions;
    [HideInInspector] public int dotsShown = 0;

    public GameObject bar;
    public Transform anchor;

    private void Start()
    {
        
        bar.transform.localPosition = Vector3.zero;
        
        for(int i = 0; i < dots.Count; i++)
        {
            dotPositions[i] = dots[i].transform.position;
            HideDot(dots[i]);
        }
    }

    private void FixedUpdate()
    {

        //bar.transform.RotateAround(anchor.localPosition, Vector3.back, Time.deltaTime * spinRate);
        bar.transform.Rotate(Vector3.back * Time.deltaTime * (360/spinRate));
    }

    public void ShowDot(GameObject dot)
    {
        dot.SetActive(true);
        dot.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        DotPulse(dot);
    }

    public void HideAllDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            HideDot(dots[i]);
        }
    }
    public void HideDot(GameObject dot)
    {
        dot.SetActive(false);
    }

    private void DotPulse(GameObject dot)
    {
        //decrease opacity
        dot.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(1, 0, spinRate));
        ShowDot(dot);
    }

    public IEnumerator Spin()
    {
        yield return new WaitForSeconds(spinRate);
    }
}
