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
    [Foldout("Data")] public int rotAdjust = 1;
    public static Color c;

    public string roomSize;
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
    [SerializeField] private GameObject toolTipOutputList;

    [SerializeField] private ShapeType shapeDataTemplate = null;

    [Foldout("Data")] public ShapeType shapeData = null;
    public ShapeTypes shapeTypes => shapeData.St;
    
    [Foldout("Data")] public float boundsUp;
    [Foldout("Data")] public float boundsDown;
    [Foldout("Data")] public float boundsLeft;
    [Foldout("Data")] public float boundsRight;
    [Foldout("Data")] public float rotBoundsRight;
    [Foldout("Data")] public float rotBoundsUp;
    [Foldout("Data")] public Vector3 rotAdjustVal;

    public bool clickAgain = true;

    private bool mouseReleased = false;
    public static bool roomIsHovered;
    
    private void Start()
    {
        //rotAdjust = false;
        c = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        c.a = 1;
        //parentObj = transform.parent.gameObject;

        ResetData();
    }

    public void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            StartCoroutine(WaitToClickRoom());
        }
    }

    public void TurnOnClickAgain()
    {
        if (preplacedRoom == false)
        {
            clickAgain = true;
            mouseReleased = false;
            CalledFromSpawn = false;
        }
    }

    public void TurnOffClickAgain()
    {
        if (preplacedRoom == false)
        {
            clickAgain = false;

            //if (nextToRoom == true && CalledFromSpawn == false)
            //{
            //    bool check = SpotChecker.instance.NextToRoomCall(gameObject, rotAdjust);
            //    if (check == false)
            //    {
            //        Debug.Log("Room not placed next to required room, it has been auto removed");

            //        SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
            //        Destroy(gameObject);
            //    }
            //}
        }
    }

    //public void OnMouseUp()
    //{
    //    //StartCoroutine(WaitToClickRoom());
    //    //clickAgain = true;

    //    mouseReleased = true;
    //}

    public void OnMouseOver()
    {
        //if(preplacedRoom) return;

        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding && clickAgain == true) // && PauseMenu.Instance.isPaused == false// commented out until menus are ready
        {
            if (ObjectMover.hasPlaced && !PauseMenu.IsPaused)
            {
                roomTooltip.SetActive(true);
                roomIsHovered = true;
            }
            else if (roomTooltip.activeSelf)
            {
                roomTooltip.SetActive(false);
            }

            //if(preplacedRoom) return; // could moved preplacedRoom check here so tooltip can be activated.

            if (Input.GetMouseButtonDown(0) && ObjectMover.hasPlaced == true && !gameObject.GetComponent<ObjectMover>().enabled && preplacedRoom == false)
            {
                //buttons.SetActive(true);
                gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                AudioManager.instance.PlaySFX(mouseOverAudio[Random.Range(0, mouseOverAudio.Length - 1)]);
                Edit();
            }

            if (Input.GetMouseButton(1) && preplacedRoom == false)
            {
                //buttons.SetActive(true);
                if (ObjectMover.hasPlaced == true)
                {
                    gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                    gameObject.GetComponent<RoomStats>().ReturnCrewOnRemove();
                    AudioManager.instance.PlaySFX("Sell");
                }

                Delete();
            }
        }

        if (GameManager.instance.currentGameState == InGameStates.CrewManagement
           || GameManager.instance.currentGameState == InGameStates.Events
           && !OverclockController.instance.overclocking && !EventSystem.instance.eventActive && !EventSystem.instance.NextEventLockedIn)
        {
            roomTooltip.SetActive(true);
            roomIsHovered = true;
            
            //if the object is clicked, open the room management menu
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<CrewManagement>().UpdateRoom(gameObject);
                FindObjectOfType<RoomPanelToggle>().OpenPanel(0, true);
                FindObjectOfType<CrewManagementRoomDetailsMenu>().ChangeCurrentRoom(gameObject);
                FindObjectOfType<CrewManagementRoomDetailsMenu>().UpdatePanelInfo();
                AudioManager.instance.PlaySFX(mouseOverAudio[Random.Range(0, mouseOverAudio.Length - 1)]);
            }
        }
    }

    public IEnumerator WaitToClickRoom()
    {
        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            r.TurnOffClickAgain();
        }

        //yield return new WaitUntil(() => mouseReleased);
        yield return new WaitForSeconds(.25f);

        foreach (ObjectScript r in otherRooms)
        {
            r.TurnOnClickAgain();
        }
    }

    public void OnMouseExit()
    {
        if (roomTooltip.activeSelf)
        {
            roomTooltip.SetActive(false);
        }
        
        roomIsHovered = false;
    }

    public void Edit()
    {
        Cursor.visible = false;
        clickAgain = false;

        //rooms being placed will appear on top of other rooms that are already placed
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

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

            if (nextToRoomNum == r.objectNum)
            {
                FindObjectOfType<SpawnObject>().NextToRoomHighlight(r.gameObject);
            }
        }
    }

    public void Delete()
    {
        Cursor.visible = true;
        clickAgain = false;

        //buttons.SetActive(false);
        if (ObjectMover.hasPlaced == true)
        {
            SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
        }
        ObjectMover.hasPlaced = true;
        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            r.TurnOnClickAgain();

            if(r.nextToRoom == true && CalledFromSpawn == false && gameObject != r.gameObject)
            {
                bool check = SpotChecker.instance.NextToRoomCall(r.gameObject, r.rotAdjust);
                if (check == false)
                {
                    SpotChecker.instance.RemoveSpots(r.gameObject, r.rotAdjust);
                    r.gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                    Destroy(r.gameObject);
                }
            }
        }

        if(nextToRoom == true)
        {
            RoomHighlightSpotsOff();
        }

        if(needsSpecificLocation == true)
        {
            HighlightSpotsOff();
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

    public void RoomHighlightSpotsOff()
    {
        ObjectScript[] cube = FindObjectsOfType<ObjectScript>();

        foreach (ObjectScript r in cube)
        {
            int shapeType = r.shapeType;
            GameObject gridPosBase = r.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            List<Vector2> gridSpots = new List<Vector2>(r.shapeData.gridSpaces);
            ArrayLayoutGameobject spots = FindObjectOfType<HighlightSpots>().highlights;

            int x = 0;
            int y = 0;

            for (int i = 0; i < gridSpots.Count; i++)
            {
                if (shapeType == 2) //these objects only have two different rotations
                {
                    if (r.rotAdjust == 1 || r.rotAdjust == 3)
                    {
                        y = (int)Math.Round(r.transform.position.y + gridSpots[i].y);
                        x = (int)Math.Round(r.transform.position.x + gridSpots[i].x);
                    }

                    if (r.rotAdjust == 2 || r.rotAdjust == 4)
                    {
                        y = ((int)Math.Round(r.transform.position.y + gridSpots[i].x));
                        x = (int)Math.Round(r.transform.position.x + gridSpots[i].y);
                    }
                }

                if (r.rotAdjust == 1)
                {
                    y = ((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y));
                    x = (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x);
                }

                if (r.rotAdjust == 2)
                {
                    y = ((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1));
                    x = (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y);
                }

                if (r.rotAdjust == 3)
                {
                    y = ((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1));
                    x = (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1);
                }

                if (r.rotAdjust == 4)
                {
                    y = ((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x));
                    x = (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1);
                }


                if (y < 5) //# needs to change to dynamically update with different ship sizes
                {
                    spots.rows[y + 1].row[x].gameObject.SetActive(false);
                }
                else if (y < 5)
                {
                    spots.rows[y + 1].row[x].gameObject.SetActive(false);
                }

                if (y > 0)
                {
                    spots.rows[y - 1].row[x].gameObject.SetActive(false);
                }
                else if (y > 0)
                {
                    spots.rows[y - 1].row[x].gameObject.SetActive(false);
                }

                if (x < 8) //# needs to change to dynamically update with different ship sizes
                {
                    spots.rows[y].row[x + 1].gameObject.SetActive(false);
                }
                else if (x < 8)
                {
                    spots.rows[y].row[x + 1].gameObject.SetActive(false);
                }

                if (x > 0)
                {
                    spots.rows[y].row[x - 1].gameObject.SetActive(false);
                }
                else if (x > 0)
                {
                    spots.rows[y].row[x - 1].gameObject.SetActive(false);
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
