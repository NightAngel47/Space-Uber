using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;

    public void Awake()
    {
        shipStats = FindObjectOfType<ShipStats>();
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
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
