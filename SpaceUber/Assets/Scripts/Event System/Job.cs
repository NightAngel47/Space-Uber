
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

public class Job : MonoBehaviour
{
    [Tooltip("How many events will happen in this journey")]
    public int maxEvents = 3;

    [Tooltip("Narrative-focused events that will play in this specific order")]
    public List<GameObject> events;

    public string jobName;
    public string description;
    public int payout;
    public bool isSideJob;
}