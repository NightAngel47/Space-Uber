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

            //This is some vile copy+paste code. I should not hunt for solutions at 2 AM. Need to ask a real programmer for help about better implementation.
            //I want to customize each SpawnRoom button to have its own array of sound effects, but I'm not sure where to hook those two up other than during 
            //the instantiation of new pieces.
            if (ga.name == "Power Core")
            {
                AudioManager.instance.PlaySFX(purchasePowerCore[Random.Range(0, purchasePowerCore.Length)]);
            }
            if (ga.name == "Hydroponics")
            {
                AudioManager.instance.PlaySFX(purchaseHydroponics[Random.Range(0, purchaseHydroponics.Length)]);
            }
            if (ga.name == "Brig")
            {
                AudioManager.instance.PlaySFX(purchaseBrig[Random.Range(0, purchaseBrig.Length)]);
            }
            if (ga.name == "Storage")
            {
                AudioManager.instance.PlaySFX(purchaseStorage[Random.Range(0, purchaseStorage.Length)]);
            }
            if (ga.name == "Bunks")
            {
                AudioManager.instance.PlaySFX(purchaseBunks[Random.Range(0, purchaseBunks.Length)]);
            }
            if (ga.name == "Medbay")
            {
                AudioManager.instance.PlaySFX(purchaseMedbay[Random.Range(0, purchaseMedbay.Length)]);
            }
        }
    }
}
