/*
 * OverclockRoom.cs
 * Author(s): Grant Frey
 * Created on: 9/25/2020
 * Description: 
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class OverclockRoom : MonoBehaviour
{

    [Dropdown("resourceTypes")]
    public string resourceType;

    private List<string> resourceTypes
    {
        get
        {
            return new List<string>() { "", "Credits", "Energy", "Security",
        "Ship Weapons", "Crew", "Food", "Food Per Tick", "Hull Durability", "Stock" };
        }
    }

    public int resourceAmount;

    public int crewMoraleAmount;

    OverclockController overclockController;

    void Start()
    {
        overclockController = FindObjectOfType<OverclockController>();
    }

    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !overclockController.overclocking)
        {
            overclockController.CallStartOverclocking(crewMoraleAmount, resourceType, resourceAmount);
        }
    }
}
