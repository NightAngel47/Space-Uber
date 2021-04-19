/*
 * ObjectMover.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Allows the room to move and rotate, calls on spot checker to see if where it can be placed is right
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{
    /// <summary>
    /// Variable to keep track of if room is placed. 
    /// True when able to spawn in a new room as there is no room not placed
    /// False when room is in the process of being placed
    /// </summary>
    public static bool hasPlaced = true; 
    private ObjectScript os;
    private Color c;

    private bool isBeingDragged = false;
    private bool mousedOver = false;
    private bool canPlace = true;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public string[] SFXs;
    public string[] Placements;

    // Start is called before the first frame update
    void Start()
    {
        c = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        c.a = .5f;

        foreach (SpriteRenderer spriteRenderer in gameObject.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = c;
        }

        gameObject.GetComponent<RoomStats>().levelIconObject.GetComponent<Image>().color = c;
        os = gameObject.GetComponent<ObjectScript>();
        
    }

    public void UpdateMouseBounds(float ymin, float ymax, float xmin, float xmax)
    {
        minX = xmin;
        maxX = xmax;
        minY = ymin;
        maxY = ymax;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
        {
            RotateObject();

            if (isBeingDragged == true)
            {
                //Follow cursor
                Vector3 mousePosition;
                mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0.0f;

                transform.position = new Vector3(Mathf.Clamp(Mathf.Round(mousePosition.x), minX, maxX), Mathf.Clamp(Mathf.Round(mousePosition.y), minY, maxY), mousePosition.z);

                if(Input.GetMouseButtonDown(0) && canPlace == true)
                {
                    isBeingDragged = false;
                    Placement();
                }

                if(Input.GetButtonDown("DeleteRoom"))
                {
                    StartCoroutine(os.Delete(os.isEdited, gameObject));
                }
            }
        }
    }

    public void TurnOnBeingDragged()
    {
        isBeingDragged = true;
        canPlace = false;
        StartCoroutine(WaitToPlace());
    }

    public void TurnOffBeingDragged()
    {
        isBeingDragged = false;
    }

    private void OnMouseEnter()
    {
        mousedOver = true;
    }

    private void OnMouseExit()
    {
        mousedOver = false;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
        {
            if (mousedOver && hasPlaced == true && gameObject.GetComponent<ObjectScript>().clickAgain == true)
            {
                if (isBeingDragged == false)
                {
                    isBeingDragged = true;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        //if (GameManager.currentGameState == InGameStates.ShipBuilding)
        //{
        //    isBeingDragged = false;

        //    Placement();
        //}
    }

    public void RotateObject()
    {
        if(Input.GetButtonDown("RotateLeft") && os.canRotate == true)
        {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, 90);
            AudioManager.instance.PlaySFX(SFXs[Random.Range(0, SFXs.Length)]);

            if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 1 || os.rotAdjust == 3))
            {
                gameObject.transform.GetChild(0).transform.position += os.rotAdjustVal;
                os.rotAdjust -= 1;
                UpdateMouseBounds(os.boundsDown, os.rotBoundsUp, os.boundsLeft, os.rotBoundsRight);
            }
            else if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 2 || os.rotAdjust == 4))
            {
                gameObject.transform.GetChild(0).transform.position -= os.rotAdjustVal;
                os.rotAdjust -= 1;
                UpdateMouseBounds(os.boundsDown, os.boundsUp, os.boundsLeft, os.boundsRight);
            }

            if(os.rotAdjust == 0)
            {
                os.rotAdjust = 4;
            }
        }

        if(Input.GetButtonDown("RotateRight") && os.canRotate == true)
        {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, -90);
            AudioManager.instance.PlaySFX(SFXs[Random.Range(0, SFXs.Length)]);

            if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 1 || os.rotAdjust == 3))
            {
                gameObject.transform.GetChild(0).transform.position += os.rotAdjustVal;
                os.rotAdjust += 1;
                UpdateMouseBounds(os.boundsDown, os.rotBoundsUp, os.boundsLeft, os.rotBoundsRight);
            }
            else if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 2 || os.rotAdjust == 4))
            {
                gameObject.transform.GetChild(0).transform.position -= os.rotAdjustVal;
                os.rotAdjust += 1;
                UpdateMouseBounds(os.boundsDown, os.boundsUp, os.boundsLeft, os.boundsRight);
            }

            if (os.rotAdjust == 5)
            {
                os.rotAdjust = 1;
            }
        }
    }

    public void Placement()
    {
        if (GameManager.instance.currentGameState != InGameStates.ShipBuilding) return;
        //Checks to see if we have enough credits or energy to place the room
        if (FindObjectOfType<ShipStats>().Credits >= gameObject.GetComponent<RoomStats>().price[gameObject.GetComponent<RoomStats>().GetRoomLevel() - 1] && 
            FindObjectOfType<ShipStats>().Energy.z >= gameObject.GetComponent<RoomStats>().minPower[gameObject.GetComponent<RoomStats>().GetRoomLevel() - 1]) 
        {
            if (os.needsSpecificLocation == false) //Check spots normally
            {
                SpotChecker.instance.FillSpots(gameObject, os.rotAdjust);
            }
            else //check for specific spots as required by parent room
            {
                SpotChecker.instance.SpecificSpotCheck(gameObject, os.rotAdjust);
            }

            if (SpotChecker.cannotPlace == false) //Once spots are checked if cannotPlace = false/no room is placed there place room
            {
                AudioManager.instance.PlaySFX(Placements[Random.Range(0, Placements.Length)]);

                if (os.isEdited == false)
                {
                    gameObject.GetComponent<RoomStats>().AddRoomStats();
                }
                else
                {
                    os.isEdited = false;
                }

                //makes sure the room is on the lower layer so that the new rooms can be on top without flickering
                foreach (SpriteRenderer spriteRenderer in  gameObject.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.sortingOrder -= 5;
                }
                
                hasPlaced = true;

                if (os.needsSpecificLocation == true)
                {
                    os.HighlightSpotsOff();
                }

                if (os.nextToRoom == true)
                {
                    os.RoomHighlightSpotsOff();
                }

                Cursor.visible = true;
                //HOVER UI does not happen when mouse is hidden
                //StartCoroutine(os.WaitToClickRoom());
                
                foreach (SpriteRenderer spriteRenderer in  gameObject.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.color = ObjectScript.c;
                    gameObject.GetComponent<RoomStats>().levelIconObject.GetComponent<Image>().color = ObjectScript.c;
                }
                
                gameObject.GetComponent<ObjectMover>().enabled = false;
                
                FindObjectOfType<CrewManagement>().CheckForRoomsCall();
            }

            else //If something is placed allow player to keep moving room
            {
                hasPlaced = false;
                TurnOnBeingDragged();
            }
        }

        else //If not enough credits or energy allow player to keep moving room
        {
            TurnOnBeingDragged();
        }
    }

    //public IEnumerator ClickWait()
    //{
    //    os.TurnOffClickAgain();
    //    yield return new WaitForSeconds(.1f);
    //    ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
    //    foreach (ObjectScript r in otherRooms)
    //    {
    //        r.TurnOnClickAgain();
    //    }
    //}

    public IEnumerator WaitToPlace()
    {
        yield return new WaitForSeconds(.1f);
        canPlace = true;
    }

    //public void LayoutPlacement() //for spawning from layout to make sure they act as if they were placed normallys
    //{
    //    hasPlaced = true;
    //    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ObjectScript.c;
    //    Destroy(gameObject.GetComponent<ObjectMover>());
    //}
}
