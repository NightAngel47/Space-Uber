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
using NaughtyAttributes;
using TMPro;

public class ObjectScript : MonoBehaviour
{
    [Foldout("Data")]
    public int rotAdjust = 1;
    private GameObject parentObj;
    private float moveDistance;
    public static Color c;

    public int shapeType;
    public int objectNum;

    [SerializeField] private ShapeType shapeDataTemplate = null;

    [Foldout("Data")]
    public ShapeType shapeData = null;
    public ShapeTypes shapeTypes => shapeData.St;

    public GameObject hoverUiPanel;
    [SerializeField] private TMP_Text roomNameUI;
    [SerializeField] private TMP_Text roomDescUI;

    [Foldout("Data")]
    public float boundsUp;
    [Foldout("Data")]
    public float boundsDown;
    [Foldout("Data")]
    public float boundsLeft;
    [Foldout("Data")]
    public float boundsRight;
    [Foldout("Data")]
    public float rotBoundsRight;
    [Foldout("Data")]
    public float rotBoundsUp;

    [Foldout("Data")]
    public Vector3 rotAdjustVal;
    private void Start()
    {
        //rotAdjust = false;
        c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = 0.5f;
        parentObj = transform.parent.gameObject;
        moveDistance = parentObj.GetComponent<ObjectMover>().GetMoveDis();
        
        roomNameUI.text = parentObj.GetComponent<RoomStats>().roomName;
        roomDescUI.text = parentObj.GetComponent<RoomStats>().roomDescription;

        ResetData();
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && ObjectMover.hasPlaced == true)
        {
            //buttons.SetActive(true);
            parentObj.GetComponent<RoomStats>().SubtractRoomStats();
            Edit();
        }

        if (Input.GetMouseButton(1))
        {
            //buttons.SetActive(true);
            if (ObjectMover.hasPlaced == true)
            {
                parentObj.GetComponent<RoomStats>().SubtractRoomStats();
            }
            Delete();
        }

        hoverUiPanel.SetActive(true);
    }

    public void OnMouseExit()
    {
        hoverUiPanel.SetActive(false);
    }

    public void Edit()
    {
        //buttons.SetActive(false);
        c.a = 1;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        c.a = .5f;
        SpotChecker.instance.RemoveSpots(parentObj, rotAdjust, moveDistance);
        parentObj.AddComponent<ObjectMover>();
        ObjectMover.hasPlaced = false;
    }

    public void Delete()
    {
        //buttons.SetActive(false);
        SpotChecker.instance.RemoveSpots(parentObj, rotAdjust, moveDistance);
        ObjectMover.hasPlaced = true;
        Destroy(parentObj);
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

    public void UpdateHoverUIData(string n, string d, string s)
    {
        hoverUiPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = n;
        hoverUiPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = d;
        hoverUiPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = s;
    }
}
