using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;

    //tick variables
    [SerializeField] private int secondsPerTick = 5;
    [SerializeField] private bool ticksPaused;
    [SerializeField] private bool tickStop = true;

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
            daysSince++;
            daysSinceDisplay.text = daysSince.ToString();

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
