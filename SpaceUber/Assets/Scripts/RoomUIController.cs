/*
 * RoomUIController.cs
 * Author(s): Grant Frey
 * Created on: 9/16/2020 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUIController : MonoBehaviour
{
    public GameObject roomPrefab;

    public List<TMP_Text> textList;

    void Start()
    {
        UpdateRoomUI();
    }

    void Update()
    {
        
    }

    public void UpdateRoomUI()
    {
        Resource[] resources = roomPrefab.GetComponents<Resource>();
        textList[0].text = roomPrefab.GetComponent<RoomStats>().roomName;
        for(int i = 0; i < resources.Length; i++)
        {
            if (resources[i] != null)
            {
                textList[i + 1].text = resources[i].resourceType + ": " + resources[i].amount;
            }
        }
        textList[4].text = roomPrefab.GetComponent<RoomStats>().roomDescription;
    }
}
