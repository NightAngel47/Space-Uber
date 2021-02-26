using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildingBuyableRoom : MonoBehaviour
{
    public GameObject roomPrefab;

    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;

    private GameObject objectsToSpawn;

    private void Start()
    {
        objectsToSpawn = GameObject.FindGameObjectWithTag("ObjectsToSpawn");
    }

    public void UpdateRoomInfo()
    {
        roomImage.sprite = roomPrefab.GetComponentInChildren<SpriteRenderer>().sprite;

        rname.text = roomPrefab.GetComponent<RoomStats>().roomName;
        needsCredits.text = "" + roomPrefab.GetComponent<RoomStats>().price;
        needsPower.text = "" + roomPrefab.GetComponent<RoomStats>().minPower;
        needsCrew.text = "" + roomPrefab.GetComponent<RoomStats>().minCrew;
        producesResource.text = roomPrefab.GetComponent<Resource>().resourceType.name;
        producesAmount.text = "" + roomPrefab.GetComponent<Resource>().amount;
    }

    public void SpawnSelectedPrefab()
    {
        objectsToSpawn.GetComponent<SpawnObject>().SpawnRoom(roomPrefab);
    }


}
