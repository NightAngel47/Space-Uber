/*
 * ShipStats.cs
 * Author(s): Grant Frey
 * Created on: 9/14/2020 (en-US)
 * Description:
 */

using System;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class ShipStats : MonoBehaviour
{
    public enum resources{ Credits, Energy, Security, ShipWeapons, Crew, Food, FoodPerTick, HullDurability, Stock}

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
    [SerializeField, Tooltip("Starting amount of crewMorale"), Foldout("Starting Ship Stats")]
    private int startingMorale;

    private List<RoomStats> rooms;

    private int credits;
    private int payout;
    private int crewPaymentDefault = 5;
    private int crewPaymentMoraleMultiplier = 10;
    private int energyMax;
    private int energyRemaining;
    private int security;
    private int shipWeapons;
    private int crewMax;
    private int crewRemaining;
    private int food;
    private int foodPerTick;
    private int foodMoraleDamageMultiplier = 2;
    private int shipHealthMax;
    private int shipHealthCurrent;
    //private int crewMorale;

    /// <summary>
    /// Reference to the ship stats UI class.
    /// </summary>
    private ShipStatsUI shipStatsUI;

    //tick variables
    private int secondsPerTick = 5;
    private bool ticksPaused;
    private bool tickStop = true;

    //mutiny calculations
    private int maxMutinyMorale = 60;
    private float zeroMoraleMutinyChance = 0.75f;
    
    //stats at the start of the job
    private int startCredits;
    private int startPayout;
    private int startEnergyMax;
    private int startEnergyRemaining;
    private int startSecurity;
    private int startShipWeapons;
    private int startCrewMax;
    private int startCrewRemaining;
    private int startFood;
    private int startFoodPerTick;
    private int startShipHealthMax;
    private int startShipHealthCurrent;
    //private int startCrewMorale;

    private void Awake()
    {
        shipStatsUI = GetComponent<ShipStatsUI>();
    }

    private void Start()
    {
        UpdateCreditsAmount(startingCredits);
        payout = 0;
        UpdateEnergyAmount(startingEnergy, startingEnergy);
        UpdateSecurityAmount(startingSecurity);
        UpdateShipWeaponsAmount(startingShipWeapons);
        UpdateCrewAmount(startingCrew, startingCrew);
        UpdateFoodAmount(startingFood);
        UpdateHullDurabilityAmount(startingShipHealth, startingShipHealth);
        //UpdateCrewMorale(startingMorale);
    }

    private IEnumerator<YieldInstruction> TickUpdate()
    {
        while(!tickStop)
        {
            while(ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(secondsPerTick);

            while(ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitWhile(() => ticksPaused);

            food += foodPerTick - crewRemaining;
            if(food < 0)
            {
                //crewMorale += (food * foodMoraleDamageMultiplier);
                food = 0;
            }

            //if(crewMorale < 0)
            //{
            //    crewMorale = 0;
            //}

            shipStatsUI.UpdateFoodUI(food, foodPerTick);
            //UpdateMoraleShipStatsUI();

            //float mutinyChance = (maxMutinyMorale - crewMorale) * zeroMoraleMutinyChance / maxMutinyMorale;
            //if(mutinyChance > UnityEngine.Random.value)
            //{
            //    GameManager.instance.ChangeInGameState(InGameStates.Mutiny);
            //}

            if(shipHealthCurrent <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
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
        if(tickStop)
        {
            tickStop = false;
            ticksPaused = false;
            StartCoroutine(TickUpdate());
        }
    }
    
    private IEnumerator<YieldInstruction> CheckDeathOnUnpause()
    {
            while(ticksPaused || tickStop)
            {
                yield return new WaitForFixedUpdate();
            }

            if(shipHealthCurrent <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
            }
    }

    public void UpdateCreditsAmount(int creditAddition)
    {
        credits += creditAddition;
        if(credits <= 0)
        {
            credits = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits);
    }

    public void UpdateEnergyAmount(int energyRemainingAddition, int energyMaxAddition = 0)
    {
        energyMax += energyMaxAddition;
        energyRemaining += energyRemainingAddition;

        if (energyRemaining <= 0)
        {
            energyRemaining = 0;
        }
        if (energyRemaining >= energyMax)
        {
            energyRemaining = energyMax;
        }

        shipStatsUI.UpdateEnergyUI(energyRemaining, energyMax);
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;

        if (security <= 0)
        {
            security = 0;
        }

        shipStatsUI.UpdateSecurityUI(security);
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;

        if (shipWeapons <= 0)
        {
            shipWeapons = 0;
        }

        shipStatsUI.UpdateShipWeaponsUI(shipWeapons);
    }

    public void UpdateCrewAmount(int crewRemainingAmount, int crewMaxAmount = 0)
    {
        crewMax += crewMaxAmount;
        crewRemaining += crewRemainingAmount;

        if (crewRemaining <= 0)
        {
            crewRemaining = 0;
        }
        if (crewRemaining >= crewMax)
        {
            crewRemaining = crewMax;
        }

        shipStatsUI.UpdateCrewUI(crewRemaining, crewMax);
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;

        if (food <= 0)
        {
            food = 0;
        }

        shipStatsUI.UpdateFoodUI(food, foodPerTick);
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;

        shipStatsUI.UpdateFoodUI(food, foodPerTick);
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityRemainingAmount, int hullDurabilityMax = 0, bool checkImmediately = true)
    {
        shipHealthMax += hullDurabilityMax;
        shipHealthCurrent += hullDurabilityRemainingAmount;

        shipStatsUI.UpdateHullUI(shipHealthCurrent, shipHealthMax);
        
        if(checkImmediately)
        {
            if(shipHealthCurrent <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
            }
        }
        else
        {
            CheckDeathOnUnpause();
        }
    }

    //public int GetCredits()
    //{
    //    return credits;
    //}

    //public int GetRemainingCrew()
    //{
    //    return crewRemaining;
    //}
    public void AddPayout(int ammount)
    {
        payout += ammount;
        if (payout <= 0)
        {
            payout = 0;
        }
    }

    public void MultiplyPayout(int multiplier)
    {
        payout *= multiplier;
        if (payout <= 0)
        {
            payout = 0;
        }
    }

        public void CashPayout()
    {
        UpdateCreditsAmount(payout);
        payout = 0;
    }

    public bool HasEnoughPower()
    {
        return EnergyRemaining >= 0;
    }

    public int Credits
    {
        get { return credits; }
        set { credits = value; }
    }
    public int EnergyRemaining
    {
        get { return energyRemaining; }
        set { energyRemaining = value; }
    }
    public int Security
    {
        get { return security; }
        set { security = value; }
    }
    public int ShipWeapons
    {
        get { return shipWeapons; }
        set { shipWeapons = value; }
    }
    public int CrewRemaining
    {
        get { return crewRemaining; }
        set { crewRemaining = value; }
    }
    public int Food
    {
        get { return food; }
        set { food = value; }
    }
    public int ShipHealthCurrent
    {
        get { return shipHealthCurrent; }
        set { shipHealthCurrent = value; }
    }
    public int Payout
    {
        get { return payout; }
        set { payout = value; }
    }

    //public void UpdateCrewMorale(int crewMoraleAmount)
    //{
    //    crewMorale += crewMoraleAmount;
    //    if(crewMorale < 0)
    //    {
    //        crewMorale = 0;
    //    }
    //    // TODO update to work with changes from development
    //    //UpdateShipStatsUI();
    //}

    public void PrintShipStats()
    {
        Debug.Log("Credits " + Credits);
        Debug.Log("Energy " + EnergyRemaining);
        Debug.Log("Security " + Security);
        Debug.Log("ShipWeapons " + ShipWeapons);
        Debug.Log("CrewRemaining " + CrewRemaining);
        Debug.Log("Food " + Food); 
        Debug.Log("ShipHealthCurrent " + ShipHealthCurrent);
        Debug.Log("Payout " + Payout);
    }
    
    public void PayCrew(int ammount)
    {
        UpdateCreditsAmount(-ammount * crewRemaining);
        //int BadMoraleMultiplier = (maxMutinyMorale - crewMorale) * crewPaymentMoraleMultiplier / maxMutinyMorale;
        //UpdateCrewMorale(BadMoraleMultiplier * (ammount - crewPaymentDefault));
    }
    
    public void SaveStats()
    {
        startCredits = credits;
        startPayout = payout;
        startEnergyMax = energyMax;
        startEnergyRemaining = energyRemaining;
        startSecurity = security;
        startShipWeapons = shipWeapons;
        startCrewMax = crewMax;
        startCrewRemaining = crewRemaining;
        startFood = food;
        startFoodPerTick = foodPerTick;
        startShipHealthMax = shipHealthMax;
        startShipHealthCurrent = shipHealthCurrent;
        //startCrewMorale = crewMorale;
    }
    
    public void ResetStats()
    {
        UpdateCreditsAmount(startCredits);
        payout = startPayout;
        UpdateEnergyAmount(startEnergyRemaining, startEnergyMax);
        UpdateSecurityAmount(startSecurity);
        UpdateShipWeaponsAmount(startShipWeapons);
        UpdateCrewAmount(startCrewRemaining, startCrewMax);
        UpdateFoodAmount(startFood);
        UpdateFoodPerTickAmount(startFoodPerTick);
        UpdateHullDurabilityAmount(startShipHealthCurrent, startShipHealthMax);
        //UpdateCrewMorale(startMorale);
    }
}
