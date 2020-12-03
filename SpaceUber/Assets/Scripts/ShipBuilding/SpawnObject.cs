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

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> allRoomList = new List<GameObject>();
    [SerializeField] private List<GameObject> availableRooms = new List<GameObject>();
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

    public void Start()
    {
        RectTransform rt = buttonPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 280 * availableRooms.Count);

        //hard coded preplaced rooms to be updated later
        if (donePreplacedRoom == false)
        {
            donePreplacedRoom = true;
            ObjectMover.hasPlaced = false;
            lastSpawned = Instantiate(powercore, new Vector3(4, 2, 0), Quaternion.identity);
            lastSpawned.GetComponent<ObjectMover>().TurnOffBeingDragged();
            lastSpawned.GetComponent<ObjectScript>().preplacedRoom = true;
            ObjectMover.hasPlaced = true;

            StartCoroutine(PreplacedRoom());
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
        if (FindObjectOfType<ShipStats>().Credits >= ga.GetComponent<RoomStats>().price && FindObjectOfType<ShipStats>().EnergyRemaining >= ga.GetComponent<RoomStats>().minPower) //checks to see if the player has enough credits for the room
        {
            if (lastSpawned == null || lastSpawned.GetComponent<ObjectMover>().enabled == false) //makes sure that the prior room is placed before the next room can be added
            {
                ObjectMover.hasPlaced = false;
                lastSpawned = Instantiate(ga, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
                lastSpawned.GetComponent<ObjectMover>().TurnOnBeingDragged();

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
                    if(GameObject.Find(lastSpawned.GetComponent<ObjectScript>().nextToRoomName + "(Clone)") != null)
                    {
                        GameObject r = GameObject.Find(lastSpawned.GetComponent<ObjectScript>().nextToRoomName + "(Clone)");

                        //r.transform.GetChild(2).gameObject.SetActive(true);

                        NextToRoomHighlight(r);
                    }
                }

                switch (ga.name) //plays sfx for each room
                {
                    case "Power Core":
                        AudioManager.instance.PlaySFX(purchasePowerCore[UnityEngine.Random.Range(0, purchasePowerCore.Length - 1)]);
                        break;
                    case "Hydroponics":
                        AudioManager.instance.PlaySFX(purchaseHydroponics[UnityEngine.Random.Range(0, purchaseHydroponics.Length - 1)]);
                        break;
                    case "Brig":
                        AudioManager.instance.PlaySFX(purchaseBrig[UnityEngine.Random.Range(0, purchaseBrig.Length - 1)]);
                        break;
                    case "Storage":
                        AudioManager.instance.PlaySFX(purchaseStorage[UnityEngine.Random.Range(0, purchaseStorage.Length - 1)]);
                        break;
                    case "Bunks":
                        AudioManager.instance.PlaySFX(purchaseBunks[UnityEngine.Random.Range(0, purchaseBunks.Length - 1)]);
                        break;
                    case "Medbay":
                        AudioManager.instance.PlaySFX(purchaseMedbay[UnityEngine.Random.Range(0, purchaseMedbay.Length - 1)]);
                        break;
                    case "VIP Lounge":
                        AudioManager.instance.PlaySFX(purchaseVIP[UnityEngine.Random.Range(0, purchaseVIP.Length - 1)]);
                        break;
                    case "Armor Plating":
                        AudioManager.instance.PlaySFX(purchaseArmor[UnityEngine.Random.Range(0, purchaseArmor.Length - 1)]);
                        break;
                    case "Armory":
                        AudioManager.instance.PlaySFX(purchaseGuns[UnityEngine.Random.Range(0, purchaseGuns.Length - 1)]);
                        break;
                    case "Core Changing Terminal":
                        AudioManager.instance.PlaySFX(purchaseCoreTerminal[UnityEngine.Random.Range(0, purchaseCoreTerminal.Length - 1)]);
                        break;
                    case "Energy Cannon":
                        AudioManager.instance.PlaySFX(purchaseEnergyCannon[UnityEngine.Random.Range(0, purchaseEnergyCannon.Length - 1)]);
                        break;
                    case "Photon Torpedoes":
                        AudioManager.instance.PlaySFX(purchasePhotonTorpedoes[UnityEngine.Random.Range(0, purchasePhotonTorpedoes.Length - 1)]);
                        break;
                    case "Pantry":
                        AudioManager.instance.PlaySFX(purchasePantry[UnityEngine.Random.Range(0, purchasePantry.Length - 1)]);
                        break;
                    case "Shield Generator":
                        AudioManager.instance.PlaySFX(purchaseShieldGenerator[UnityEngine.Random.Range(0, purchaseShieldGenerator.Length - 1)]);
                        break;
                    case "Teleportation Station":
                        AudioManager.instance.PlaySFX(purchaseTeleporter[UnityEngine.Random.Range(0, purchaseTeleporter.Length - 1)]);
                        break;
                    case "Warp Drive":
                        AudioManager.instance.PlaySFX(purchaseWarpDrive[UnityEngine.Random.Range(0, purchaseWarpDrive.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (FindObjectOfType<ShipStats>().Credits < ga.GetComponent<RoomStats>().price)
            {
                AudioManager.instance.PlaySFX(cannotPlaceCredits[UnityEngine.Random.Range(0, cannotPlaceCredits.Length)]);
                Debug.Log("Cannot Afford");
                FindObjectOfType<ShipStats>().cantPlaceText.gameObject.SetActive(true);
                FindObjectOfType<ShipStats>().cantPlaceText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<ShipStats>().statIcons[0];
                StartCoroutine(WaitForText());
            }

            if (FindObjectOfType<ShipStats>().EnergyRemaining < ga.GetComponent<RoomStats>().minPower)
            {
                AudioManager.instance.PlaySFX(cannotPlaceEnergy[UnityEngine.Random.Range(0, cannotPlaceEnergy.Length)]);
                Debug.Log("No Energy");
                FindObjectOfType<ShipStats>().cantPlaceText.gameObject.SetActive(true);
                FindObjectOfType<ShipStats>().cantPlaceText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<ShipStats>().statIcons[5];
                StartCoroutine(WaitForText());
            }
        }
    }

    public IEnumerator WaitForText()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<ShipStats>().cantPlaceText.SetActive(false);
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
