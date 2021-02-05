/*
 * Resouce.cs
 * Author(s): Grant Frey []
 * Created on: 9/16/2020 (en-US)
 * Description: 
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [Dropdown("resourceTypes")]
    public string resourceType;

    private List<string> resourceTypes { get { return new List<string>() { "", "Credits", "Energy", "Security",
        "Ship Weapons", "Crew", "Food", "Food Per Tick", "Hull Durability", "Crew Morale"}; } }

    public Sprite resourceIcon;
    
    public int amount;
    public int activeAmount = 0;
    public int minAmount;

    void Start()
    {
        GetComponent<RoomStats>().AddToResourceList(this);
    }
}
