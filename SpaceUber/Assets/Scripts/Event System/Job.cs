
/*
 * Job.cs
 * Author(s): Scott Acker
 * Created on: 10/16/2020 (en-US)
 * Description: Stores information about a particular job. Intended to be a prefab
 */

using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class Job : MonoBehaviour
{
    [Tooltip("Determines when in the campaign sequence should this job be available to the player. " +
             "For example: if set to 0 then it will appear as part of the list of available first jobs in this campaign."), 
     HideIf("isSideJob")]
    public int campaignIndexAvailable = 0;
    
    [Tooltip("How many random events will happen in this journey")]
    public int maxRandomEvents = 3;
    
    [Tooltip("How many story events will happen in this journey")]
    public int maxStoryEvents = 3;

    [Tooltip("The introduction to the job. Will be played immediately when reaching the travel scene"), HideIf("isSideJob")]
    public List<GameObject> introEvents = new List<GameObject>();

    [Tooltip("The story events included in this job"),HideIf("isSideJob"), ReorderableList]
    public List<GameObject> storyEvents;

    [Tooltip("The random events that will be included in this job"), ReorderableList]
    public List<GameObject> randomEvents;


    public string jobName;
    public string description;
    public int payout;
    public bool isSideJob;
    public Sprite jobListImage;
}