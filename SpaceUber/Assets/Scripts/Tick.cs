using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;
    private float secondsPassed;
    private Coroutine tickCoroutine;

    public void Awake()
    {
        shipStats = FindObjectOfType<ShipStats>();
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
    }

    public void StartTickUpdate()
    {
        secondsPassed = 0;
        tickCoroutine = StartCoroutine(TickUpdate());
    }

    public void StopTickUpdate()
    {
        if (tickCoroutine != null)
        {
            StopCoroutine(tickCoroutine);
        }
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
            
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForEndOfFrame();
    }
}
