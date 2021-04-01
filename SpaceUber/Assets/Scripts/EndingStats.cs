using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndingStatTypes { NA = -1, MinigamesPlayed, Credits, Energy, ShipWeapons, Security, HullDurability, Food, Crew, RoomsBought, CrewDeaths}

public class EndingStats : MonoBehaviour
{
    private int[] stats;
    
    public static EndingStats instance;
    
    private void Awake()
    {
        // Singleton pattern that makes sure that there is only one MoraleManager
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetStats();
        }
        else
        {
            stats = new int[10];
            SaveStats();
        }
    }
    
    public void AddToStat(int value, EndingStatTypes stat)
    {
        stats[(int) stat] += value;
        SaveStats();
    }
    
    public int GetStat(EndingStatTypes stat)
    {
        return stats[(int) stat];
    }
    
    private void SaveStats()
    {
        SavingLoadingManager.instance.Save<int[]>("endingStats", stats);
    }
    
    private void ResetStats()
    {
        stats = SavingLoadingManager.instance.Load<int[]>("endingStats");
    }
}
