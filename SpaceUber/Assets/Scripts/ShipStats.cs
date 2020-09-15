/*
 * ShipStats.cs
 * Author(s): Grant Frey
 * Created on: 9/14/2020 (en-US)
 * Description: 
 */

using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "new Ship Stats", menuName = "Ship Stats")]
public class ShipStats : ScriptableObject
{
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
    }

    public void UpdateShipStatsUI()
    {

    }
}
