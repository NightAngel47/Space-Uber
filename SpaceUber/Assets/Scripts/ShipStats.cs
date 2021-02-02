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
using JetBrains.Annotations;
using NaughtyAttributes;
using TMPro;

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

    public GameObject cantPlaceText;
    public Sprite[] statIcons;

    private List<RoomStats> rooms;

    public GameObject roomBeingPlaced;

    private int credits;
    private int payout;
    private int crewPaymentDefault = 5;
    private int crewPaymentMoraleMultiplier = 10;
    private int energyMax;
    private int energyRemaining;
    private int security;
    private int shipWeapons;
    private int crewCapacity;
    private int crewCurrent;
    private int crewUnassigned;
    private int crewLossMoraleMultiplier = 10;
    private int food;
    private int foodPerTick;
    private int foodMoraleDamageMultiplier = 2;
    private int shipHealthMax;
    private int shipHealthCurrent;
    private int crewMorale;

    /// <summary>
    /// Reference to the ship stats UI class.
    /// </summary>
    private ShipStatsUI shipStatsUI;

    //tick variables
    private int secondsPerTick = 5;
    private bool ticksPaused;
    private bool tickStop = true;

    public int daysSince;
    [SerializeField] private TMP_Text daysSinceDisplay;

    //mutiny calculations
    private int maxMutinyMorale = 39;
    private float maxMutinyMoraleMutinyChance = 0.2f;
    private float zeroMoraleMutinyChance = 1f;

    //stats at the start of the job
    private int startCredits;
    private int startPayout;
    private int startEnergyMax;
    private int startEnergyRemaining;
    private int startSecurity;
    private int startShipWeapons;
    private int startCrewCapacity;
    private int startCrewCurrent;
    private int startCrewUnassigned;
    private int startFood;
    private int startFoodPerTick;
    private int startShipHealthMax;
    private int startShipHealthCurrent;
    private int startCrewMorale;

    private void Awake()
    {
        shipStatsUI = GetComponent<ShipStatsUI>();
    }

    private void Start()
    {
        UpdateCreditsAmount(startingCredits);
        UpdatePayoutAmount(0);
        UpdateEnergyAmount(startingEnergy, startingEnergy);
        UpdateSecurityAmount(startingSecurity);
        UpdateShipWeaponsAmount(startingShipWeapons);
        UpdateCrewAmount(startingCrew, startingCrew, startingCrew);
        UpdateFoodAmount(startingFood);
        UpdateHullDurabilityAmount(startingShipHealth, startingShipHealth);
        UpdateCrewMorale(startingMorale);
    }

    private IEnumerator TickUpdate()
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
            
            food += foodPerTick - crewCurrent;
            if(food < 0)
            {
                crewMorale += (food * foodMoraleDamageMultiplier);
                food = 0;
            }
            shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
            
            // increment days since events
            daysSince++;
            daysSinceDisplay.text = daysSince.ToString();
            
            if(crewMorale < 0)
            {
                crewMorale = 0;
            }
            
            shipStatsUI.UpdateCrewMoraleUI(crewMorale);
            
            float mutinyChance = (maxMutinyMorale - crewMorale) * zeroMoraleMutinyChance * (1 - maxMutinyMoraleMutinyChance) / maxMutinyMorale + maxMutinyMoraleMutinyChance;
            if(mutinyChance > UnityEngine.Random.value)
            {
                //replace this with event that gives a choice
                GameManager.instance.ChangeInGameState(InGameStates.Mutiny);
            }
            
            if(shipHealthCurrent <= 0)
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
        if(tickStop)
        {
            tickStop = false;
            ticksPaused = false;
            StartCoroutine(TickUpdate());
        }
    }

    public void ResetDaysSince()
    {
        daysSince = 0;
        daysSinceDisplay.text = daysSince.ToString();
    }

    private IEnumerator CheckDeathOnUnpause()
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

    public void SetObjectBeingPlaced()
    {
        shipStatsUI.roomBeingPlaced = roomBeingPlaced;
    }

    public void UpdateCreditsAmount(int creditAddition)
    {
        SetObjectBeingPlaced();
        credits += creditAddition;
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
        if(credits <= 0)
        {
            credits = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(creditAddition);
    }

    public void UpdateEnergyAmount(int energyRemainingAddition, int energyMaxAddition = 0)
    {
        energyMax += energyMaxAddition;
        energyRemaining += energyRemainingAddition;
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
        shipStatsUI.ShowEnergyUIChange(energyRemainingAddition, energyMaxAddition);
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;
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
        shipStatsUI.ShowSecurityUIChange(securityAmount);
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;
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
        shipStatsUI.ShowShipWeaponsUIChange(shipWeaponsAmount);
    }

    public void UpdateCrewAmount(int crewUnassignedAmount, int crewCurrentAmount = 0, int crewCapacityAmount = 0)
    {
        if(GameManager.instance.currentGameState == InGameStates.CrewManagement)
        {
            SetObjectBeingPlaced();
        }

        crewCapacity += crewCapacityAmount;
        
        if(crewUnassignedAmount > crewCapacity - crewCurrent)
        {
            crewUnassigned += crewCapacity - crewCurrent;
        }
        else
        {
            crewUnassigned += crewUnassignedAmount;
        }
        
        crewCurrent += crewCurrentAmount;

        if (crewCurrentAmount < 0)
        {
            if(crewUnassigned < 0)
            {
                RemoveRandomCrew(Mathf.Abs(crewUnassigned));
            }
            
            if(crewCurrentAmount < crewCapacityAmount)
            {
                UpdateCrewMorale(crewCurrentAmount * crewLossMoraleMultiplier, true);
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
        if (crewUnassigned >= crewCurrent)
        {
            crewUnassigned = crewCurrent;
        }

        shipStatsUI.UpdateCrewUI(crewUnassigned, crewCurrent, crewCapacity);
        shipStatsUI.ShowCrewUIChange(crewUnassignedAmount, crewCurrentAmount, crewCapacityAmount);
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;
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
        shipStatsUI.ShowFoodUIChange(foodAmount, 0);
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;

        shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
        shipStatsUI.ShowFoodUIChange(0, foodPerTickAmount);
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityRemainingAmount, int hullDurabilityMax = 0, bool checkImmediately = true)
    {
        shipHealthMax += hullDurabilityMax;
        shipHealthCurrent += hullDurabilityRemainingAmount;

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
        shipStatsUI.ShowHullUIChange(hullDurabilityRemainingAmount, hullDurabilityMax);

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
    
    public void UpdateCrewMorale(int crewMoraleAmount, bool hidden = false)
    {
        crewMorale += crewMoraleAmount;
        
        /*
        if (crewMoraleAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Morale");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Morale");
        }
        */
        
        if(crewMorale < 0)
        {
            crewMorale = 0;
        }
        
        if(crewMorale > 100)
        {
            crewMorale = 100;
        }
        
        shipStatsUI.UpdateCrewMoraleUI(crewMorale);
        shipStatsUI.ShowCrewMoraleUIChange(crewMoraleAmount, hidden);
    }

    //public int GetCredits()
    //{
    //    return credits;
    //}

    //public int GetRemainingCrew()
    //{
    //    return crewRemaining;
    //}
    public void UpdatePayoutAmount(int ammount)
    {
        int initialPayout = payout;
        payout += ammount;
        if (payout <= 0)
        {
            payout = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(0, payout - initialPayout);
    }

    public void MultiplyPayout(int multiplier)
    {
        int initialPayout = payout;
        payout *= multiplier;
        if (payout <= 0)
        {
            payout = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(0, payout - initialPayout);
    }

    public void CashPayout()
    {
        UpdateCreditsAmount(payout);
        payout = 0;
    }

    public bool HasEnoughPower(int power)
    {
        return EnergyRemaining >= power;
    }

    public int Credits
    {
        get { return credits; }
        set { credits = value; }
    }
    public int Payout
    {
        get { return payout; }
        set { payout = value; }
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
    public int CrewUnassigned
    {
        get { return crewUnassigned; }
        set { crewUnassigned = value; }
    }
    public int CrewCurrent
    {
        get { return crewCurrent; }
        set { crewCurrent = value; }
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

    public int Morale
    {
        get { return crewMorale; }
        set { crewMorale = value; }
    }

    public void PrintShipStats()
    {
        Debug.Log("Credits " + Credits);
        Debug.Log("Energy " + EnergyRemaining);
        Debug.Log("Security " + Security);
        Debug.Log("ShipWeapons " + ShipWeapons);
        Debug.Log("CrewUnassigned " + CrewUnassigned);
        Debug.Log("CrewCurrent " + CrewCurrent);
        Debug.Log("Food " + Food);
        Debug.Log("ShipHealthCurrent " + ShipHealthCurrent);
        Debug.Log("Payout " + Payout);
        Debug.Log("Morale " + Morale);
    }

    public void PayCrew(int amount)
    {
        UpdateCreditsAmount(-amount * crewCurrent);
        int BadMoraleMultiplier = (100 - crewMorale) * crewPaymentMoraleMultiplier / 100;
        UpdateCrewMorale(BadMoraleMultiplier * (amount - crewPaymentDefault), true);
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
                rooms[i].SpawnStatChangeText(crewLost[i], 4);
            }
        }
    }

    public void SaveStats()
    {
        startCredits = credits;
        startPayout = payout;
        startEnergyMax = energyMax;
        startEnergyRemaining = energyRemaining;
        startSecurity = security;
        startShipWeapons = shipWeapons;
        startCrewCapacity = crewCapacity;
        startCrewCurrent = crewCurrent;
        startCrewUnassigned = crewUnassigned;
        startFood = food;
        startFoodPerTick = foodPerTick;
        startShipHealthMax = shipHealthMax;
        startShipHealthCurrent = shipHealthCurrent;
        startCrewMorale = crewMorale;
    }

    public void ResetStats()
    {
        credits = 0;
        payout = 0;
        energyRemaining = 0;
        energyMax = 0;
        security = 0;
        shipWeapons = 0;
        crewUnassigned = 0;
        crewCurrent = 0;
        crewCapacity = 0;
        food = 0;
        foodPerTick = 0;
        shipHealthCurrent = 0;
        shipHealthMax = 0;
        crewMorale = 0;
        
        UpdateCreditsAmount(startCredits);
        UpdatePayoutAmount(startPayout);
        UpdateEnergyAmount(startEnergyRemaining, startEnergyMax);
        UpdateSecurityAmount(startSecurity);
        UpdateShipWeaponsAmount(startShipWeapons);
        UpdateCrewAmount(startCrewUnassigned, startCrewCurrent, startCrewCapacity);
        UpdateFoodAmount(startFood);
        UpdateFoodPerTickAmount(startFoodPerTick);
        UpdateHullDurabilityAmount(startShipHealthCurrent, startShipHealthMax);
        UpdateCrewMorale(startCrewMorale);
    }
}
