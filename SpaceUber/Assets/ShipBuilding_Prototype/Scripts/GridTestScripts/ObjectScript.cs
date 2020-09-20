/*
 * ObjectScript.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Interface for object after its placed, checks if it is deleted or edited and sets that up
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour
{
    public static int rotateVal;
    public int rotAdjust = 1;
    private GameObject parentObj;
    public static Color c;

    public GameObject buttons;

    public int shapeType;
    public int objectNum;

    [SerializeField] private ShapeType shapeDataTemplate = null;

    public ShapeType shapeData = null;
    public ShapeTypes shapeTypes => shapeData.St;

    public float boundsUp;
    public float boundsDown;
    public float boundsLeft;
    public float boundsRight;
    public float rotBoundsRight;
    public float rotBoundsUp;

    public Vector3 rotAdjustVal;

    public static ObjectScript instance;

    private void Start()
    {
        instance = this;
        //rotAdjust = false;
        c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = 0.5f;
        parentObj = gameObject.transform.parent.gameObject;
        //buttons = gameObject.transform.parent.transform.GetChild(1).gameObject;

        ResetData();
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && ObjectMover.hasPlaced == true)
        {
            //buttons.SetActive(true);
            Edit();
        }

        if (Input.GetMouseButton(1))
        {
            //buttons.SetActive(true);
            Delete();
        }
    }

    public void Edit()
    {
        //buttons.SetActive(false);
        c.a = 1;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        c.a = .5f;
        SpotChecker.instance.RemoveSpots(parentObj, rotAdjust);
        parentObj.AddComponent<ObjectMover>();
        ObjectMover.hasPlaced = false;
    }

    public void Delete()
    {
        //buttons.SetActive(false);
        SpotChecker.instance.RemoveSpots(parentObj, rotAdjust);
        ObjectMover.hasPlaced = true;
        Destroy(parentObj);
    }

    public void SetButtons()
    {
        buttons = gameObject.transform.parent.transform.GetChild(1).gameObject;
        buttons.SetActive(false);
    }

    private void ResetData()
    {
        shapeData = shapeDataTemplate.CloneData();
        boundsUp = shapeData.boundsUp;
        boundsDown = shapeData.boundsDown;
        boundsLeft = shapeData.boundsLeft;
        boundsRight = shapeData.boundsRight;
        rotBoundsRight = shapeData.rotBoundsRight;
        rotBoundsUp = shapeData.rotBoundsUp;

        rotAdjustVal = shapeData.rotAdjustVal;
    }
}
