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

    //TONS of copy+paste arrays. These might be necessary, but the stuff farther down isn't. Need to ask a real programmer for help about better implementation. - Jake
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
        CreateRoomSpawnButtons();
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
            GameObject g = Instantiate(buttonPrefab, buttonPanel.transform);
            //g.transform.SetParent(buttonPanel.transform);
            g.GetComponent<Button>().onClick.AddListener(() => SpawnRoom(room)); //spawn a room upon clicking the button
            g.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = room.name; //Set G's title to the room's name
            g.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = room.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void SpawnRoom(GameObject ga)
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(ga, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
            g.GetComponent<ObjectMover>().TurnOnBeingDragged();

            ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
            foreach(ObjectScript r in otherRooms)
            {
                r.TurnOffClickAgain();
            }

            switch (ga.name)
            {
                case "Power Core":
                    AudioManager.instance.PlaySFX(purchasePowerCore[Random.Range(0, purchasePowerCore.Length)]);
                    break;
                case "Hydroponics":
                    AudioManager.instance.PlaySFX(purchaseHydroponics[Random.Range(0, purchaseHydroponics.Length)]);
                    break;
                case "Brig":
                    AudioManager.instance.PlaySFX(purchaseBrig[Random.Range(0, purchaseBrig.Length)]);
                    break;
                case "Storage":
                    AudioManager.instance.PlaySFX(purchaseStorage[Random.Range(0, purchaseStorage.Length)]);
                    break;
                case "Bunks":
                    AudioManager.instance.PlaySFX(purchaseBunks[Random.Range(0, purchaseBunks.Length)]);
                    break;
                case "Medbay":
                    AudioManager.instance.PlaySFX(purchaseMedbay[Random.Range(0, purchaseMedbay.Length)]);
                    break;
                case "VIP Lounge":
                    AudioManager.instance.PlaySFX(purchaseVIP[Random.Range(0, purchaseVIP.Length)]);
                    break;
                case "Armor Plating":
                    AudioManager.instance.PlaySFX(purchaseArmor[Random.Range(0, purchaseArmor.Length)]);
                    break;
                case "Armory":
                    AudioManager.instance.PlaySFX(purchaseGuns[Random.Range(0, purchaseGuns.Length)]);
                    break;
                case "Core Changing Terminal":
                    AudioManager.instance.PlaySFX(purchaseCoreTerminal[Random.Range(0, purchaseCoreTerminal.Length)]);
                    break;
                case "Energy Cannon":
                    AudioManager.instance.PlaySFX(purchaseEnergyCannon[Random.Range(0, purchaseEnergyCannon.Length)]);
                    break;
                case "Photon Torpedoes":
                    AudioManager.instance.PlaySFX(purchasePhotonTorpedoes[Random.Range(0, purchasePhotonTorpedoes.Length)]);
                    break;
                case "Pantry":
                    AudioManager.instance.PlaySFX(purchasePantry[Random.Range(0, purchasePantry.Length)]);
                    break;
                case "Shield Generator":
                    AudioManager.instance.PlaySFX(purchaseShieldGenerator[Random.Range(0, purchaseShieldGenerator.Length)]);
                    break;
                case "Teleportation Station":
                    AudioManager.instance.PlaySFX(purchaseTeleporter[Random.Range(0, purchaseTeleporter.Length)]);
                    break;
                case "Warp Drive":
                    AudioManager.instance.PlaySFX(purchaseWarpDrive[Random.Range(0, purchaseWarpDrive.Length)]);
                    break;
                default:
                    break;
            }
        }
    }
}
