using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStatsInterface : MonoBehaviour
{
    private ShipStats ship;
    
    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
    }
    
    public void PauseTickEvents()
    {
        ship.PauseTickEvents();
    }
    
    public void UnpauseTickEvents()
    {
        ship.UnpauseTickEvents();
    }
    
    public void StopTickEvents()
    {
        ship.StopTickEvents();
    }
    
    public void StartTickEvents()
    {
        ship.StartTickEvents();
    }
    
    public void UpdateCreditsAmount(int creditAmount)
    {
        ship.UpdateCreditsAmount(creditAmount);
    }
    
    public void UpdateRemainingEnergyAmount(int energyRemainingAmount)
    {
        ship.UpdateEnergyAmount(energyRemainingAmount);
    }
    
    public void UpdateMaxEnergyAmount(int energyMaxAmount)
    {
        ship.UpdateEnergyAmount(0, energyMaxAmount);
    }
    
    public void UpdateSecurityAmount(int securityAmount)
    {
        ship.UpdateSecurityAmount(securityAmount);
    }
    
    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        ship.UpdateShipWeaponsAmount(shipWeaponsAmount);
    }
    
    public void UpdateRemainingCrewAmount(int crewRemainingAmount)
    {
        ship.UpdateCrewAmount(crewRemainingAmount);
    }
    
    public void UpdateMaxCrewAmount(int crewMaxAmount)
    {
        ship.UpdateCrewAmount(0, crewMaxAmount);
    }
    
    public void UpdateFoodAmount(int foodAmount)
    {
        ship.UpdateFoodAmount(foodAmount);
    }
    
    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        ship.UpdateFoodPerTickAmount(foodPerTickAmount);
    }
    
    public void UpdateRemainingHullDurabilityAmount(int hullDurabilityRemainingAmount)
    {
        ship.UpdateHullDurabilityAmount(hullDurabilityRemainingAmount);
    }
    
    public void UpdateMaxHullDurabilityAmount(int hullDurabilityMax)
    {
        ship.UpdateHullDurabilityAmount(0, hullDurabilityMax);
    }
    
    public int GetCredits()
    {
        return ship.GetCredits();
    }
    
    public void AddPayout(int ammount)
    {
        ship.AddPayout(ammount);
    }
    
    public void CashPayout()
    {
        ship.CashPayout();
    }
    
    public bool HasEnoughPower()
    {
        return ship.HasEnoughPower();
    }
    
    // not sure how these things work
    /*
    public int Credits { get; set; }
    public int EnergyRemaining { get; set; }
    public int Security { get; set; }
    public int ShipWeapons { get; set; }
    public int CrewRemaining { get; set; }
    public int Food { get; set; }
    public int ShipHealthCurrent { get; set; }
     */
    
    //public void UpdateCrewMorale(int crewMoraleAmount)
    //{
    //    ship.UpdateCrewMorale(crewMoraleAmount);
    //}
    
    public void PayCrew(int ammount)
    {
        ship.PayCrew(ammount);
    }
}
