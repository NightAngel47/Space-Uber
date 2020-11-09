/*
 * RoomTooltipUI.cs
 * Author(s): Steven Drovie []
 * Created on: 11/8/2020 (en-US)
 * Description: 
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomTooltipUI : MonoBehaviour
{
    [SerializeField] private RoomStats roomStats;
    
    [SerializeField] private TMP_Text roomNameUI;
    [SerializeField] private TMP_Text roomDescUI;
    
    [SerializeField] private TMP_Text roomPrice;
    [SerializeField] private TMP_Text roomSellPrice;
    [SerializeField] private GameObject roomUsedImg;
    [SerializeField] private TMP_Text roomPower;
    [SerializeField] private TMP_Text roomCrew;
    
    [SerializeField] private Transform statsUI;
    [SerializeField] private GameObject resourceUI;
    
    private void Start()
    {
        roomNameUI.text = roomStats.roomName;
        roomDescUI.text = roomStats.roomDescription;
        roomPrice.text = roomStats.price.ToString();
        roomSellPrice.text = roomStats.price.ToString();
        roomUsedImg.SetActive(roomStats.usedRoom);
        if (roomStats.maxPower <= 0)
        {
            roomPower.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            roomPower.text = $"{roomStats.minPower}-{roomStats.maxPower}";
        }
        if (roomStats.maxCrew <= 0)
        {
            roomCrew.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            roomCrew.text = $"{roomStats.minCrew}-{roomStats.maxCrew}";
        }

        foreach (var resource in roomStats.resources)
        {
            GameObject resourceGO = Instantiate(resourceUI, statsUI);
            resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = resource.resourceIcon; // resource icon
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = resource.resourceType; // resource name
            resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.amount.ToString(); // resource amount
        }
    }

    public void RoomIsUsed()
    {
        roomSellPrice.text = ((int)(roomStats.price * roomStats.priceReducationPercent)).ToString();
        roomUsedImg.SetActive(roomStats.usedRoom);
    }
}
