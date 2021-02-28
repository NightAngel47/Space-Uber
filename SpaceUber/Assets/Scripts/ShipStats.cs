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

    [HideInInspector]
    public CharacterStats cStats;

    public GameObject cantPlaceText;

    private List<RoomStats> rooms;

    [HideInInspector] public GameObject roomBeingPlaced;

    private int credits;
    private int payout;
    private int energyMax;
    private int energyRemaining;
    private int security;
    private int shipWeapons;
    private int crewCapacity;
    private int crewCurrent;
    private int crewUnassigned;
    private int food;
    private int foodPerTick;
    private int shipHealthMax;
    private int shipHealthCurrent;

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
    }

    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetStats();
        }
        else
        {
            Credits = startingCredits;
            Payout = 0;
            EnergyRemaining = new Vector2(startingEnergy, startingEnergy);
            Security = startingSecurity;
            ShipWeapons = startingShipWeapons;
            CrewCurrent = new Vector3(startingCrew, startingCrew, startingCrew);
            Food = startingFood;
            FoodPerTick = 0;
            ShipHealthCurrent = new Vector2(startingShipHealth, startingShipHealth);
            
            SaveStats();
        }

        cStats = gameObject.GetComponent<CharacterStats>();
    }

    /// <summary>
    /// Property for Credits. Getter and setter
    /// </summary>
    public int Credits
    {
        get => credits;
        set
        {
            int prevValue = credits;
            SetObjectBeingPlaced();
            credits = value;
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
            if (credits <= 0)
            {
                credits = 0;
            }

            shipStatsUI.UpdateCreditsUI(credits, payout);
            shipStatsUI.ShowCreditsUIChange(value - prevValue);
        }
    }

    /// <summary>
    /// Property for Payout. Getter and Setter
    /// </summary>
    public int Payout
    {
        get => payout;
        set
        {
            int initialPayout = payout;
            payout = value;

            if (payout <= 0)
            {
                payout = 0;
            }

            shipStatsUI.UpdateCreditsUI(credits, payout);
            shipStatsUI.ShowCreditsUIChange(0, payout - initialPayout);
        }
    }

    /// <summary>
    /// Property for Energy. Getter and Setter. Vector2 x is energyRemaining and y is energyMax
    /// </summary>
    public Vector2 EnergyRemaining // x = energyRemaining y = energyMax
    {
        get => new Vector2(energyRemaining, energyMax);
        set
        {
            Vector2 prevValue = new Vector2(energyRemaining, energyMax);
            energyMax = (int)value.y;
            energyRemaining = (int)value.x;
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
            if (energyRemaining <= 0)
            {
                energyRemaining = 0;
            }
            if (energyRemaining >= energyMax)
            {
                energyRemaining = energyMax;
            }

            shipStatsUI.UpdateEnergyUI(energyRemaining, energyMax);
            shipStatsUI.ShowEnergyUIChange((int)(value.x - prevValue.x), (int)(value.y - prevValue.y));
        }
    }

    /// <summary>
    /// Property for Security. Getter and Setter
    /// </summary>
    public int Security
    {
        get => security;
        set
        {
            int prevValue = security;
            security = value;
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
            if (security <= 0)
            {
                security = 0;
            }

            shipStatsUI.UpdateSecurityUI(security);
            shipStatsUI.ShowSecurityUIChange(value - prevValue);
        }
    }

    /// <summary>
    /// Property for ShipWeapons. Getter and Setter
    /// </summary>
    public int ShipWeapons
    {
        get => shipWeapons;
        set
        {
            int prevValue = shipWeapons;
            shipWeapons = value;
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
            if (shipWeapons <= 0)
            {
                shipWeapons = 0;
            }

            shipStatsUI.UpdateShipWeaponsUI(shipWeapons);
            shipStatsUI.ShowShipWeaponsUIChange(value - prevValue);
        }
    }

    /// <summary>
    /// Property for CrewCurrent. Getter for crewCurrent and setter for crewCurrent, crewCapacity, and crewUnassigned. x = crewCurrent, y = crewCapacity, z = crewUnnassigned
    /// </summary>
    public Vector3 CrewCurrent //x = crewCurrent y = crewCapacity z = crewUnnassigned
    {
        get => new Vector3(crewCurrent, crewCapacity, crewUnassigned);
        set
        {
            if (GameManager.instance.currentGameState == InGameStates.CrewManagement)
            {
                SetObjectBeingPlaced();
            }

            Vector3 prevValue = new Vector3(crewCurrent, crewCapacity, crewUnassigned);
            crewCurrent = (int)value.x;
            crewCapacity = (int)value.y;
            crewUnassigned = (int)value.z;

            if (crewCurrent - prevValue.x < 0)
            {
                if(crewUnassigned < 0)
                {
                    RemoveRandomCrew(Mathf.Abs(crewUnassigned));
                }

                if(crewCurrent - prevValue.x < crewCapacity - prevValue.y)
                {
                    MoraleManager.instance.CrewLoss((int)(crewCurrent - prevValue.x));
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

            if (crewCurrent <= 0)
            {
                crewCurrent = 0;
            }
            if (crewCurrent >= crewCapacity)
            {
                crewCurrent = crewCapacity;
            }

            if (crewUnassigned <= 0)
            {
                crewUnassigned = 0;
            }

            shipStatsUI.UpdateCrewUI(crewUnassigned, crewCurrent, crewCapacity);
            shipStatsUI.ShowCrewUIChange((int)(value.z - prevValue.z), (int)(value.x - prevValue.x), (int)(value.y - prevValue.y));
        }
    }

    /// <summary>
    /// Property for Food. Getter and Setter
    /// </summary>
    public int Food
    {
        get => food;
        set
        {
            int prevValue = food;
            food = value;
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
            if (food <= 0)
            {
                food = 0;
            }

            shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
            shipStatsUI.ShowFoodUIChange(value - prevValue, 0);
        }
    }

    /// <summary>
    /// Property for FoodPerTick. Getter and Setter
    /// </summary>
    public int FoodPerTick
    {
        get => foodPerTick;
        set
        {
            int prevValue = foodPerTick;
            foodPerTick = value;

            shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
            shipStatsUI.ShowFoodUIChange(0, value - prevValue);
        }
    }

    /// <summary>
    /// Property for shipHealthCurrent. Getter and Setter, sets the shipHealth max as well. x = shipHealthCurrent, y = shipHealthMax
    /// </summary>
    public Vector2 ShipHealthCurrent //x = shipHealthCurrent y = shipHealthMax
    {
        get => new Vector2(shipHealthCurrent, shipHealthMax);
        set
        {
            Vector2 prevValue = new Vector2(shipHealthCurrent, shipHealthMax);
            shipHealthMax = (int)value.y;
            shipHealthCurrent = (int)value.x;

            if (shipHealthCurrent >= shipHealthMax)
            {
                shipHealthCurrent = shipHealthMax;
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

            shipStatsUI.UpdateHullUI(shipHealthCurrent, shipHealthMax);
            shipStatsUI.ShowHullUIChange((int)(value.x - prevValue.x), (int)(value.y - prevValue.y));

            // check for death
            StartCoroutine(CheckDeathOnUnpause());
        }
    }

    private IEnumerator CheckDeathOnUnpause()
    {
        yield return new WaitUntil(() => tick.IsTickStopped());

        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (shipHealthCurrent <= 0)
        {
            GameManager.instance.ChangeInGameState(InGameStates.Death);
        }
    }

    private void SetObjectBeingPlaced()
    {
        shipStatsUI.roomBeingPlaced = roomBeingPlaced;
    }

    public void CashPayout()
    {
        Credits += payout;
        Payout = 0;
    }

    public bool HasEnoughPower(int power)
    {
        return EnergyRemaining.x >= power;
    }

    public void PrintShipStats()
    {
        Debug.Log("Credits " + Credits);
        Debug.Log("Energy " + EnergyRemaining);
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
        Credits -= (amount * crewCurrent);
        MoraleManager.instance.CrewPayment(amount);
    }

    public void RemoveRandomCrew(int amount)
    {
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();
        int[] crewLost = new int[rooms.Length];
        int crewAssigned = crewCurrent - crewUnassigned;

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

    public void SaveStats()
    {
        SavingLoadingManager.instance.Save<int>("credits", credits);
        SavingLoadingManager.instance.Save<int>("payout", payout);
        SavingLoadingManager.instance.Save<int>("energyMax", energyMax);
        SavingLoadingManager.instance.Save<int>("energyRemaining", energyRemaining);
        SavingLoadingManager.instance.Save<int>("security", security);
        SavingLoadingManager.instance.Save<int>("shipWeapons", shipWeapons);
        SavingLoadingManager.instance.Save<int>("crewCapacity", crewCapacity);
        SavingLoadingManager.instance.Save<int>("crewCurrent", crewCurrent);
        SavingLoadingManager.instance.Save<int>("crewUnassigned", crewUnassigned);
        SavingLoadingManager.instance.Save<int>("food", food);
        SavingLoadingManager.instance.Save<int>("foodPerTick", foodPerTick);
        SavingLoadingManager.instance.Save<int>("shipHealthMax", shipHealthMax);
        SavingLoadingManager.instance.Save<int>("shipHealthCurrent", shipHealthCurrent);
    }

    public void ResetStats()
    {
        Credits = SavingLoadingManager.instance.Load<int>("credits");
        Payout = SavingLoadingManager.instance.Load<int>("payout");
        EnergyRemaining = new Vector2(SavingLoadingManager.instance.Load<int>("energyRemaining"), SavingLoadingManager.instance.Load<int>("energyMax"));
        Security = SavingLoadingManager.instance.Load<int>("security");
        ShipWeapons = SavingLoadingManager.instance.Load<int>("shipWeapons");
        CrewCurrent = new Vector3(SavingLoadingManager.instance.Load<int>("crewCurrent"), SavingLoadingManager.instance.Load<int>("crewCapacity"), SavingLoadingManager.instance.Load<int>("crewUnassigned"));
        Food = SavingLoadingManager.instance.Load<int>("food");
        FoodPerTick = SavingLoadingManager.instance.Load<int>("foodPerTick");
        ShipHealthCurrent = new Vector2(SavingLoadingManager.instance.Load<int>("shipHealthCurrent"), SavingLoadingManager.instance.Load<int>("shipHealthMax"));
    }
}
