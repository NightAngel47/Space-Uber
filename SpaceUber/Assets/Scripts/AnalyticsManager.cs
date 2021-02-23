/*
 * AnalyticsManager.cs
 * Author(s): Steven Drovie []
 * Created on: 2/5/2021 (en-US)
 * Description: Handles collecting & sending analytics data using Unity Analytics.
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsManager
{
    #region Event Analytics

    /// <summary>
    /// The time that the player started the event, used to calculate how long the player spends in an event.
    /// </summary>
    private static float eventStartTime;

    /// <summary>
    /// Tracks the choices made during an event to be sent once an event concludes.
    /// </summary>
    public static List<string> EventChoiceData { get; } = new List<string>();

    /// <summary>
    /// Collects and Sends data when an event is started.
    /// </summary>
    /// <param name="gameEvent">The event that has started.</param>
    /// <param name="eventLockedIn">If </param>
    public static void OnEventStarted(InkDriverBase gameEvent, bool eventLockedIn)
    {
        string eventType = GetEventType(ref gameEvent);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"eventName", gameEvent.EventName}          // tracks the event name
        };

        if (!gameEvent.isCharacterEvent && !gameEvent.isMutinyEvent)
        {
            data.Add("eventWasLockedIn", eventLockedIn); // tracks if story/random event was skipped to or not
        }

        Analytics.CustomEvent(eventType + "-started", data);

        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Event Started Sent");
        #endif
        
        eventStartTime = Time.time; // store time for determining how long the player was in an event
    }
    
    /// <summary>
    /// Collects and Sends data when an event is completed.
    /// </summary>
    /// <param name="gameEvent">The event that the player just completed.</param>
    public static void OnEventComplete(InkDriverBase gameEvent)
    {
        string eventType = GetEventType(ref gameEvent);

        float timeInEvent = Time.time - eventStartTime;
        
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"eventName", gameEvent.EventName},         // tracks the event name
            {"timeInEvent", timeInEvent}                // tracks how long the player was in the event
        };

        string choiceNames = "";
        for (var i = 0; i < EventChoiceData.Count; i++)
        {
            choiceNames += EventChoiceData[i];
            data.Add("choiceName", choiceNames);        // tracks the choice name
            if (i + 1 < EventChoiceData.Count) choiceNames += " > ";
        }
        
        data.Add(gameEvent.EventName, choiceNames);     // tracks event name with all event choices

        Analytics.CustomEvent(eventType + "-completed", data);
        
        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Event Complete Sent");
        #endif
    }

    /// <summary>
    /// Returns the type of game event as a string.
    /// </summary>
    /// <param name="gameEvent">The game event that is being checked.</param>
    /// <returns>A string for the event type.</returns>
    private static string GetEventType(ref InkDriverBase gameEvent)
    {
        if (gameEvent.isMutinyEvent) return "mutinyEvent";
        if (gameEvent.isCharacterEvent) return "characterEvent";
        if (gameEvent.isStoryEvent) return "storyEvent";
        return gameEvent.nextChoices.Count != 0 ? "randomEvent" : "introEvent";
    }

    #endregion

    #region Ship Building Analytics

    /// <summary>
    /// Dictionary of rooms and their count on the ship. Room name is key, and count is value.
    /// </summary>
    private static Dictionary<string, int> shipRooms = new Dictionary<string, int>();

    /// <summary>
    /// Collects and Sends data on the state of the ship when entering the starport.
    /// </summary>
    /// <param name="ship">The player's ship</param>
    public static void OnEnteringStarport(ShipStats ship)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"credits", ship.Credits},              // tracks starting credits
            {"power", (int)ship.EnergyRemaining.x}, // tracks starting power
            {"roomCounts", RoomCountsToString()}    // tracks room counts on ship
        };

        Analytics.CustomEvent("enteringStarport", data);

        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Entering Starport Sent");
        #endif
    }
    
    /// <summary>
    /// Collects and Sends data on the state of the ship when leaving the starport after ship building and crew management.
    /// </summary>
    /// <param name="ship">The player's ship</param>
    public static void OnLeavingStarport(ShipStats ship)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"credits", ship.Credits},              // tracks remaining credits
            {"power", (int)ship.EnergyRemaining.x}, // tracks remaining power
            {"roomCounts", RoomCountsToString()}    // tracks room counts on ship
        };

        Analytics.CustomEvent("leavingStarport", data);

        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On Leaving Starport Sent");
        #endif
    }

    /// <summary>
    /// Increases count for passed room if first time placed. Otherwise, adds entry for new room.
    /// </summary>
    /// <param name="room">The room being added.</param>
    public static void AddRoomForAnalytics(RoomStats room)
    { 
        if (shipRooms.ContainsKey(room.roomName))
        {
            shipRooms[room.roomName]++;
        }
        else
        {
            shipRooms.Add(room.roomName, 1);
        }
    }
    
    /// <summary>
    /// Decreases count for passed room if room has been placed before. Removes entry for room if last one.
    /// </summary>
    /// <param name="room">The room being removed.</param>
    public static void SubtractRoomForAnalytics(RoomStats room)
    {
        if (!shipRooms.ContainsKey(room.roomName)) return;
        
        shipRooms[room.roomName]--;

        if (shipRooms[room.roomName] == 0)
        {
            shipRooms.Remove(room.roomName);
        }
    }

    /// <summary>
    /// Returns formatted list of rooms on the ship.
    /// </summary>
    /// <returns>String of rooms formatted to display roomName:count|</returns>
    private static string RoomCountsToString()
    {
        return shipRooms.Aggregate("", (current, roomData) => current + roomData.Key + ":" + roomData.Value + "|");
    }

    #endregion

    #region Mini-Game Analytics

    /// <summary>
    /// Time at the start of the minigame. Used to calculate how long players spend in the minigame.
    /// </summary>
    private static float minigameStartTime = 0;

    /// <summary>
    /// Collects and Sends data when the player starts a minigame.
    /// </summary>
    /// <param name="miniGame">The mini game that the player started playing.</param>
    public static void OnMiniGameStarted(MiniGameType miniGame)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"minigameName", miniGame},                 // tracks minigame name
        };

        Analytics.CustomEvent("minigameStarted", data);

        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On MiniGame Started Sent");
        #endif

        minigameStartTime = Time.time;
    }
    
    /// <summary>
    /// Collects and Sends data when the player finishes a minigame.
    /// </summary>
    /// <param name="miniGame">The minigame that the player finished.</param>
    /// <param name="didSucceed">Did the player succeed at the minigame?</param>
    /// <param name="statModification">The base stat output for the minigame when finished.</param>
    public static void OnMiniGameFinished(MiniGameType miniGame, bool didSucceed, float statModification = 0)
    {
        float timeSpent = Time.time - minigameStartTime;
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"minigameName", miniGame},                 // tracks minigame name
            {"didSucceed", didSucceed},                 // tracks if player beat minigame
            {"timeSpentPlaying",timeSpent}              // tracks time spent playing the minigame
        };

        if (miniGame == MiniGameType.SlotMachine)
        {
            data.Add("slotsOutput", statModification);  // tracks the output for slots
        }

        Analytics.CustomEvent("minigameFinished", data);

        #if UNITY_EDITOR
        Debug.LogWarning("Analytics: On MiniGame Finished Sent");
        #endif
    }

    #endregion
}
