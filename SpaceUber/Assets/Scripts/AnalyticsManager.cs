/*
 * AnalyticsManager.cs
 * Author(s): Steven Drovie []
 * Created on: 2/5/2021 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsManager
{
    public static void OnEventComplete(InkDriverBase gameEvent, EventChoice choice)
    {
        var eventType = gameEvent.isStoryEvent ? "storyEvent" : "randomEvent";
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"eventName", gameEvent.EventName}, 
            {"choiceName", choice.ChoiceName}
        };

        Analytics.CustomEvent(eventType, data);
        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Event Complete Sent");
        #endif
    }
}
