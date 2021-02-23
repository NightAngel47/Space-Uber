using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipBuildingUIController : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;

    [SerializeField] Sprite roomImage;
    [SerializeField] TextMeshProUGUI rname;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;

    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;

    private Image roomImageTemp;
    private string nameTemp;
    private int needsCreditsTemp;
    private int needsPowerTemp;
    private int needsCrewTemp;

    private string producesResourceTemp;
    private int producesAmountTemp;

    private void Start()
    {
        nameTemp = roomPrefab.GetComponent<RoomStats>().roomName;
        needsCreditsTemp = roomPrefab.GetComponent<RoomStats>().price;
        needsPowerTemp = roomPrefab.GetComponent<RoomStats>().minPower;
        needsCrewTemp = roomPrefab.GetComponent<RoomStats>().minCrew;
        producesResourceTemp = roomPrefab.GetComponent<Resource>().resourceType.name;
        producesAmountTemp = roomPrefab.GetComponent<Resource>().amount;

    }

    private void Update()
    {
        rname.text = nameTemp;
        needsCredits.text = "" + needsCreditsTemp;
        needsPower.text = "" + needsPowerTemp;
        needsCrew.text = "" + needsCrewTemp;
        producesResource.text = producesResourceTemp;
        producesAmount.text = "" + producesAmountTemp;

    }

    
}
