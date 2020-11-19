/*
 * ObjectScript.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Interface for object after its placed, checks if it is deleted or edited and sets that up
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using Random = UnityEngine.Random;

public class ObjectScript : MonoBehaviour
{
    [Foldout("Data")]
    public int rotAdjust = 1;
    public static Color c;

    public int shapeType;
    public int objectNum;

    public bool canRotate;  //true can rotate | false cannot rotate
    public bool nextToRoom; //true required next to x room | false no condition 
    public int nextToRoomNum;
    public string nextToRoomName;
    public bool needsSpecificLocation;
    public bool preplacedRoom;
    public static bool CalledFromSpawn = false;

    public string[] mouseOverAudio;
    
    [SerializeField] private GameObject roomTooltip;

    [SerializeField] private ShapeType shapeDataTemplate = null;

    [Foldout("Data")]
    public ShapeType shapeData = null;
    public ShapeTypes shapeTypes => shapeData.St;

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

    public bool clickAgain = true;

    private void Start()
    {
        //rotAdjust = false;
        c = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        c.a = 1;
        //parentObj = transform.parent.gameObject;
        
        FindObjectOfType<EditCrewButton>().CheckForRooms();
        
        ResetData();
    }

    public void TurnOnClickAgain()
    {
        if (preplacedRoom == false)
        {
            clickAgain = true;
            CalledFromSpawn = false;
        }
    }

    public void TurnOffClickAgain()
    {
        if (preplacedRoom == false)
        {
            clickAgain = false;

            if (nextToRoom == true && CalledFromSpawn == false)
            {
                bool check = SpotChecker.instance.NextToRoomCall(gameObject, rotAdjust);
                if (check == false)
                {
                    Debug.Log("Room not placed next to required room, it has been auto removed");
                    SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnMouseOver()
    {
        if (preplacedRoom == false)
        {
            if (GameManager.instance.currentGameState == InGameStates.ShipBuilding && clickAgain == true)
            {
                if (ObjectMover.hasPlaced == true)
                {
                    roomTooltip.SetActive(true);
                }
                else if (roomTooltip.activeSelf)
                {
                    roomTooltip.SetActive(false);
                }

                if (Input.GetMouseButton(0) && ObjectMover.hasPlaced == true)
                {
                    //buttons.SetActive(true);
                    gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                    AudioManager.instance.PlaySFX(mouseOverAudio[Random.Range(0, mouseOverAudio.Length - 1)]);
                    Edit();
                }

                if (Input.GetMouseButton(1))
                {
                    //buttons.SetActive(true);
                    if (ObjectMover.hasPlaced == true)
                    {
                        gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                        AudioManager.instance.PlaySFX("Sell");
                    }

                    Delete();
                }
            }

            if (GameManager.instance.currentGameState == InGameStates.CrewManagement
               || GameManager.instance.currentGameState == InGameStates.Events
               && !OverclockController.instance.overclocking && !EventSystem.instance.eventActive)
            {
                roomTooltip.SetActive(true);

                if (Input.GetMouseButton(0))
                {
                    FindObjectOfType<CrewManagement>().UpdateRoom(gameObject);
                }
            }
        }
    }


    public void OnMouseExit()
    {
        if (roomTooltip.activeSelf)
        {
            roomTooltip.SetActive(false);
        }
    }

    public void Edit()
    {
        c.a = .5f;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
        c.a = 1;
        SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
        gameObject.GetComponent<ObjectMover>().enabled = true;
        gameObject.GetComponent<ObjectMover>().TurnOnBeingDragged();
        ObjectMover.hasPlaced = false;

        if (needsSpecificLocation == true)
        {
            HighlightSpotsOn();
        }

        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            if (r != gameObject.GetComponent<ObjectScript>())
            {
                r.TurnOffClickAgain();
            }
        }
    }

    public void Delete()
    {
        //buttons.SetActive(false);
        SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
        ObjectMover.hasPlaced = true;
        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            r.TurnOnClickAgain();
        }

        Destroy(gameObject);
    }

    public void HighlightSpotsOn()
    {
        for (int i = 0; i < gameObject.GetComponent<SpecificLocationData>().specficLocations.rows.Length - 1; i++)
        {
            for (int j = 0; j < gameObject.GetComponent<SpecificLocationData>().specficLocations.rows[i].row.Length - 1; j++)
            {
                if (gameObject.GetComponent<SpecificLocationData>().specficLocations.rows[i].row[j] == true)
                {
                    FindObjectOfType<HighlightSpots>().highlights.rows[i].row[j].gameObject.SetActive(true);
                }
            }
        }
    }

    public void HighlightSpotsOff()
    {
        if (needsSpecificLocation == true)
        {
            for (int i = 0; i < gameObject.GetComponent<SpecificLocationData>().specficLocations.rows.Length - 1; i++)
            {
                for (int j = 0; j < gameObject.GetComponent<SpecificLocationData>().specficLocations.rows[i].row.Length - 1; j++)
                {
                    if (gameObject.GetComponent<SpecificLocationData>().specficLocations.rows[i].row[j] == true)
                    {
                        FindObjectOfType<HighlightSpots>().highlights.rows[i].row[j].gameObject.SetActive(false);
                    }
                }
            }
        }
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

        gameObject.GetComponent<ObjectMover>().UpdateMouseBounds(boundsDown, boundsUp, boundsLeft, boundsRight);
    }

    private void OnDestroy()
    {
        FindObjectOfType<EditCrewButton>()?.CheckForRooms();
    }
}
