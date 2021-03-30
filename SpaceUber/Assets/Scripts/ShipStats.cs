/*
 * ShipStats.cs
 * Author(s): Grant Frey
 * Created on: 9/14/2020 (en-US)
 * Description:
 */

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;

public class ShipStats : MonoBehaviour
{
    [SerializeField ,Tooltip("Starting amount of credits"), Foldout("Starting Ship Stats")]
    private int startingCredits;
    [SerializeField, Tooltip("Starting amount of energy"), Foldout("Starting Ship Stats")]
    private int startingEnergy;
    [SerializeField, Tooltip("Starting value of security"), Foldout("Starting Ship Stats")]
    private int startingSecurity;
    [SerializeField, Tooltip("Starting value of ship weapons"), Foldout("Starting Ship Stats")]
    private int startingShipWeapons;
    [SerializeField, Tooltip("Starting amount of crew"), Foldout("Starting Ship Stats")]
    private int startingCrew;
    [SerializeField, Tooltip("Starting amount of food"), Foldout("Starting Ship Stats")]
    private int startingFood;
    [SerializeField, Tooltip("Starting amount of ship health"), Foldout("Starting Ship Stats")]
    private int startingShipHealth;

    [HideInInspector] public CharacterStats cStats;

    //private List<RoomStats> rooms;

    [HideInInspector] public GameObject roomBeingPlaced;

    public enum Stats { NA = -1, Credits, Payout, EnergyMax, EnergyRemaining, EnergyUnassigned, Security, ShipWeapons, CrewCapacity, CrewCurrent, CrewUnassigned, Food, FoodPerTick, ShipHealthMax, ShipHealthCurrent}

    private int[] stats = new int[14];

    /// <summary>
    /// Reference to the ship stats UI class.
    /// </summary>
    private ShipStatsUI shipStatsUI;

    /// <summary>
    /// Reference to tick
    /// </summary>
    private Tick tick;

    private void Awake()
    {
        shipStatsUI = GetComponent<ShipStatsUI>();
        tick = FindObjectOfType<Tick>();
        cStats = gameObject.GetComponent<CharacterStats>();
    }

    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            LoadShipStats();
        }
        else
        {
            SetStartingStats();
            SaveShipStats();
        }
    }

    private void SetStartingStats()
    {
        StatsArray = new int[] {startingCredits, 0, startingEnergy, startingEnergy, startingEnergy, startingSecurity, startingShipWeapons, startingCrew, startingCrew, startingCrew, startingFood, 0, startingShipHealth, startingShipHealth};
    }

    /// <summary>
    /// Property for Credits. Getter and setter
    /// </summary>
    public int Credits
    {
        get => stats[(int) Stats.Credits];
        set
        {
            int prevValue = stats[(int) Stats.Credits];
            SetObjectBeingPlaced();
            stats[(int) Stats.Credits] = value;
            /*
            if (creditAddition >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Credits");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Credits");
            }
            */
            if (stats[(int) Stats.Credits] <= 0)
            {
                stats[(int) Stats.Credits] = 0;
            }

            shipStatsUI.UpdateCreditsUI(stats[(int) Stats.Credits], stats[(int) Stats.Payout]);
            shipStatsUI.ShowCreditsUIChange(value - prevValue);
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value - prevValue > 0)
            {
                EndingStats.instance.AddToStat(value - prevValue, EndingStatTypes.Credits);
            }
        }
    }

    /// <summary>
    /// Property for Payout. Getter and Setter
    /// </summary>
    public int Payout
    {
        get => stats[(int) Stats.Payout];
        set
        {
            int initialPayout = stats[(int) Stats.Payout];
            stats[(int) Stats.Payout] = value;

            if (stats[(int) Stats.Payout] <= 0)
            {
                stats[(int) Stats.Payout] = 0;
            }

            shipStatsUI.UpdateCreditsUI(stats[(int) Stats.Credits], stats[(int) Stats.Payout]);
            shipStatsUI.ShowCreditsUIChange(0, stats[(int) Stats.Payout] - initialPayout);
        }
    }

    /// <summary>
    /// Property for Energy. Getter and Setter. Vector2 x is energyRemaining | y is energyMax | z is energyUnassigned
    /// </summary>
    public Vector3 Energy // x = energyRemaining y = energyMax z = energyUnassigned
    {
        get => new Vector3(stats[(int) Stats.EnergyRemaining], stats[(int) Stats.EnergyMax], stats[(int) Stats.EnergyUnassigned]);
        set
        {
            Vector3 prevValue = new Vector3(stats[(int) Stats.EnergyRemaining], stats[(int) Stats.EnergyMax], stats[(int) Stats.EnergyUnassigned]);
            stats[(int) Stats.EnergyMax] = (int)value.y;
            stats[(int) Stats.EnergyRemaining] = (int)value.x;
            stats[(int)Stats.EnergyUnassigned] = (int)value.z;
            /*
            if (energyRemainingAddition >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Energy");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Energy");
            }
            */
            if (stats[(int) Stats.EnergyRemaining] <= 0)
            {
                stats[(int) Stats.EnergyRemaining] = 0;
            }

            if (stats[(int) Stats.EnergyUnassigned] <= 0)
            {
                stats[(int) Stats.EnergyUnassigned] = 0;
            }

            if (stats[(int) Stats.EnergyRemaining] >= stats[(int) Stats.EnergyUnassigned])
            {
                stats[(int) Stats.EnergyRemaining] = stats[(int) Stats.EnergyUnassigned];
            }

            shipStatsUI.UpdateEnergyUI(stats[(int) Stats.EnergyRemaining], stats[(int) Stats.EnergyUnassigned], stats[(int)Stats.EnergyMax]);
            shipStatsUI.ShowEnergyUIChange((int)(value.x - prevValue.x), (int)(value.z - prevValue.z));
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value.x - prevValue.x > 0)
            {
                EndingStats.instance.AddToStat((int)(value.x - prevValue.x), EndingStatTypes.Energy);
            }
        }
    }

    /// <summary>
    /// Property for Security. Getter and Setter
    /// </summary>
    public int Security
    {
        get => stats[(int) Stats.Security];
        set
        {
            int prevValue = stats[(int) Stats.Security];
            stats[(int) Stats.Security] = value;
            /*
            if (securityAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Security");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Security");
            }
            */
            if (stats[(int) Stats.Security] <= 0)
            {
                stats[(int) Stats.Security] = 0;
            }

            shipStatsUI.UpdateSecurityUI(stats[(int) Stats.Security]);
            shipStatsUI.ShowSecurityUIChange(value - prevValue);
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value - prevValue > 0)
            {
                EndingStats.instance.AddToStat(value - prevValue, EndingStatTypes.Security);
            }
        }
    }

    /// <summary>
    /// Property for ShipWeapons. Getter and Setter
    /// </summary>
    public int ShipWeapons
    {
        get => stats[(int) Stats.ShipWeapons];
        set
        {
            int prevValue = stats[(int) Stats.ShipWeapons];
            stats[(int) Stats.ShipWeapons] = value;
            /*
            if (shipWeaponsAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Weapons");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Weapons");
            }
            */
            if (stats[(int) Stats.ShipWeapons] <= 0)
            {
                stats[(int) Stats.ShipWeapons] = 0;
            }

            shipStatsUI.UpdateShipWeaponsUI(stats[(int) Stats.ShipWeapons]);
            shipStatsUI.ShowShipWeaponsUIChange(value - prevValue);
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value - prevValue > 0)
            {
                EndingStats.instance.AddToStat(value - prevValue, EndingStatTypes.ShipWeapons);
            }
        }
    }

    /// <summary>
    /// Property for CrewCurrent. Getter for crewCurrent and setter for crewCurrent, crewCapacity, and crewUnassigned. x = crewCurrent, y = crewCapacity, z = crewUnnassigned
    /// </summary>
    public Vector3 CrewCurrent //x = crewCurrent y = crewCapacity z = crewUnnassigned
    {
        get => new Vector3(stats[(int) Stats.CrewCurrent], stats[(int) Stats.CrewCapacity], stats[(int) Stats.CrewUnassigned]);
        set
        {
            if (GameManager.instance.currentGameState == InGameStates.CrewManagement)
            {
                SetObjectBeingPlaced();
            }

            Vector3 prevValue = new Vector3(stats[(int) Stats.CrewCurrent], stats[(int) Stats.CrewCapacity], stats[(int) Stats.CrewUnassigned]);
            stats[(int) Stats.CrewCurrent] = (int)value.x;
            stats[(int) Stats.CrewCapacity] = (int)value.y;
            stats[(int) Stats.CrewUnassigned] = (int)value.z;

            if (stats[(int) Stats.CrewCurrent] - prevValue.x != 0)
            {
                stats[(int) Stats.CrewUnassigned] = stats[(int) Stats.CrewCurrent] - (int)prevValue.x + (int)prevValue.z;
            }

            if (stats[(int) Stats.CrewCurrent] - prevValue.x < 0)
            {
                if(stats[(int) Stats.CrewUnassigned] < 0)
                {
                    RemoveRandomCrew(Mathf.Abs(stats[(int) Stats.CrewUnassigned]));
                }

                if(stats[(int) Stats.CrewCurrent] - prevValue.x < stats[(int) Stats.CrewCapacity] - prevValue.y)
                {
                    MoraleManager.instance.CrewLoss((int)(stats[(int) Stats.CrewCurrent] - prevValue.x));
                    EndingStats.instance.AddToStat((int)(stats[(int) Stats.CrewCurrent] - prevValue.x), EndingStatTypes.CrewDeaths);
                }
            }

            /*
            if (crewRemainingAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Crew");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Crew");
            }
            */

            if (stats[(int) Stats.CrewCurrent] <= 0)
            {
                stats[(int) Stats.CrewCurrent] = 0;
            }
            if (stats[(int) Stats.CrewCurrent] >= stats[(int) Stats.CrewCapacity])
            {
                stats[(int) Stats.CrewUnassigned] += stats[(int) Stats.CrewCapacity] - stats[(int) Stats.CrewCurrent];
                stats[(int) Stats.CrewCurrent] = stats[(int) Stats.CrewCapacity];
            }

            if (stats[(int) Stats.CrewUnassigned] <= 0)
            {
                stats[(int) Stats.CrewUnassigned] = 0;
            }

            shipStatsUI.UpdateCrewUI(stats[(int) Stats.CrewUnassigned], stats[(int) Stats.CrewCurrent], stats[(int) Stats.CrewCapacity]);
            shipStatsUI.ShowCrewUIChange((int)(value.z - prevValue.z), (int)(value.x - prevValue.x), (int)(value.y - prevValue.y));
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value.x - prevValue.x > 0)
            {
                EndingStats.instance.AddToStat((int)(value.x - prevValue.x), EndingStatTypes.Crew);
            }
        }
    }

    /// <summary>
    /// Property for Food. Getter and Setter
    /// </summary>
    public int Food
    {
        get => stats[(int) Stats.Food];
        set
        {
            int prevValue = stats[(int) Stats.Food];
            stats[(int) Stats.Food] = value;
            /*
            if (foodAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Food");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Food");
            }
            */
            if (stats[(int) Stats.Food] <= 0)
            {
                stats[(int) Stats.Food] = 0;
            }

            shipStatsUI.UpdateFoodUI(stats[(int) Stats.Food], stats[(int) Stats.FoodPerTick], stats[(int) Stats.CrewCurrent]);
            shipStatsUI.ShowFoodUIChange(value - prevValue, 0);
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value - prevValue > 0)
            {
                EndingStats.instance.AddToStat(value - prevValue, EndingStatTypes.Food);
            }
        }
    }

    /// <summary>
    /// Property for FoodPerTick. Getter and Setter
    /// </summary>
    public int FoodPerTick
    {
        get => stats[(int) Stats.FoodPerTick];
        set
        {
            int prevValue = stats[(int) Stats.FoodPerTick];
            stats[(int) Stats.FoodPerTick] = value;

            if(stats[(int) Stats.FoodPerTick] <= 0)
            {
                stats[(int) Stats.FoodPerTick] = 0;
            }

            shipStatsUI.UpdateFoodUI(stats[(int) Stats.Food], stats[(int) Stats.FoodPerTick], stats[(int) Stats.CrewCurrent]);
            shipStatsUI.ShowFoodUIChange(0, value - prevValue);
        }
    }

    /// <summary>
    /// Property for shipHealthCurrent. Getter and Setter, sets the shipHealth max as well. x = shipHealthCurrent, y = shipHealthMax
    /// </summary>
    public Vector2 ShipHealthCurrent //x = shipHealthCurrent y = shipHealthMax
    {
        get => new Vector2(stats[(int) Stats.ShipHealthCurrent], stats[(int) Stats.ShipHealthMax]);
        set
        {
            Vector2 prevValue = new Vector2(stats[(int) Stats.ShipHealthCurrent], stats[(int) Stats.ShipHealthMax]);
            stats[(int) Stats.ShipHealthCurrent] = (int)value.x;
            stats[(int) Stats.ShipHealthMax] = (int)value.y;

            if(stats[(int) Stats.ShipHealthCurrent] <= 0)
            {
                stats[(int) Stats.ShipHealthCurrent] = 0;
            }

            if(stats[(int) Stats.ShipHealthMax] <= 0)
            {
                stats[(int) Stats.ShipHealthMax] = 0;
            }

            if (stats[(int) Stats.ShipHealthCurrent] >= stats[(int) Stats.ShipHealthMax])
            {
                stats[(int) Stats.ShipHealthCurrent] = stats[(int) Stats.ShipHealthMax];
            }

            /*
            if (hullDurabilityRemainingAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Hull");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Hull");
            }
            */

            shipStatsUI.UpdateHullUI(stats[(int) Stats.ShipHealthCurrent], stats[(int) Stats.ShipHealthMax]);
            shipStatsUI.ShowHullUIChange((int)(value.x - prevValue.x), (int)(value.y - prevValue.y));
            
            if(GameManager.instance.currentGameState == InGameStates.Events && value.x - prevValue.x > 0)
            {
                EndingStats.instance.AddToStat((int)(value.x - prevValue.x), EndingStatTypes.HullDurability);
            }

            // check for death
            StartCoroutine(CheckDeathOnUnpause());
        }
    }

    public int[] StatsArray
    {
        get => stats;
        set
        {
            stats = value;

            shipStatsUI.UpdateCreditsUI(stats[(int) Stats.Credits], stats[(int) Stats.Payout]);
            shipStatsUI.UpdateEnergyUI(stats[(int)Stats.EnergyRemaining], stats[(int)Stats.EnergyUnassigned], stats[(int)Stats.EnergyMax]);
            shipStatsUI.UpdateSecurityUI(stats[(int) Stats.Security]);
            shipStatsUI.UpdateShipWeaponsUI(stats[(int) Stats.ShipWeapons]);
            shipStatsUI.UpdateCrewUI(stats[(int) Stats.CrewUnassigned], stats[(int) Stats.CrewCurrent], stats[(int) Stats.CrewCapacity]);
            shipStatsUI.UpdateFoodUI(stats[(int) Stats.Food], stats[(int) Stats.FoodPerTick], stats[(int) Stats.CrewCurrent]);
            shipStatsUI.UpdateHullUI(stats[(int) Stats.ShipHealthCurrent], stats[(int) Stats.ShipHealthMax]);
        }
    }

    private IEnumerator CheckDeathOnUnpause()
    {
        yield return new WaitUntil(() => tick.IsTickStopped());

        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (stats[(int) Stats.ShipHealthCurrent] <= 0)
        {
            if(DevelopmentAccess.instance.cheatModeActive && CheatsMenu.instance != null && CheatsMenu.instance.deathDisabled)
            {
                Debug.Log("Cheated Death");
            }
            else
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
                AudioManager.instance.PlaySFX("Hull Death");
                AudioManager.instance.PlayMusicWithTransition("Death Theme");
            }
        }
    }

    private void SetObjectBeingPlaced()
    {
        shipStatsUI.roomBeingPlaced = roomBeingPlaced;
    }

    public void CashPayout()
    {
        Credits += stats[(int) Stats.Payout];
        Payout = 0;
    }

    public bool HasEnoughPower(int power)
    {
        return Energy.y >= power;
    }

    public void PrintShipStats()
    {
        Debug.Log("Credits " + Credits);
        Debug.Log("Energy " + Energy);
        Debug.Log("Security " + Security);
        Debug.Log("ShipWeapons " + ShipWeapons);
        Debug.Log("CrewUnassigned " + CrewCurrent.z);
        Debug.Log("CrewCurrent " + CrewCurrent);
        Debug.Log("Food " + Food);
        Debug.Log("ShipHealthCurrent " + ShipHealthCurrent);
        Debug.Log("Payout " + Payout);
    }

    public void PayCrew(int amount)
    {
        Credits -= (amount * stats[(int) Stats.CrewCurrent]);
        MoraleManager.instance.CrewPayment(amount);
    }

    public void RemoveRandomCrew(int amount)
    {
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();
        int[] crewLost = new int[rooms.Length];
        int crewAssigned = stats[(int) Stats.CrewCurrent] - stats[(int) Stats.CrewUnassigned];

        for(int i = 0; i < amount; i++)
        {
            int selection = Mathf.FloorToInt(UnityEngine.Random.value * crewAssigned);
            if(selection == crewAssigned) // because Unity's Random.value includes 1, we have to do this
            {
                selection -= 1;
            }

            int index = 0;
            int crewChecked = 0;
            while(crewChecked <= selection)
            {
                crewChecked += rooms[index].currentCrew;

                if(crewChecked > selection)
                {
                    rooms[index].UpdateCurrentCrew(-1);
                    crewLost[index] += 1;
                    crewAssigned -= 1;
                }

                index += 1;
            }
        }

        for(int i = 0; i < crewLost.Length; i++)
        {
            if(crewLost[i] != 0)
            {
                rooms[i].SpawnStatChangeText(crewLost[i], GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon);
            }
        }
    }

    public void SaveShipStats()
    {
        SavingLoadingManager.instance.Save<int[]>("stats", stats);
    }

    public void LoadShipStats()
    {
        StatsArray = SavingLoadingManager.instance.Load<int[]>("stats");
    }

    public int[] GetCoreStats()
    {
        int[] coreStats = { stats[(int) Stats.CrewCapacity] - stats[(int) Stats.CrewCurrent], 
                            stats[(int) Stats.ShipHealthMax] - stats[(int) Stats.ShipHealthCurrent], 
                            stats[(int) Stats.Food], stats[(int) Stats.FoodPerTick], 
                            stats[(int) Stats.Security], stats[(int) Stats.ShipWeapons], 
                            stats[(int) Stats.Credits], stats[(int) Stats.EnergyRemaining] - stats[(int) Stats.EnergyUnassigned], 
                            stats[(int) Stats.CrewCurrent] };

        return coreStats;
    }

    /// <summary>
    /// returns the number of rooms on the ship that have the same name
    /// </summary>
    /// <param name="roomName"></param>
    /// <returns></returns>
    public int NumberOfRoomsOfType(string roomName)
    {
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();
        int count = 0;

        foreach (RoomStats room in rooms)
        {
            if(room.roomName == roomName)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Provides the number of staffed rooms at each level. Supply a saved variable for each level
    /// </summary>
    /// <param name="roomName">The room you would like to search for</param>
    /// <param name="level1">How many level 1 rooms there will be</param>
    /// <param name="level2">How many level 1 rooms there will be</param>
    /// <param name="level3">How many level 1 rooms there will be</param>
    public void RoomsOfTypeLevel(string roomName, int level1,int level2,int level3)
    {
        level1 = 0;
        level2 = 0;
        level3 = 0;
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();

        foreach (RoomStats room in rooms)
        {
            if (room.roomName == roomName && room.currentCrew >= room.minCrew)
            {
                switch(room.GetRoomLevel())
                {
                    case 1:
                        level1++;
                        break;
                    case 2:
                        level2++;
                        break;
                    case 3:
                        level3++;
                        break;

                }
            }
            
        }
    }

    public void ReAddPayoutFromRooms()
    {
        foreach (RoomStats room in FindObjectsOfType<RoomStats>())
        {
            if(room.resources[0].resourceType.Rt == ResourceDataTypes._Payout)
            {
                Payout += room.resources[0].activeAmount;
            }
        }
    }
}
