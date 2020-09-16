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

    [OnValueChanged("UpdateShipStatsUI")]
    private int credits;
    [OnValueChanged("UpdateShipStatsUI")]
    private int energyMax;
    [OnValueChanged("UpdateShipStatsUI")]
    private int energyRemaining;
    [OnValueChanged("UpdateShipStatsUI")]
    private int security;
    [OnValueChanged("UpdateShipStatsUI")]
    private int shipWeapons;
    [OnValueChanged("UpdateShipStatsUI")]
    private int crewMax;
    [OnValueChanged("UpdateShipStatsUI")]
    private int crewRemaining;
    [OnValueChanged("UpdateShipStatsUI")]
    private int food;
    [OnValueChanged("UpdateShipStatsUI")]
    private int foodPerTick;
    [OnValueChanged("UpdateShipStatsUI")]
    private int shipHealthMax;
    [OnValueChanged("UpdateShipStatsUI")]
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
    }

    public void UpdateEnergyAmount(int energyAmount)
    {
        energyMax += energyAmount;
        energyRemaining += energyAmount;
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;
    }

    public void UpdateCrewAmount(int crewAmount)
    {
        crewMax += crewAmount;
        crewRemaining += crewAmount;
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityAmount)
    {
        shipHealthMax += hullDurabilityAmount;
        shipHealthCurrent += hullDurabilityAmount;
    }
}
