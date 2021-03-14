using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tick : MonoBehaviour
{
    private ShipStats shipStats;

    //tick variables
    [SerializeField, Min(0.1f)] private float secondsPerTick = 5;
    private float secondsPassed;
    private Coroutine tickCoroutine;
    
    // days since variables
    private int daysSince;
    private int daysSinceChat;
    private TMP_Text daysSinceDisplay;

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
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Interface_Runtime").isLoaded);
        daysSinceDisplay = GameObject.FindGameObjectWithTag("DaysSince").GetComponent<TMP_Text>();
        
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
                DaysSince++;
                daysSinceChat++;
                
                MoraleManager.instance.CheckMutiny();
                
                shipStats.CheckForDeath();
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
    
    

    public int DaysSince
    {
        get => daysSince;
        set
        {
            daysSince = value;
            if(daysSinceDisplay != null) daysSinceDisplay.text = daysSince.ToString();
        }
    }

    public int DaysSinceChat
    {
        get => daysSinceChat;
        set
        {
            daysSince = value;
        }
    }
}
