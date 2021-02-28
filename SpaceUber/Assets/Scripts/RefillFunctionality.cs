using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillFunctionality : MonoBehaviour
{
    [SerializeField] private float costForCrew = 0;
    [SerializeField] private float costForHullDurability = 0;

    /// <summary>
    /// How much of the Hull Damage is the player incrementing
    /// </summary>
    [SerializeField] private int hullIncrement = 0;

    private ShipStats shipStats;

    private int hullDamage = 0;
    private int priceForHullRepair = 0;

    private int crewLost = 0;
    private int priceForCrewReplacement = 0;

    public void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();
    }

    public void RefillHullDurability()
    {
        if(shipStats.Credits >= priceForHullRepair)
        {
            shipStats.Credits += -priceForHullRepair;
            shipStats.ShipHealthCurrent += new Vector2(hullDamage, 0);
        }
    }

    public void RefillCrew()
    {
        if (shipStats.Credits >= priceForCrewReplacement)
        {
            shipStats.Credits += -priceForCrewReplacement;
            shipStats.CrewCurrent += new Vector3(crewLost, 0, crewLost);
        }
    }

    /// <summary>
    /// How much of the Hull Damage is to be fixed. Passes in bool: true for adding, false for subtracting
    /// </summary>
    public void HullDamageToFix(bool addDamage)
    {
        if(addDamage == true && (hullDamage + hullIncrement <= shipStats.ShipHealthCurrent.y - shipStats.ShipHealthCurrent.x))
        {
            hullDamage += hullIncrement;
        }
        else if (addDamage == true && (hullDamage + hullIncrement >= shipStats.ShipHealthCurrent.y - shipStats.ShipHealthCurrent.x)) //fix max damage
        {
            hullDamage = (int)(shipStats.ShipHealthCurrent.y - shipStats.ShipHealthCurrent.x);
        }
        else if(addDamage == false && hullDamage - hullIncrement > 0)
        {
            hullDamage -= hullIncrement;
        }
        else if (addDamage == false && hullDamage - hullIncrement < 0) //fix 0 damage
        {
            hullDamage = 0;
        }

        priceForHullRepair = (int)(hullDamage * costForHullDurability);
    }

    /// <summary>
    /// How much of the crew is to be replace. Passes in bool: true for adding, false for subtracting
    /// </summary>
    public void CrewToReplace(bool addCrew)
    {
        if (addCrew == true && crewLost < shipStats.CrewCurrent.y - shipStats.CrewCurrent.x)
        {
            crewLost += 1;
        }
        else if(addCrew == false && crewLost > 0)
        {
            crewLost -= 1;
        }

        priceForCrewReplacement = (int)(crewLost * costForCrew);
    }
}
