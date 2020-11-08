
/*
 * Job.cs
 * Author(s): Scott Acker
 * Created on: 10/16/2020 (en-US)
 * Description: Stores information about a particular job. Intended to be a prefab
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NaughtyAttributes;

public class Job : MonoBehaviour
{
    [Tooltip("How many events will happen in this journey"),HideIf("isSideJob")]
    public int maxEvents = 3;

    [Tooltip("The events included in this job")]
    public List<GameObject> events;

    public string jobName;
    public string description;
    public int payout;
    public bool isSideJob;
}