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
            GameObject g = Instantiate(buttonPrefab, buttonPanel.transform);
            //g.transform.SetParent(buttonPanel.transform);
            g.GetComponent<Button>().onClick.AddListener(() => SpawnRoom(room));
            g.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = room.name;
        }
    }

    public void SpawnRoom(GameObject ga)
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(ga, new Vector3(spawnLoc.x, spawnLoc.y, 0), Quaternion.identity);
        }
    }
}
