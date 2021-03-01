using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class RefillFunctionality : MonoBehaviour
{
    //[SerializeField] 
    private float costForCrew = 0;
    //[SerializeField] 
    private float costForHullDurability = 0;

    /// <summary>
    /// How much of the Hull Damage is the player incrementing
    /// </summary>
    //[SerializeField] 
    private int hullIncrement = 0;

    private ShipStats shipStats;

    [SerializeField, Foldout("Repair Hull")] private ButtonTwoBehaviour hullRepairButton;
    [SerializeField, Foldout("Repair Hull")] private TMP_Text[] repairToolTipText = new TMP_Text[2];
    [SerializeField, Foldout("Repair Hull")] private int hullDamage = 0;
    [SerializeField, Foldout("Repair Hull")] private int priceForHullRepair = 0;
    
    [SerializeField, Foldout("Replace Crew")] private ButtonTwoBehaviour crewRefillButton;
    [SerializeField, Foldout("Replace Crew")] private TMP_Text[] refillToolTipText = new TMP_Text[2];
    [SerializeField, Foldout("Replace Crew")] private int crewLost = 0;
    [SerializeField, Foldout("Replace Crew")] private int priceForCrewReplacement = 0;

    public void Start()
    {
        shipStats = FindObjectOfType<ShipStats>();

        // set repair tooltip costs/gains
        repairToolTipText[0].text = "-" + priceForHullRepair;
        repairToolTipText[1].text = hullDamage.ToString();
        
        // set refill tooltip costs/gains
        refillToolTipText[0].text = "-" + priceForCrewReplacement;
        refillToolTipText[1].text = crewLost.ToString();

        CheckCanRefillCrew();
        CheckCanRepairShip();
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

    // if hull repair should deactivate
    private void CheckCanRepairShip()
    {
        // has enough credits and hull is less than max
        hullRepairButton.SetButtonInteractable(shipStats.Credits < priceForHullRepair && shipStats.ShipHealthCurrent.x < shipStats.ShipHealthCurrent.y);
    }

    // if crew refill should deactivate
    private void CheckCanRefillCrew()
    {
        // has enough credits and crew current is less than capacity
        hullRepairButton.SetButtonInteractable(shipStats.Credits >= priceForCrewReplacement && shipStats.CrewCurrent.x < shipStats.CrewCurrent.y);
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
