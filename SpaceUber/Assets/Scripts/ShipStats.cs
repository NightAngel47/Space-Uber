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

//[CreateAssetMenu(fileName = "new Ship Stats", menuName = "Ship Stats")]
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

    private List<RoomStats> rooms;

    private int credits;
    private int energyMax;
    private int energyRemaining;
    private int security;
    private int shipWeapons;
    private int crewMax;
    private int crewRemaining;
    private int food;
    private int foodPerTick;
    private int shipHealthMax;
    private int shipHealthCurrent;
    private int crewMorale;

    /// <summary>
    /// Reference to the ship stats UI class.
    /// </summary>
    private ShipStatsUI shipStatsUI;

    private void Awake()
    {
        shipStatsUI = GetComponent<ShipStatsUI>();
    }

    private void Start()
    {
        UpdateCreditsAmount(startingCredits);
        UpdateEnergyAmount(startingEnergy, startingEnergy);
        UpdateSecurityAmount(startingSecurity);
        UpdateShipWeaponsAmount(startingShipWeapons);
        UpdateCrewAmount(startingCrew, startingCrew);
        UpdateFoodAmount(startingFood);
        UpdateHullDurabilityAmount(startingShipHealth, startingShipHealth);
    }

    public void UpdateCreditsAmount(int creditAmount)
    {
        credits += creditAmount;

        shipStatsUI.UpdateCreditsUI(credits);
    }

    public void UpdateEnergyAmount(int energyRemainingAmount, int energyMaxAmount = 0)
    {
        energyMax += energyMaxAmount;
        energyRemaining += energyRemainingAmount;

        shipStatsUI.UpdateEnergyUI(energyRemaining, energyMax);
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;

        shipStatsUI.UpdateSecurityUI(security);
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;

        shipStatsUI.UpdateShipWeaponsUI(shipWeapons);
    }

    public void UpdateCrewAmount(int crewRemainingAmount, int crewMaxAmount = 0)
    {
        crewMax += crewMaxAmount;
        crewRemaining += crewRemainingAmount;

        shipStatsUI.UpdateCrewUI(crewRemaining, crewMax);
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;

        shipStatsUI.UpdateFoodUI(food, foodPerTick);
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;

        shipStatsUI.UpdateFoodUI(food, foodPerTick);
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityRemainingAmount, int hullDurabilityMax = 0)
    {
        shipHealthMax += hullDurabilityMax;
        shipHealthCurrent += hullDurabilityRemainingAmount;

        shipStatsUI.UpdateHullUI(shipHealthCurrent, shipHealthMax);
    }

    public int GetCredits()
    {
        return credits;
    }

    public int Credits { get; set; }
    public int EnergyRemaining { get; set; }
    public int Security { get; set; }
    public int ShipWeapons { get; set; }
    public int CrewRemaining { get; set; }
    public int Food { get; set; }
    public int ShipHealthCurrent { get; set; }

    public void UpdateCrewMorale(int crewMoraleAmount)
    {
        crewMorale += crewMoraleAmount;
        // TODO update to work with changes from development
        //UpdateShipStatsUI();
    }
}
