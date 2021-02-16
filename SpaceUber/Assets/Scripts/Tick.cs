using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStats shipStats;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;
    private float secondsPassed;
    private Coroutine tickCoroutine;

    public void Awake()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void StartTickUpdate()
    {
        secondsPassed = 0;
        if (tickCoroutine == null)
        {
            tickCoroutine = StartCoroutine(TickUpdate());
        }
    }

    public void StopTickUpdate()
    {
        if (tickCoroutine == null) return;
        StopCoroutine(tickCoroutine);
        tickCoroutine = null;
    }

    public bool IsTickStopped()
    {
        return tickCoroutine == null;
    }

    private IEnumerator TickUpdate()
    {
        while (GameManager.instance.currentGameState == InGameStates.Events)
        {
            secondsPassed += Time.deltaTime;
            if (secondsPassed >= secondsPerTick)
            {
                // reset seconds passsed
                secondsPassed = 0;

                // calculate net food produced per tick
                int netFood = shipStats.FoodPerTick - (int) shipStats.CrewCurrent.x;
                // calculate possible missing food
                int missingFood = shipStats.Food + netFood;
                // will there be missing food, thus starving crew?
                if (missingFood < 0)
                {
                    // update crew morale based on missing food
                    MoraleManager.instance.CrewStarving(missingFood);
                }
                // add net food to food stat
                shipStats.Food += netFood;

                // increment days since events
                shipStats.DaysSince++;

                MoraleManager.instance.CheckMutiny();

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

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
}
