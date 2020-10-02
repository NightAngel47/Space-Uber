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

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> allRoomList = new List<GameObject>();
    [SerializeField] private List<GameObject> availableRooms = new List<GameObject>();
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonPanel;

    public void Start()
    {
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
            GameObject g = Instantiate(buttonPrefab);
            g.transform.SetParent(buttonPanel.transform);
            g.GetComponent<Button>().onClick.AddListener(() => SpawnRoom(room));
            g.transform.GetChild(0).gameObject.GetComponent<Text>().text = room.name;
        }
    }

    public void SpawnRoom(GameObject ga)
    {
        ObjectMover.hasPlaced = false;
        GameObject g = Instantiate(ga, new Vector3(1, -1, 0), Quaternion.identity);
    }
}
