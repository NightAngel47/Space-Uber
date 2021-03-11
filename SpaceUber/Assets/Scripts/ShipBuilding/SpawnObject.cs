/*
 * SpawnObject.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Functions to spawn each of the rooms for buttons
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> allRoomList = new List<GameObject>();
    public List<GameObject> availableRooms = new List<GameObject>();
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private Vector2 spawnLoc;
    public GameObject powercore;
    public static bool donePreplacedRoom = false;

    private GameObject lastSpawned;

    public string[] purchasePowerCore;
    public string[] purchaseHydroponics;
    public string[] purchaseBrig;
    public string[] purchaseStorage;
    public string[] purchaseBunks;
    public string[] purchaseMedbay;
    public string[] purchaseVIP;
    public string[] purchaseArmor;
    public string[] purchaseGuns;
    public string[] purchaseCoreTerminal;
    public string[] purchaseEnergyCannon;
    public string[] purchasePhotonTorpedoes;
    public string[] purchasePantry;
    public string[] purchaseShieldGenerator;
    public string[] purchaseTeleporter;
    public string[] purchaseWarpDrive;
    public string[] cannotPlaceCredits;
    public string[] cannotPlaceEnergy;

    public IEnumerator Start()
    {
        RectTransform rt = buttonPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 280 * availableRooms.Count);

        //hard coded preplaced rooms to be updated later
        if (donePreplacedRoom == false)
        {
            donePreplacedRoom = true;
            
            yield return new WaitUntil(() => GameManager.instance.hasLoadedRooms);

            // Check if power core has already been placed
            if (!FindObjectsOfType<ObjectScript>().Any(objectScript => objectScript.preplacedRoom && objectScript.GetComponent<RoomStats>().roomName.Equals(powercore.GetComponent<RoomStats>().roomName)))
            {
                // if the power core hasn't been placed then place a power core
                ObjectMover.hasPlaced = false;
                lastSpawned = Instantiate(powercore, new Vector3(4, 2, 0), Quaternion.identity);
                lastSpawned.GetComponent<ObjectMover>().TurnOffBeingDragged();
                lastSpawned.GetComponent<ObjectScript>().preplacedRoom = true;
                ObjectMover.hasPlaced = true;

                StartCoroutine(PreplacedRoom());
            }
        }

        CreateRoomSpawnButtons(); 
    }

    IEnumerator PreplacedRoom()
    {
        yield return new WaitForSeconds(.25f);
        //hard coded preplaced rooms to be updated lated
        FindObjectOfType<SpotChecker>().FillPreplacedSpots(lastSpawned);
        lastSpawned.GetComponent<RoomStats>().AddRoomStats();
        lastSpawned.GetComponent<ObjectMover>().enabled = false;
        lastSpawned.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        
        FindObjectOfType<ShipStats>().SaveShipStats();
        SavingLoadingManager.instance.SaveRooms();
    }

    public void SetAvailableRoomList(List<GameObject> l)
    {
        availableRooms = new List<GameObject>(l);
    }

    public void CreateRoomSpawnButtons()
    {
        foreach (GameObject room in availableRooms)
        {
            //g is the button that is created
            GameObject roomButton = Instantiate(buttonPrefab, buttonPanel.transform);
            //g.transform.SetParent(buttonPanel.transform);
            roomButton.GetComponent<Button>().onClick.AddListener(() => SpawnRoom(room)); //spawn a room upon clicking the button
            roomButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = room.name; //Set G's title to the room's name
            roomButton.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = room.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            roomButton.GetComponentInChildren<ShopTooltipUI>().SetRoomInfo(room.GetComponent<RoomStats>());
        }
    }

    public void SpawnRoom(GameObject ga)
    {
        if (FindObjectOfType<ShipStats>().Credits >= ga.GetComponent<RoomStats>().price[ga.GetComponent<RoomStats>().GetRoomLevel() - 1] && 
            FindObjectOfType<ShipStats>().EnergyRemaining.x >= ga.GetComponent<RoomStats>().minPower[ga.GetComponent<RoomStats>().GetRoomLevel() - 1]) //checks to see if the player has enough credits for the room
        {
            if (lastSpawned == null || lastSpawned.GetComponent<ObjectMover>().enabled == false) //makes sure that the prior room is placed before the next room can be added
            {
                ObjectMover.hasPlaced = false;
                lastSpawned = Instantiate(ga, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
                Cursor.visible = false;
                //HOVER UI does not happen when mouse is hidden
                lastSpawned.GetComponent<ObjectMover>().TurnOnBeingDragged();

                //rooms being placed will appear on top of other rooms that are already placed
                lastSpawned.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

                ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
                ObjectScript.CalledFromSpawn = true;
                foreach (ObjectScript r in otherRooms)
                {
                    r.TurnOffClickAgain();
                }

                if(lastSpawned.GetComponent<ObjectScript>().needsSpecificLocation == true)
                {
                    lastSpawned.GetComponent<ObjectScript>().HighlightSpotsOn();
                }

                if(lastSpawned.GetComponent<ObjectScript>().nextToRoom == true)
                {
                    foreach (ObjectScript r in otherRooms)
                    {
                        if(lastSpawned.GetComponent<ObjectScript>().nextToRoomNum == r.objectNum)
                        {
                            NextToRoomHighlight(r.gameObject);
                        }
                    }
                }

                switch (ga.name) //plays sfx for each room
                {
                    case "Power Core":
                        AudioManager.instance.PlaySFX(purchasePowerCore[UnityEngine.Random.Range(0, purchasePowerCore.Length)]);
                        break;
                    case "Hydroponics":
                        AudioManager.instance.PlaySFX(purchaseHydroponics[UnityEngine.Random.Range(0, purchaseHydroponics.Length)]);
                        break;
                    case "Brig":
                        AudioManager.instance.PlaySFX(purchaseBrig[UnityEngine.Random.Range(0, purchaseBrig.Length)]);
                        break;
                    case "Storage":
                        AudioManager.instance.PlaySFX(purchaseStorage[UnityEngine.Random.Range(0, purchaseStorage.Length)]);
                        break;
                    case "Bunks":
                        AudioManager.instance.PlaySFX(purchaseBunks[UnityEngine.Random.Range(0, purchaseBunks.Length)]);
                        break;
                    case "Medbay":
                        AudioManager.instance.PlaySFX(purchaseMedbay[UnityEngine.Random.Range(0, purchaseMedbay.Length)]);
                        break;
                    case "VIP Lounge":
                        AudioManager.instance.PlaySFX(purchaseVIP[UnityEngine.Random.Range(0, purchaseVIP.Length)]);
                        break;
                    case "Armor Plating":
                        AudioManager.instance.PlaySFX(purchaseArmor[UnityEngine.Random.Range(0, purchaseArmor.Length)]);
                        break;
                    case "Armory":
                        AudioManager.instance.PlaySFX(purchaseGuns[UnityEngine.Random.Range(0, purchaseGuns.Length)]);
                        break;
                    case "Core Charging Terminal":
                        AudioManager.instance.PlaySFX(purchaseCoreTerminal[UnityEngine.Random.Range(0, purchaseCoreTerminal.Length)]);
                        break;
                    case "Energy Cannon":
                        AudioManager.instance.PlaySFX(purchaseEnergyCannon[UnityEngine.Random.Range(0, purchaseEnergyCannon.Length)]);
                        break;
                    case "Photon Torpedoes":
                        AudioManager.instance.PlaySFX(purchasePhotonTorpedoes[UnityEngine.Random.Range(0, purchasePhotonTorpedoes.Length)]);
                        break;
                    case "Pantry":
                        AudioManager.instance.PlaySFX(purchasePantry[UnityEngine.Random.Range(0, purchasePantry.Length)]);
                        break;
                    case "Shield Generator":
                        AudioManager.instance.PlaySFX(purchaseShieldGenerator[UnityEngine.Random.Range(0, purchaseShieldGenerator.Length)]);
                        break;
                    case "Teleportation Station":
                        AudioManager.instance.PlaySFX(purchaseTeleporter[UnityEngine.Random.Range(0, purchaseTeleporter.Length)]);
                        break;
                    case "Warp Drive":
                        AudioManager.instance.PlaySFX(purchaseWarpDrive[UnityEngine.Random.Range(0, purchaseWarpDrive.Length)]);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (FindObjectOfType<ShipStats>().Credits < ga.GetComponent<RoomStats>().price[ga.GetComponent<RoomStats>().GetRoomLevel() - 1])
            {
                AudioManager.instance.PlaySFX(cannotPlaceCredits[UnityEngine.Random.Range(0, cannotPlaceCredits.Length)]);
                //Debug.Log("Cannot Afford");
                FindObjectOfType<ShipBuildingAlertWindow>().OpenAlert(GameManager.instance.GetResourceData((int) ResourceDataTypes._Credits));
            }

            if (FindObjectOfType<ShipStats>().EnergyRemaining.x < ga.GetComponent<RoomStats>().minPower[ga.GetComponent<RoomStats>().GetRoomLevel() - 1])
            {
                AudioManager.instance.PlaySFX(cannotPlaceEnergy[UnityEngine.Random.Range(0, cannotPlaceEnergy.Length)]);
                //Debug.Log("No Energy");
                FindObjectOfType<ShipBuildingAlertWindow>().OpenAlert(GameManager.instance.GetResourceData((int) ResourceDataTypes._Energy));
            }
        }
    }

    public void NextToRoomHighlight(GameObject cube)
    {
        int shapeType = cube.GetComponent<ObjectScript>().shapeType;
        GameObject gridPosBase = cube.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        List<Vector2> gridSpots = new List<Vector2>(cube.GetComponent<ObjectScript>().shapeData.gridSpaces);
        ArrayLayoutGameobject spots = FindObjectOfType<HighlightSpots>().highlights;

        int x = 0;
        int y = 0;

        for (int i = 0; i < gridSpots.Count; i++)
        {
            if (shapeType == 2) //these objects only have two different rotations
            {
                if (cube.GetComponent<ObjectScript>().rotAdjust == 1 || cube.GetComponent<ObjectScript>().rotAdjust == 3)
                {
                    y = (int)Math.Round(cube.transform.position.y + gridSpots[i].y);
                    x = (int)Math.Round(cube.transform.position.x + gridSpots[i].x);
                }

                if (cube.GetComponent<ObjectScript>().rotAdjust == 2 || cube.GetComponent<ObjectScript>().rotAdjust == 4)
                {
                    y = ((int)Math.Round(cube.transform.position.y + gridSpots[i].x));
                    x = (int)Math.Round(cube.transform.position.x + gridSpots[i].y);
                }
            }

            if (cube.GetComponent<ObjectScript>().rotAdjust == 1)
            {
                y = ((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].y));
                x = (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].x);
            }

            if (cube.GetComponent<ObjectScript>().rotAdjust == 2)
            {
                y = ((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].x - 1));
                x = (int)Math.Round(gridPosBase.transform.position.x + gridSpots[i].y);
            }

            if (cube.GetComponent<ObjectScript>().rotAdjust == 3)
            {
                y = ((int)Math.Round(gridPosBase.transform.position.y - gridSpots[i].y - 1));
                x = (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].x - 1);
            }

            if (cube.GetComponent<ObjectScript>().rotAdjust == 4)
            {
                y = ((int)Math.Round(gridPosBase.transform.position.y + gridSpots[i].x));
                x = (int)Math.Round(gridPosBase.transform.position.x - gridSpots[i].y - 1);
            }

        
            if (y < 5) //# needs to change to dynamically update with different ship sizes
            {
                spots.rows[y + 1].row[x].gameObject.SetActive(true);
            }
            else if(y < 5)
            {
                spots.rows[y + 1].row[x].gameObject.SetActive(false);
            }

            if (y > 0)
            {
                spots.rows[y - 1].row[x].gameObject.SetActive(true);
            }
            else if(y > 0)
            {
                spots.rows[y - 1].row[x].gameObject.SetActive(false);
            }

            if (x < 8) //# needs to change to dynamically update with different ship sizes
            {
                spots.rows[y].row[x + 1].gameObject.SetActive(true);
            }
            else if(x < 8)
            {
                spots.rows[y].row[x + 1].gameObject.SetActive(false);
            }

            if (x > 0)
            {
                spots.rows[y].row[x - 1].gameObject.SetActive(true);
            }
            else if(x > 0)
            {
                spots.rows[y].row[x - 1].gameObject.SetActive(false);
            }
        }
    }
}
