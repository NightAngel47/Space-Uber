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

[CreateAssetMenu(fileName = "New ShapeType", menuName = "DataTypes/New ShapeType")]
public class ShapeType : DataType
{
	// Overridden properties from DataType.
    public override string AssemblyName => typeof(ShapeType).AssemblyQualifiedName;
    public override string TypeAsString => "ShapeType";

    [SerializeField] private ShapeTypes shapeType = ShapeTypes._1x1;
    public ShapeTypes St => shapeType;


    [SerializeField] public float boundsUp = 0;
    public float BoundsUp => boundsUp;

    [SerializeField] public float boundsDown = 0;
    public float BoundsDown => boundsDown;

    [SerializeField] public float boundsLeft = 0;
    public float BoundsLeft => boundsLeft;

    [SerializeField] public float boundsRight = 0;
    public float BoundsRight => boundsRight;

    [SerializeField] public float rotBoundsRight = 0;
    public float RotBoundsRight => rotBoundsRight;

    [SerializeField] public float rotBoundsUp = 0;
    public float RotBoundsUp => rotBoundsUp;

    [SerializeField] public Vector3 rotAdjustVal;
    public Vector3 RotAdjustVal => rotAdjustVal;

    [SerializeField] public List<Vector2> gridSpaces = new List<Vector2>();
    public List<Vector2> GridSpaces => gridSpaces;


    public ShapeType CloneData()
    {
        ShapeType clone = CreateInstance<ShapeType>();
        clone.shapeType = shapeType;
        clone.boundsUp = boundsUp;
        clone.boundsDown = boundsDown;
        clone.boundsLeft = boundsLeft;
        clone.boundsRight = boundsRight;
        clone.rotBoundsRight = rotBoundsRight;
        clone.rotBoundsUp = rotBoundsUp;
        clone.rotAdjustVal = rotAdjustVal;
        clone.gridSpaces = gridSpaces;
        return clone;
    }
}

public enum ShapeTypes
{
    _1x1,
    _1x2,
    _1x3,
    _2x2,
    _LeftL,
    _3x3,
    _Z,
    _ShortT,
    _SmallLeftL
}
