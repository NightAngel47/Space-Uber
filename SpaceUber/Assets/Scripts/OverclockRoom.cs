/*
 * OverclockRoom.cs
 * Author(s): Grant Frey
 * Created on: 9/25/2020
 * Description: 
 */

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverclockRoom : MonoBehaviour
{
    //Move to OverclockController? ********************************************Start
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

    //Move to OverclockController? **************************************************End

    OverclockController overclockController;
    [Tooltip("Name of mini game scene")]
    [SerializeField] string miniGame;

    void Start()
    {
        overclockController = FindObjectOfType<OverclockController>();
    }

	private void OnMouseDown()
    {
        //Restrict to not activate during room placement?

        if (!overclockController.overclocking && !overclockController.miniGameInProgress)
        {

            overclockController.StartMiniGame(miniGame);
            //overclockController.CallStartOverclocking(crewMoraleAmount, resourceType, resourceAmount);
        }
    }
}
