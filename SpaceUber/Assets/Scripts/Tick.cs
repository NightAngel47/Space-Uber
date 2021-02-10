using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;
    private MoraleManager moraleManager;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;

    public void Awake()
    {
        shipStats = FindObjectOfType<ShipStats>();
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
        moraleManager = FindObjectOfType<MoraleManager>();
    }

    public float SecondsPerTick { get; set; } = 5;

    public bool TicksPaused { get; set; }

    public bool TickStop { get; set; } = true;

    public void CallTickUpdate()
    {
        TickStop = false;
        TicksPaused = false;
        StartCoroutine(TickUpdate());
    }

    private IEnumerator TickUpdate()
    {
        while (!TickStop)
        {
            while (TicksPaused)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(SecondsPerTick);

            while (TicksPaused)
            {
                yield return new WaitForFixedUpdate();
            }

            // calculate net food produced per tick
            int netFood = shipStats.FoodPerTick - (int) shipStats.CrewCurrent.x;
            // calculate possible missing food
            int missingFood = shipStats.Food + netFood;
            // will there be missing food, thus starving crew?
            if (missingFood < 0)
            {                   
                // update crew morale based on missing food
                moraleManager.CrewStarving(missingFood);
            }
            // add net food to food stat
            shipStats.Food += netFood;

            // increment days since events
            shipStats.DaysSince++;

            if(moraleManager.CrewMorale < 0)
            {
                moraleManager.CrewMorale = 0;
            }
            
            shipStatsUI.UpdateCrewMoraleUI(moraleManager.CrewMorale);
            
            moraleManager.CheckMutiny();
            
            RoomStats[] rooms = FindObjectsOfType<RoomStats>();
            
            foreach(RoomStats room in rooms)
            {
                room.KeepRoomStatsUpToDateWithMorale();
            }

            if (shipStats.ShipHealthCurrent.x <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
                AudioManager.instance.PlaySFX("Hull Death");
                AudioManager.instance.PlayMusicWithTransition("Death Theme");
            }
        }
    }

    public void PauseTickEvents()
    {
        TicksPaused = true;
    }

    public void UnpauseTickEvents()
    {
        TicksPaused = false;
    }

    public void StopTickEvents()
    {
        TickStop = true;
    }

    public void StartTickEvents()
    {
        if (TickStop)
        {
            TickStop = false;
            TicksPaused = false;
            StartCoroutine(TickUpdate());
        }
    }
}
