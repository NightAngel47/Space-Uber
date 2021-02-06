/*
 * AnalyticsManager.cs
 * Author(s): Steven Drovie []
 * Created on: 2/5/2021 (en-US)
 * Description: Handles collecting & sending analytics data using Unity Analytics.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsManager
{
    /// <summary>
    /// Collects and Sends data on the completed story and random event.
    /// </summary>
    /// <param name="gameEvent">The event that the player just completed.</param>
    /// <param name="choice">The choice that the player made during the event.</param>
    public static void OnEventComplete(InkDriverBase gameEvent, EventChoice choice)
    {
        var eventType = gameEvent.isStoryEvent ? "storyEvent" : "randomEvent";
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"eventName", gameEvent.EventName},
            {"choiceName", choice.ChoiceName},
            {gameEvent.EventName, choice.ChoiceName}
        };

        Analytics.CustomEvent(eventType, data);
        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Event Complete Sent");
        #endif
    }
}
