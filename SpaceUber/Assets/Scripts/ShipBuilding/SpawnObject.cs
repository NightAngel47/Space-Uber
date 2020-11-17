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

    public void Start()
    {
        RectTransform rt = buttonPanel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 280 * availableRooms.Count);

        //hard coded preplaced rooms to be updated lated
        if (donePreplacedRoom == false)
        {
            donePreplacedRoom = true;
            ObjectMover.hasPlaced = false;
            lastSpawned = Instantiate(powercore, new Vector3(4, 2, 0), Quaternion.identity);
            lastSpawned.GetComponent<ObjectMover>().TurnOffBeingDragged();
            lastSpawned.GetComponent<ObjectScript>().preplacedRoom = true;
            
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

                switch (ga.name) //plays sfx for each room
                {
                    case "Power Core":
                        AudioManager.instance.PlaySFX(purchasePowerCore[Random.Range(0, purchasePowerCore.Length - 1)]);
                        break;
                    case "Hydroponics":
                        AudioManager.instance.PlaySFX(purchaseHydroponics[Random.Range(0, purchaseHydroponics.Length - 1)]);
                        break;
                    case "Brig":
                        AudioManager.instance.PlaySFX(purchaseBrig[Random.Range(0, purchaseBrig.Length - 1)]);
                        break;
                    case "Storage":
                        AudioManager.instance.PlaySFX(purchaseStorage[Random.Range(0, purchaseStorage.Length - 1)]);
                        break;
                    case "Bunks":
                        AudioManager.instance.PlaySFX(purchaseBunks[Random.Range(0, purchaseBunks.Length - 1)]);
                        break;
                    case "Medbay":
                        AudioManager.instance.PlaySFX(purchaseMedbay[Random.Range(0, purchaseMedbay.Length - 1)]);
                        break;
                    case "VIP Lounge":
                        AudioManager.instance.PlaySFX(purchaseVIP[Random.Range(0, purchaseVIP.Length - 1)]);
                        break;
                    case "Armor Plating":
                        AudioManager.instance.PlaySFX(purchaseArmor[Random.Range(0, purchaseArmor.Length - 1)]);
                        break;
                    case "Armory":
                        AudioManager.instance.PlaySFX(purchaseGuns[Random.Range(0, purchaseGuns.Length - 1)]);
                        break;
                    case "Core Changing Terminal":
                        AudioManager.instance.PlaySFX(purchaseCoreTerminal[Random.Range(0, purchaseCoreTerminal.Length - 1)]);
                        break;
                    case "Energy Cannon":
                        AudioManager.instance.PlaySFX(purchaseEnergyCannon[Random.Range(0, purchaseEnergyCannon.Length - 1)]);
                        break;
                    case "Photon Torpedoes":
                        AudioManager.instance.PlaySFX(purchasePhotonTorpedoes[Random.Range(0, purchasePhotonTorpedoes.Length - 1)]);
                        break;
                    case "Pantry":
                        AudioManager.instance.PlaySFX(purchasePantry[Random.Range(0, purchasePantry.Length - 1)]);
                        break;
                    case "Shield Generator":
                        AudioManager.instance.PlaySFX(purchaseShieldGenerator[Random.Range(0, purchaseShieldGenerator.Length - 1)]);
                        break;
                    case "Teleportation Station":
                        AudioManager.instance.PlaySFX(purchaseTeleporter[Random.Range(0, purchaseTeleporter.Length - 1)]);
                        break;
                    case "Warp Drive":
                        AudioManager.instance.PlaySFX(purchaseWarpDrive[Random.Range(0, purchaseWarpDrive.Length - 1)]);
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
                Debug.Log("Cannot Afford");
                FindObjectOfType<ShipStats>().cantPlaceText.gameObject.SetActive(true);
                FindObjectOfType<ShipStats>().cantPlaceText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<ShipStats>().statIcons[0];
                StartCoroutine(WaitForText());
            }

            if (FindObjectOfType<ShipStats>().EnergyRemaining < ga.GetComponent<RoomStats>().minPower)
            {
                Debug.Log("No Energy");
                FindObjectOfType<ShipStats>().cantPlaceText.gameObject.SetActive(true);
                FindObjectOfType<ShipStats>().cantPlaceText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<ShipStats>().statIcons[5];
                StartCoroutine(WaitForText());
            }
        }
    }

    public IEnumerator WaitForText()
    {
        yield return new WaitForSeconds(3);
        FindObjectOfType<ShipStats>().cantPlaceText.SetActive(false);
    }
}
