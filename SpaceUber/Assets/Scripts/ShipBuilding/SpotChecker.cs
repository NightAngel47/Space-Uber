/*
 * SpotChecker.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: When a room gets placed, its makes sure it is not being placed on another room, then adds it to the grid. It will also remove rooms when they are deleted
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SpotChecker : MonoBehaviour
{
    public ArrayLayout spots;

    public static bool cannotPlace = false; //bool for when the spot is filled
    private bool isNextToRoom = true;
    public static SpotChecker instance;

    private void Awake()
    {
        instance = this;
    }

    //how to change the individual thing
    //spots.rows[1].row[1] = true;

    public void FillSpots(GameObject cube, int rotate) //called when object is attempted to be placed
    {
        int shapeType = cube.GetComponent<ObjectScript>().shapeType;
        int objectNum = cube.GetComponent<ObjectScript>().objectNum;
        GameObject gridPosBase = cube.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        List<Vector2> gridSpots = new List<Vector2>(cube.GetComponent<ObjectScript>().shapeData.gridSpaces);
        cannotPlace = false;

        if (cube.GetComponent<ObjectScript>().nextToRoom == true)
        {
            isNextToRoom = false;
        }

        else
        {
            isNextToRoom = true;
        }

        for (int i = 0; i < gridSpots.Count; i++)
        {
            if(shapeType == 2) //these objects only have two different rotations
            {
                if (rotate == 1 || rotate == 3)
                {
                    if (spots.rows[(int)Math.Round(cube.transform.position.y + gridSpots[i].y)]
                        .row[(int)Math.Round(cube.transform.position.x + gridSpots[i].x)] != 0 || (int)Math.Round(cube.transform.position.y + gridSpots[i].y) >= 6
                        || (int)Math.Round(cube.transform.position.x + gridSpots[i].x) >= 9)
                    {
                        cannotPlace = true; //lets user keep moving object
                        Debug.Log("Cannot place here");
                        ObjectMover.hasPlaced = true;
                        return;
                    }

                    else if (cube.GetComponent<ObjectScript>().nextToRoom == true)
                    {
                        NextToRoomCheck((int)Math.Round(cube.transform.position.y + gridSpots[i].y),
                            (int)Math.Round(cube.transform.position.x + gridSpots[i].x), cube);
                    }
                }

                if(rotate == 2 || rotate == 4)
                {
                    if (spots.rows[(int)Math.Round(cube.transform.position.y + gridSpots[i].x)]
                        .row[(int)Math.Round(cube.transform.position.x + gridSpots[i].y)] != 0 || (int)Math.Round(cube.transform.position.y + gridSpots[i].x) >= 6
                        || (int)Math.Round(cube.transform.position.x + gridSpots[i].y) >= 9)
                    {
                        cannotPlace = true; //lets user keep moving object
                        Debug.Log("Cannot place here");
                        ObjectMover.hasPlaced = true;
                        return;
                    }

                    else if (cube.GetComponent<ObjectScript>().nextToRoom == true)
                    {
                        NextToRoomCheck((int)Math.Round(cube.transform.position.y + gridSpots[i].x),
                            (int)Math.Round(cube.transform.position.x + gridSpots[i].y), cube);
                    }
                }
            }

            if (rotate == 1)
            {
                if (spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x)] != 0 || (int)Math.Round(cube.transform.position.y + gridSpots[i].y) >= 6
                    || (int)Math.Round(cube.transform.position.x + gridSpots[i].x) >= 9)
                    {
                    cannotPlace = true; //lets user keep moving object
                    Debug.Log("Cannot place here");
                    ObjectMover.hasPlaced = true;
                    return;
                }

                else if (cube.GetComponent<ObjectScript>().nextToRoom == true)
                {
                    NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y),
                        (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x), cube);
                }
            }

            if (rotate == 2)
            {
                if (spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y)] != 0 || (int)Math.Round(cube.transform.position.y - gridSpots[i].x) >= 6
                        || (int)Math.Round(cube.transform.position.x + gridSpots[i].y) >= 9) 
                {
                    cannotPlace = true; //lets user keep moving object
                    Debug.Log("Cannot place here");
                    ObjectMover.hasPlaced = true;
                    return;
                }

                else if (cube.GetComponent<ObjectScript>().nextToRoom == true)
                {
                    NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1),
                        (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y), cube);
                }
            }

            if (rotate == 3)
            {
                if (spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1)] != 0 || (int)Math.Round(cube.transform.position.y - gridSpots[i].y) >= 6
                        || (int)Math.Round(cube.transform.position.x - gridSpots[i].x) >= 9)
                {
                    cannotPlace = true; //lets user keep moving object
                    Debug.Log("Cannot place here");
                    ObjectMover.hasPlaced = true;
                    return;
                }

                else if (cube.GetComponent<ObjectScript>().nextToRoom == true)
                {
                    NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1),
                        (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1), cube);
                }
            }

            if (rotate == 4)
            {
                if (spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x)]
                        .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1)] != 0 || (int)Math.Round(cube.transform.position.y + gridSpots[i].x) >= 6
                        || (int)Math.Round(cube.transform.position.x - gridSpots[i].y) >= 9)
                {
                    cannotPlace = true; //lets user keep moving object
                    Debug.Log("Cannot place here");
                    ObjectMover.hasPlaced = true;
                    return;
                }

                else if(cube.GetComponent<ObjectScript>().nextToRoom == true)
                {
                    NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x),
                        (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1), cube);
                }
            }
        }

        if(isNextToRoom == false)
        {
            cannotPlace = true;
            Debug.Log("Cannot place here");
            return;
        }

        if (cannotPlace == false)
        {
            for (int i = 0; i < gridSpots.Count; i++)
            {
                if (shapeType == 2) //these objects only have two different rotations
                {
                    if (rotate == 1 || rotate == 3)
                    {
                        spots.rows[(int)Math.Round(cube.transform.position.y + gridSpots[i].y)]
                            .row[(int)Math.Round(cube.transform.position.x + gridSpots[i].x)] = objectNum;
                        continue;
                    }

                    if (rotate == 2 || rotate == 4)
                    {
                        spots.rows[(int)Math.Round(cube.transform.position.y + gridSpots[i].x)]
                            .row[(int)Math.Round(cube.transform.position.x + gridSpots[i].y)] = objectNum;
                        continue;
                    }
                }

                if (rotate == 1)
                {
                    spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y)]
                        .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x)] = objectNum;
                }

                if (rotate == 2)
                {
                    spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1)]
                        .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y)] = objectNum;
                }

                if (rotate == 3)
                {
                    spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1)]
                        .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1)] = objectNum;
                }

                if (rotate == 4)
                {
                    spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x)]
                        .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1)] = objectNum;
                }
            }
        }
    }
    
    public void RemoveSpots(GameObject cube, int rotate) //when the object is edited and moved, erase prev spot
    {
        GameObject gridPosBase = cube.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        List<Vector2> gridSpots = new List<Vector2>(cube.GetComponent<ObjectScript>().shapeData.gridSpaces);

        for (int i = 0; i < gridSpots.Count; i++)
        {
            if (rotate == 1)
            {
                spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x)] = 0;
            }

            if (rotate == 2)
            {
                spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y)] = 0;
            }

            if (rotate == 3)
            {
                spots.rows[(int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1)] = 0;
            }

            if (rotate == 4)
            {
                spots.rows[(int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x)]
                    .row[(int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1)] = 0;
            }
        }
    }

    public bool NextToRoomCall(GameObject cube, int rotate) //when only next to room needs to be called
    {
        int shapeType = cube.GetComponent<ObjectScript>().shapeType;
        GameObject gridPosBase = cube.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        List<Vector2> gridSpots = new List<Vector2>(cube.GetComponent<ObjectScript>().shapeData.gridSpaces);

        isNextToRoom = false;

        for (int i = 0; i < gridSpots.Count; i++)
        {
            if (shapeType == 2) //these objects only have two different rotations
            {
                if (rotate == 1 || rotate == 3)
                {
                    NextToRoomCheck((int)Math.Round(cube.transform.position.y + gridSpots[i].y),
                        (int)Math.Round(cube.transform.position.x + gridSpots[i].x), cube);
                }

                if (rotate == 2 || rotate == 4)
                {
                    NextToRoomCheck((int)Math.Round(cube.transform.position.y + gridSpots[i].x),
                        (int)Math.Round(cube.transform.position.x + gridSpots[i].y), cube);
                }
            }

            if (rotate == 1)
            {
                NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y),
                    (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x), cube);
            }

            if (rotate == 2)
            {
                NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1),
                    (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y), cube);
            }

            if (rotate == 3)
            {
                NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1),
                    (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1), cube);
            }

            if (rotate == 4)
            {
                NextToRoomCheck((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x),
                    (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1), cube);
            }
        }

        return isNextToRoom;
    }

    public void NextToRoomCheck(int y, int x, GameObject cube)
    {
        if (y < 5) //# needs to change to dynamically update with different ship sizes
        {
            if (spots.rows[y + 1].row[x] != cube.GetComponent<ObjectScript>().nextToRoomNum && isNextToRoom == false)
            {
                isNextToRoom = false;
            }
            else
            {
                isNextToRoom = true;
            }
        }

        if (y > 0)
        {
            if (spots.rows[y - 1].row[x] != cube.GetComponent<ObjectScript>().nextToRoomNum && isNextToRoom == false)
            {
                isNextToRoom = false;
            }
            else
            {
                isNextToRoom = true;
            }
        }

        if (x < 9) //# needs to change to dynamically update with different ship sizes
        {
            if (spots.rows[y].row[x + 1] != cube.GetComponent<ObjectScript>().nextToRoomNum && isNextToRoom == false)
            {
                isNextToRoom = false;
            }
            else
            {
                isNextToRoom = true;
            }
        }

        if (x > 0)
        {
            if (spots.rows[y].row[x - 1] != cube.GetComponent<ObjectScript>().nextToRoomNum && isNextToRoom == false)
            {
                isNextToRoom = false;
            }
            else
            {
                isNextToRoom = true;
            }
        }
    }
}


//Functions to save the layout to a file and to reupload it back inm, save function

//public void LayoutToFile() //takes 2d list of objects locations and turns it into a file to be uploaded
//{
//    string str = "";

//    for (int i = 0; i < 10; i++) //goes through y of grid
//    {
//        for (int j = 0; j < 10; j++) //goes through x of grid 
//        {
//            str = str + spots.rows[i].row[j] + ","; //adds to str
//        }
//        str = str + "\n"; //new line for each x row
//    }

//    File.WriteAllText("D:/OSF Unity Projects/Image Upload/Assets/Files/drawers.txt", str); //put in file
//}

//public void FileToLayout() //turn the file back into the 2d list
//{
//    //gets file
//    string text = File.ReadAllText("D:/OSF Unity Projects/Image Upload/Assets/Files/drawers.txt"); 
//    string[] val = new string[100];
//    int valCount = 0;

//    StreamReader reader = new StreamReader("D:/OSF Unity Projects/Image Upload/Assets/Files/drawers.txt");
//    string itemStrings = reader.ReadLine();
//    char[] delimiter = { ',' };

//    for (int i = 0; i < 10; i++) //goes through y of list in file 
//    {
//        for(int j = 0; j < 10; j++) //goes through x of list in file
//        {
//            val = text.Split(delimiter); //splits eveything up into one single array
//        }
//    }

//    for (int i = 0; i < 10; i++) //goes through y of grid
//    {
//        for (int j = 0; j < 10; j++) //goes through x of grid
//        {
//            spots.rows[i].row[j] = int.Parse(val[valCount]); //adds each number from array into the 2d list
//            valCount++;
//        }
//    }
//}

//public void SpawnFromLayout() //when layout is loaded spawn all the objects in the right postition
//{
//    int temp = 0; //int check to see if an object needs to be created

//    for (int i = 9; i > 0; i--) //starts on the 10th y column and counts down
//    {
//        for (int j = 0; j < 10; j++) //starts on 0th x row and counts up
//        {
//            temp = spots.rows[i].row[j]; //object number

//            if (temp > 0) //if zero or negative skip
//            {
//                GameObject g = PrefabList.objectList[temp - 1][0];

//                if(g.transform.GetChild(0).GetComponent<ObjectScript>().shapeType == 1) //1x1 shape type
//                {
//                    GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity); //spwans object 
//                    gg.transform.rotation = Quaternion.Euler(0, 0, 0); //makes sure rotation isnt wack

//                    spots.rows[i].row[j] = -temp; //switches number on grid so that it isn't checked again

//                    //turns off the edit and deletebuttons, makes it transparent, and calls a placement function made just for loading
//                    // objects into the drawer so that they are not moveable 
//                    gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                    gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                    gg.GetComponent<ObjectMover>().LayoutPlacement();

//                }

//                if (g.transform.GetChild(0).GetComponent<ObjectScript>().shapeType == 2) //1x2 shape type
//                {
//                    if (spots.rows[i].row[j + 1] == temp) //if it is rotated
//                    {
//                        GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity);
//                        gg.transform.rotation = Quaternion.Euler(0, 0, 0);

//                        gg.transform.GetChild(0).transform.Rotate(0, 90, 0); //rotates spawn
//                        gg.transform.GetChild(0).transform.position += gg.transform.GetComponentInChildren<ObjectScript>().rotAdjustVal; //accounts for rotate adjust
//                        gg.transform.GetComponentInChildren<ObjectScript>().rotAdjust = true;

//                        spots.rows[i].row[j] = -temp; //changes both spots to negative so that it doesn't run again when it hits second half
//                        spots.rows[i].row[j + 1] = -temp;

//                        gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                        gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                        gg.GetComponent<ObjectMover>().LayoutPlacement();
//                    }

//                    else //not rotated
//                    {
//                        GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity);
//                        gg.transform.rotation = Quaternion.Euler(0, 0, 0);

//                        spots.rows[i].row[j] = -temp;
//                        spots.rows[i - 1].row[j] = -temp;

//                        gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                        gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                        gg.GetComponent<ObjectMover>().LayoutPlacement();
//                    }
//                }

//                if(g.transform.GetChild(0).GetComponent<ObjectScript>().shapeType == 3) //1x3 shape type
//                {
//                    if (spots.rows[i].row[j + 1] == temp) //if it is rotated
//                    {
//                        GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity);
//                        gg.transform.rotation = Quaternion.Euler(0, 0, 0);

//                        gg.transform.GetChild(0).transform.Rotate(0, 90, 0);
//                        gg.transform.GetChild(0).transform.position += gg.transform.GetComponentInChildren<ObjectScript>().rotAdjustVal;
//                        gg.transform.GetComponentInChildren<ObjectScript>().rotAdjust = true;

//                        spots.rows[i].row[j] = -temp;
//                        spots.rows[i].row[j + 1] = -temp;
//                        spots.rows[i].row[j + 2] = -temp;

//                        gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                        gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                        gg.GetComponent<ObjectMover>().LayoutPlacement();
//                    }

//                    else //not rotated
//                    {
//                        GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity);
//                        gg.transform.rotation = Quaternion.Euler(0, 0, 0);

//                        spots.rows[i].row[j] = -temp;
//                        spots.rows[i - 1].row[j] = -temp;
//                        spots.rows[i - 2].row[j] = -temp;

//                        gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                        gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                        gg.GetComponent<ObjectMover>().LayoutPlacement();
//                    }
//                }

//                if (g.transform.GetChild(0).GetComponent<ObjectScript>().shapeType == 4)
//                {
//                    if (spots.rows[i - 1].row[j] == temp && spots.rows[i].row[j + 1] == temp && spots.rows[i - 1].row[j + 1] == temp)
//                    {
//                        GameObject gg = Instantiate(g, new Vector3(j, 0, i), Quaternion.identity);
//                        gg.transform.rotation = Quaternion.Euler(0, 0, 0);

//                        spots.rows[i].row[j] = -temp;
//                        spots.rows[i - 1].row[j] = -temp;
//                        spots.rows[i].row[j + 1] = -temp;
//                        spots.rows[i - 1].row[j + 1] = -temp;

//                        gg.transform.GetComponentInChildren<ObjectScript>().SetButtons();
//                        gg.transform.GetComponentInChildren<ObjectScript>().SetColor();
//                        gg.GetComponent<ObjectMover>().LayoutPlacement();
//                    }

//                }
//            }
//        }
//    }
//    //calls this again to make sure all the numbers in the 2d list are correct
//    FileToLayout();
//}
