/*
 * ShipStats.cs
 * Author(s): Grant Frey
 * Created on: 9/14/2020 (en-US)
 * Description: 
 */

using System;
using UnityEngine;
using TMPro;
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

    [Foldout("Ship Stats UI Objects")]
    public TMP_Text creditsText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text energyText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text securityText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text shipWeaponsText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text crewText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text foodText;
    [Foldout("Ship Stats UI Objects")]
    public TMP_Text shipHealthText;

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

    void Start()
    {
        credits = startingCredits;
        energyMax = startingEnergy;
        energyRemaining = startingEnergy;
        security = startingSecurity;
        shipWeapons = startingShipWeapons;
        crewMax = startingCrew;
        crewRemaining = startingCrew;
        food = startingFood;
        foodPerTick = 0;
        shipHealthMax = startingShipHealth;
        shipHealthCurrent = startingShipHealth;

        UpdateShipStatsUI();
    }

    public void UpdateShipStatsUI()
    {
        creditsText.text = "Credits: " + credits.ToString();
        energyText.text = "Energy: " + energyRemaining.ToString() + " / " + energyMax.ToString();
        securityText.text = "Security: " + security.ToString();
        shipWeaponsText.text = "Ship Weapons: " + shipWeapons.ToString();
        crewText.text = "Crew: " + crewRemaining.ToString() + " / " + crewMax.ToString();
        foodText.text = "Food: " + food.ToString() + " + " + foodPerTick.ToString();
        shipHealthText.text = "Hull Durability: " + shipHealthCurrent.ToString() + " / " + shipHealthMax.ToString();
    }

    public void UpdateCreditsAmount(int creditAmount)
    {
        credits += creditAmount;
        UpdateShipStatsUI();
    }

    public void UpdateEnergyAmount(int energyRemainingAmount, int energyMaxAmount = 0)
    {
        energyMax += energyMaxAmount;
        energyRemaining += energyRemainingAmount;
        UpdateShipStatsUI();
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;
        UpdateShipStatsUI();
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;
        UpdateShipStatsUI();
    }

    public void UpdateCrewAmount(int crewRemainingAmount, int crewMaxAmount = 0)
    {
        crewMax += crewMaxAmount;
        crewRemaining += crewRemainingAmount;
        UpdateShipStatsUI();
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;
        UpdateShipStatsUI();
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;
        UpdateShipStatsUI();
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityRemainingAmount, int hullDurabilityMax = 0)
    {
        shipHealthMax += hullDurabilityMax;
        shipHealthCurrent += hullDurabilityRemainingAmount;
        UpdateShipStatsUI();
    }
}
