/*
 * ShapeType.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Data type for all the different shape types of the rooms, holds all the things that each room needs numbers for
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACTools.DataManager;

[CreateAssetMenu(fileName = "New ResourceDataType", menuName = "DataTypes/New ResourceDataType")]
public class ResourceDataType : DataType
{
	// Overridden properties from DataType.
    public override string AssemblyName => typeof(ResourceDataType).AssemblyQualifiedName;
    public override string TypeAsString => "ResourceDataType";

    [SerializeField] private ResourceDataTypes resourceType = ResourceDataTypes._Credits;
    public ResourceDataTypes Rt => resourceType;


    ////[SerializeField] public float boundsUp = 0;
    ////public float BoundsUp => boundsUp;

    [SerializeField] public string resourceName = "";
    public string ResourceName => resourceName;

    [SerializeField] public Sprite resourceIcon;
    public Sprite ResourceIcon => resourceIcon;


    public ResourceDataType CloneData()
    {
        ResourceDataType clone = CreateInstance<ResourceDataType>();
        clone.resourceName = resourceName;
        clone.resourceIcon = resourceIcon;
        //clone.shapeType = shapeType;
        //clone.boundsUp = boundsUp;
        //clone.boundsDown = boundsDown;
        //clone.boundsLeft = boundsLeft;
        //clone.boundsRight = boundsRight;
        //clone.rotBoundsRight = rotBoundsRight;
        //clone.rotBoundsUp = rotBoundsUp;
        //clone.rotAdjustVal = rotAdjustVal;
        //clone.gridSpaces = gridSpaces;
        return clone;
    }
}

public enum ResourceDataTypes
{
    _Credits,
    _Food,
    _ShipWeapons,
    _HullDurability,
    _Crew,
    _Energy,
    _Security,
    _FoodPerTick,
    _Payout
}
