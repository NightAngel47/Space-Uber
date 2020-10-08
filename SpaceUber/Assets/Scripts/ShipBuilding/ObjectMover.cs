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
    [SerializeField] private float moveDistance;

    // Start is called before the first frame update
    void Start()
    {
        c = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        c.a = 1;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
        os = gameObject.GetComponentInChildren<ObjectScript>();
    }

    public float GetMoveDis()
    {
        return moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        RotateObject();
        Placement();
    }

    public void Movement()
    {
        if (os.rotAdjust == 2 || os.rotAdjust == 4)
        {
            if (Input.GetKeyDown(KeyCode.W) && gameObject.transform.position.y < os.rotBoundsUp)
            {
                gameObject.transform.position += new Vector3(0, moveDistance, 0);
            }

            if (Input.GetKeyDown(KeyCode.S) && gameObject.transform.position.y > os.boundsDown)
            {
                gameObject.transform.position -= new Vector3(0, moveDistance, 0);
            }

            if (Input.GetKeyDown(KeyCode.D) && gameObject.transform.position.x < os.rotBoundsRight)
            {
                gameObject.transform.position += new Vector3(moveDistance, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.A) && gameObject.transform.position.x > os.boundsLeft)
            {
                gameObject.transform.position -= new Vector3(moveDistance, 0, 0);
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.W) && gameObject.transform.position.y < os.boundsUp)
            {
                gameObject.transform.position += new Vector3(0, moveDistance, 0);
            }

            if (Input.GetKeyDown(KeyCode.S) && gameObject.transform.position.y > os.boundsDown)
            {
                gameObject.transform.position -= new Vector3(0, moveDistance, 0);
            }

            if (Input.GetKeyDown(KeyCode.D) && gameObject.transform.position.x < os.boundsRight)
            {
                gameObject.transform.position += new Vector3(moveDistance, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.A) && gameObject.transform.position.x > os.boundsLeft)
            {
                gameObject.transform.position -= new Vector3(moveDistance, 0, 0);
            }
        }
    }

    public void RotateObject()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, 90);

            if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 1 || os.rotAdjust == 3))
            {
                gameObject.transform.GetChild(0).transform.position += os.rotAdjustVal;
                os.rotAdjust -= 1;
            }
            else if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 2 || os.rotAdjust == 4))
            {
                gameObject.transform.GetChild(0).transform.position -= os.rotAdjustVal;
                os.rotAdjust -= 1;
            }

            if(os.rotAdjust == 0)
            {
                os.rotAdjust = 4;
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, -90);


            if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 1 || os.rotAdjust == 3))
            {
                gameObject.transform.GetChild(0).transform.position += os.rotAdjustVal;
                os.rotAdjust += 1;
            }
            else if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 2 || os.rotAdjust == 4))
            {
                gameObject.transform.GetChild(0).transform.position -= os.rotAdjustVal;
                os.rotAdjust += 1;
            }

            if (os.rotAdjust == 5)
            {
                os.rotAdjust = 1;
            }
        }
    }

    public void Placement()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (FindObjectOfType<ShipStats>().GetCredits() >= gameObject.GetComponent<RoomStats>().price)
            {
                SpotChecker.instance.FillSpots(gameObject, os.rotAdjust, moveDistance);

                if (SpotChecker.cannotPlace == false)
                {
                    gameObject.GetComponent<RoomStats>().AddRoomStats();

                    hasPlaced = true;
                    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ObjectScript.c;
                    Destroy(gameObject.GetComponent<ObjectMover>());
                }
            }

            else
            {
                Debug.Log("Cannot Afford");
            }
        }
    }

    //public void LayoutPlacement() //for spawning from layout to make sure they act as if they were placed normallys
    //{
    //    hasPlaced = true;
    //    gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ObjectScript.c;
    //    Destroy(gameObject.GetComponent<ObjectMover>());
    //}
}
