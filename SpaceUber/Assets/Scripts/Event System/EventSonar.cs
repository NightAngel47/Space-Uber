/*
 * EventSonar.cs
 * Author(s): Scott Acker
 * Created on: 10/20/2020 (en-US)
 * Description: Controls the sonar function of the event timer. When an event is chosen, a ping appears on the sonar.
 * It revolves at the same rate that events are chosen
 */

using UnityEngine;
using System.Collections;

public class EventSonar : MonoBehaviour
{
    public float spinRate = 5f;
    public GameObject dot;
    public GameObject bar;

    void ShowDot()
    {

    }

    void HideDot()
    {

    }

    public IEnumerator Spin()
    {
        yield return new WaitForSeconds(spinRate);
    }
}
