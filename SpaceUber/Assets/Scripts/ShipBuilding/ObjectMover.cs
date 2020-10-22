/*
 * ObjectMover.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Allows the room to move and rotate, calls on spot checker to see if where it can be placed is right
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public static bool hasPlaced = true;
    private ObjectScript os;
    private Color c;

    private bool isBeingDragged = false;
    private bool mousedOver = false;

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
        c.a = 1;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
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
        if (GameManager.currentGameState == InGameStates.ShipBuilding)
        {
            //Movement();
            RotateObject();
            //Placement();
            
            if(isBeingDragged)
            {
                //Follow cursor
                Vector3 mousePosition;
                mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0.0f;

                transform.position = new Vector3(Mathf.Clamp(Mathf.Round(mousePosition.x), minX, maxX), Mathf.Clamp(Mathf.Round(mousePosition.y), minY, maxY), mousePosition.z);
            }
        }
    }

    public void TurnOnBeingDragged()
    {
        isBeingDragged = true;
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
        if (mousedOver) 
        { 
            isBeingDragged = true; 
        } 
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;

        Placement();
    }

    public void RotateObject()
    {
        if(Input.GetKeyDown(KeyCode.Q))
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

        if(Input.GetKeyDown(KeyCode.E))
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
        if (GameManager.currentGameState != InGameStates.ShipBuilding) return;
        if (FindObjectOfType<ShipStats>().GetCredits() >= gameObject.GetComponent<RoomStats>().price)
        {
            SpotChecker.instance.FillSpots(gameObject, os.rotAdjust);
            AudioManager.instance.PlaySFX(Placements[Random.Range(0, Placements.Length)]);

            if (SpotChecker.cannotPlace == false)
            {
                gameObject.GetComponent<RoomStats>().AddRoomStats();

                hasPlaced = true;
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ObjectScript.c;
                gameObject.GetComponent<ObjectMover>().enabled = false;
            }
        }

        else
        {
            Debug.Log("Cannot Afford");
            Destroy(gameObject);
        }
    }

    

    

    //public void LayoutPlacement() //for spawning from layout to make sure they act as if they were placed normallys
    //{
    //    hasPlaced = true;
    //    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ObjectScript.c;
    //    Destroy(gameObject.GetComponent<ObjectMover>());
    //}
}
