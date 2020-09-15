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

//[CreateAssetMenu(fileName = "new Ship Stats", menuName = "Ship Stats")]
public class ShipStats : MonoBehaviour
{
    [Header("Starting Ship Stats")]
    [SerializeField ,Tooltip("Starting amount of credits")]
    private int startingCredits;
    [SerializeField, Tooltip("Starting amount of energy")]
    private int startingEnergy;
    [SerializeField, Tooltip("Starting value of security")]
    private int startingSecurity;
    [SerializeField, Tooltip("Starting value of ship weapons")]
    private int startingShipWeapons;
    [SerializeField, Tooltip("Starting amount of crew")]
    private int startingCrew;
    [SerializeField, Tooltip("Starting amount of food")]
    private int startingFood;
    [SerializeField, Tooltip("Starting amount of ship health")]
    private int startingShipHealth;

    [Header("Ship Stats UI")]
    public TMP_Text creditsText;
    public TMP_Text energyText;
    public TMP_Text securityText;
    public TMP_Text shipWeaponsText;
    public TMP_Text crewText;
    public TMP_Text foodText;
    public TMP_Text shipHealthText;

    List<GameObject> rooms;

    int credits { get; set; }
    int energyMax;
    int energyRemaining;
    int security;
    int shipWeapons;
    int crewMax;
    int crewRemaining;
    int food;
    int foodPerTick;
    int shipHealthMax;
    int shipHealthCurrent;

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
        energyText.text = "Energy: " + energyRemaining.ToString() + "/" + energyMax.ToString();
        securityText.text = "Security: " + security.ToString();
        shipWeaponsText.text = "Ship Weapons: " + shipWeapons.ToString();
        crewText.text = "Crew: " + crewRemaining.ToString() + "/" + crewMax.ToString();
        foodText.text = "Food: " + food.ToString() + " + " + foodPerTick.ToString();
        shipHealthText.text = "Hull Durability: " + shipHealthCurrent.ToString() + "/" + shipHealthMax.ToString();
    }
}
