/* Frank Calabrese
 * ShipBuildingBuyableRoom.cs
 * Changes what room it holds/displays
 * based off of instructions from ShipBuildingShop.cs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuildingBuyableRoom : MonoBehaviour
{
    public GameObject roomPrefab;
    
    [SerializeField] Image resourceIcon;
    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI roomSize;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;
    [SerializeField] TextMeshProUGUI level;

    private SpawnObject objectsToSpawn;

    private void Start()
    {
        objectsToSpawn = FindObjectOfType<SpawnObject>();
    }

    public void UpdateRoomInfo()
    {
        roomImage.sprite = roomPrefab.GetComponentInChildren<SpriteRenderer>().sprite;

        RoomStats roomStats = roomPrefab.GetComponent<RoomStats>();
        rname.text = roomStats.roomName;
        needsCredits.text = "" + roomStats.price;
        needsPower.text = "" + roomStats.minPower;
        needsCrew.text = "" + roomStats.minCrew + "-" + roomStats.maxCrew.ToString();
        roomSize.text = roomPrefab.GetComponent<ObjectScript>().shapeDataTemplate.roomSizeName;
        level.text = roomPrefab.GetComponent<RoomStats>().GetRoomLevel().ToString();

        if(roomPrefab.TryGetComponent(out Resource resource))
        {
            resourceIcon.sprite = resource.resourceType.resourceIcon;
            producesResource.text = resource.resourceType.resourceName;
            producesAmount.text = "" + resource.amount;
        }
        else
        {
            resourceIcon.gameObject.SetActive(false);
            producesResource.text = "No Production";
            producesAmount.text = "";
        }
    }

    public void SpawnSelectedPrefab()
    {
        objectsToSpawn.SpawnRoom(roomPrefab);
    }
}
