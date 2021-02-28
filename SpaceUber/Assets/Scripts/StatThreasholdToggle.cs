using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatThreasholdToggle : MonoBehaviour
{
    [SerializeField] private ResourceDataTypes stat;
    [SerializeField] private int threashold;
    [SerializeField] private GameObject onBelowThreashold;
    [SerializeField] private GameObject onAboveThreashold;
    
    private ShipStats ship;
    
    void Start()
    {
        ship = FindObjectOfType<ShipStats>();
        
        int shipStat = 0;
        switch (stat)
        {
            case ResourceDataTypes._HullDurability:
                shipStat = (int)ship.ShipHealthCurrent.x;
                break;
            case ResourceDataTypes._Energy:
                shipStat = (int)ship.EnergyRemaining.x;
                break;
            case ResourceDataTypes._Crew:
                shipStat = (int)ship.CrewCurrent.x;
                break;
            case ResourceDataTypes._Food:
                shipStat = ship.Food;
                break;
            case ResourceDataTypes._FoodPerTick:
                shipStat = ship.FoodPerTick;
                break;
            case ResourceDataTypes._ShipWeapons:
                shipStat = ship.ShipWeapons;
                break;
            case ResourceDataTypes._Security:
                shipStat = ship.Security;
                break;
            case ResourceDataTypes._CrewMorale:
                shipStat = MoraleManager.instance.CrewMorale;
                break;
            case ResourceDataTypes._Credits:
                shipStat = ship.Credits;
                break;
            case ResourceDataTypes._Payout:
                shipStat = ship.Payout;
                break;
        }
        
        onBelowThreashold.SetActive(shipStat < threashold);
        onAboveThreashold.SetActive(shipStat >= threashold);
    }
}
