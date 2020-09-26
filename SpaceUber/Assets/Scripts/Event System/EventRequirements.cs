/*
 * EventRequirements.cs
 * Author(s): Scott Acker
 * Created on: 9/25/2020 
 * Description: Stores information about the requirements for each job
 */

using UnityEngine;

public class EventRequirements : MonoBehaviour
{
    [Header("Stat Requirements")]
    
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int hullRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")]
    public int energyRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int crewRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int foodRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int weaponsRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int securityRequired;
    [Tooltip("Positive to check if greater than x, negative to check if less than x")] 
    public int creditsRequired;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool MatchesRequirements(ShipStats thisShip)
    {

        //test variables
        bool hullMatch;
        if (hullRequired > 0)
        {
            hullMatch = (thisShip.ShipHealthCurrent > hullRequired);
        }
        else
        {
            hullMatch = (thisShip.ShipHealthCurrent < hullRequired);
        }

        bool energyMatch;
        if (energyRequired > 0)
        {
            energyMatch = (thisShip.EnergyRemaining > energyRequired);
        }
        else
        {
            energyMatch = (thisShip.EnergyRemaining < energyRequired);
        }

        bool crewMatch;
        if (crewRequired > 0)
        {
            crewMatch = (thisShip.CrewRemaining > crewRequired);
        }
        else
        {
            crewMatch = (thisShip.CrewRemaining < crewRequired);
        }

        bool foodMatch;
        if (hullRequired > 0)
        {
            foodMatch = (thisShip.Food > foodRequired);
        }
        else
        {
            foodMatch = (thisShip.Food < foodRequired);
        }

        bool weaponsMatch;
        if (weaponsRequired > 0)
        {
            weaponsMatch = (thisShip.ShipWeapons > weaponsRequired);
        }
        else
        {
            weaponsMatch = (thisShip.ShipWeapons < weaponsRequired);
        }

        bool securityMatch;
        if (securityRequired > 0)
        {
            securityMatch = (thisShip.Security > securityRequired);
        }
        else
        {
            securityMatch = (thisShip.Security < securityRequired);
        }

        bool creditsMatch;
        if (creditsRequired > 0)
        {
            creditsMatch = (thisShip.Credits > creditsRequired);
        }
        else
        {
            creditsMatch = (thisShip.Credits < creditsRequired);
        }

        bool result = hullMatch && energyMatch && crewMatch && foodMatch && weaponsMatch && securityMatch && creditsMatch;
        return result;
    }
}
