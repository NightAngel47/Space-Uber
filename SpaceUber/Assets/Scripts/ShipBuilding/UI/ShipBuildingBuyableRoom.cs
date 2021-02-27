using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildingBuyableRoom : MonoBehaviour
{
    public GameObject roomPrefab;
    public Image resourceIcon;

    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI roomSize;
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
        roomSize.text = roomPrefab.GetComponent<ObjectScript>().roomSize;
        needsCredits.text = "" + roomPrefab.GetComponent<RoomStats>().price;
        needsPower.text = "" + roomPrefab.GetComponent<RoomStats>().minPower;
        needsCrew.text = "" + roomPrefab.GetComponent<RoomStats>().minCrew;

        if(roomPrefab.GetComponent<Resource>() != null)
        {
            producesResource.text = roomPrefab.GetComponent<Resource>().resourceType.name.Substring(1);
            producesAmount.text = "" + roomPrefab.GetComponent<Resource>().amount;
        }
        else
        {
            producesResource.text = "No production";
            producesAmount.text = "";
        }
        
    }

    public void SpawnSelectedPrefab()
    {
        objectsToSpawn.GetComponent<SpawnObject>().SpawnRoom(roomPrefab);
    }


}
