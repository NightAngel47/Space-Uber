/*
 * CrewManagement.cs
 * Author(s): Sydney
 * Created on: 10-20-20
 * Description:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrewManagement : MonoBehaviour
{
    private RoomStats[] currentRoomList;

    public GameObject[] sceneButtons; //contains finish ship and back to shipbuilding buttons
    
    public void Start()
    {
        currentRoomList = FindObjectsOfType<RoomStats>();
        
        CheckForMinCrew();

        //removed since crew management and shipbuilding are being combined
        //CrewViewManager.Instance.EnableCrewView();//automatically enable crew view when you enter crew mgmt
        
        
    }

    public void CheckForMinCrew()
    {
        sceneButtons[0].GetComponent<ButtonTwoBehaviour>().SetButtonInteractable(currentRoomList.All(room => room.minCrew <= room.currentCrew));
    }

    public void ClearRoomDetails()
    {
        FindObjectOfType<CrewManagementRoomDetailsMenu>()?.ClearUI();
    }
}
