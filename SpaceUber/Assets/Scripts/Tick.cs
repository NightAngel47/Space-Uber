using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;

    //tick variables
    private int secondsPerTick = 5;
    private bool ticksPaused;
    private bool tickStop = true;

    public void Awake()
    {
        shipStats = FindObjectOfType<ShipStats>();
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
    }

    public int SecondsPerTick
    {
        get { return secondsPerTick; }
        set
        {
            secondsPerTick = value;
        }
    }

    public bool TicksPaused
    {
        get { return ticksPaused; }
        set { ticksPaused = value; }
    }

    public bool TickStop
    {
        get { return tickStop; }
        set { tickStop = value; }
    }

    public void CallTickUpdate()
    {
        TickStop = false;
        TicksPaused = false;
        StartCoroutine("TickUpdate");
    }

    private IEnumerator TickUpdate()
    {
        while (!tickStop)
        {
            while (ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(secondsPerTick);

            while (ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitWhile(() => ticksPaused);

            shipStats.Food += (shipStats.FoodPerTick - (int)shipStats.CrewCurrent.x);
            //food += foodPerTick - crewCurrent;
            if (shipStats.Food < 0)
            {
                //crewMorale += (food * foodMoraleDamageMultiplier);
                shipStats.Food = 0;
            }
            shipStatsUI.UpdateFoodUI(shipStats.Food, shipStats.FoodPerTick, (int)shipStats.CrewCurrent.x);

            // increment days since events
            shipStats.DaysSince++;

            //if(crewMorale < 0)
            //{
            //    crewMorale = 0;
            //}

            //UpdateMoraleShipStatsUI();

            //float mutinyChance = (maxMutinyMorale - crewMorale) * zeroMoraleMutinyChance / maxMutinyMorale;
            //if(mutinyChance > UnityEngine.Random.value)
            //{
            //    GameManager.instance.ChangeInGameState(InGameStates.Mutiny);
            //}

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
        ticksPaused = true;
    }

    public void UnpauseTickEvents()
    {
        ticksPaused = false;
    }

    public void StopTickEvents()
    {
        tickStop = true;
    }

    public void StartTickEvents()
    {
        if (tickStop)
        {
            tickStop = false;
            ticksPaused = false;
            StartCoroutine(TickUpdate());
        }
    }
}
