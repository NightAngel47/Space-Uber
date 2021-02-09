using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private ShipStatsUI shipStatsUI;
    private ShipStats shipStats;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;

    //mutiny variables
    public int maxMutinyMorale = 39;
    public float maxMutinyMoraleMutinyChance = 0.2f;
    public float zeroMoraleMutinyChance = 1f;
    public int baseMutinyCost = 100;
    private int mutinyCount = 0;
    public GameObject mutinyEvent;

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

            // calculate net food produced per tick
            int netFood = shipStats.FoodPerTick - (int) shipStats.CrewCurrent.x;
            // calculate possible missing food
            int missingFood = shipStats.Food + netFood;
            // will there be missing food, thus starving crew?
            if (missingFood < 0)
            {                   
                // update crew morale based on missing food
                shipStats.UpdateCrewMorale(missingFood * shipStats.foodMoraleDamageMultiplier);
            }
            // add net food to food stat
            shipStats.Food += netFood;

            // increment days since events
            shipStats.DaysSince++;

            if(shipStats.Morale < 0)
            {
                shipStats.Morale = 0;
            }
            
            shipStatsUI.UpdateCrewMoraleUI(shipStats.Morale);

            float mutinyChance = (maxMutinyMorale - shipStats.Morale) * zeroMoraleMutinyChance * (1 - maxMutinyMoraleMutinyChance) / maxMutinyMorale + maxMutinyMoraleMutinyChance;
            if(mutinyChance > UnityEngine.Random.value)
            {
                mutinyCount++;
                int mutinyCost = Mathf.RoundToInt((baseMutinyCost * ((100 - shipStats.Morale) / 100.0f)) * mutinyCount);
                mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].choiceRequirements[0].requiredAmount = mutinyCost;
                mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].outcomes[0].amount = -mutinyCost;
                EventSystem.instance.CreateMutinyEvent(mutinyEvent);
            }
            
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
