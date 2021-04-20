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
    public GameObject[] sceneButtons; //contains finish ship and back to shipbuilding buttons

    [SerializeField] private int minRoomPlacedToContinue = 1;

    private bool passedRoomCount = false;

    public void Start()
    {

        StartCoroutine(CheckForRooms());

        CheckForMinCrew();

        //removed since crew management and shipbuilding are being combined
        //CrewViewManager.Instance.EnableCrewView();//automatically enable crew view when you enter crew mgmt
        
        
    }

    public void CheckForRoomsCall()
    {
        StartCoroutine(CheckForRooms());
    }

    public IEnumerator CheckForRooms()
    {
        yield return new WaitUntil(() => FindObjectOfType<SpotChecker>());
        yield return new WaitUntil(() => GameManager.instance.hasLoadedRooms);
        passedRoomCount = FindObjectsOfType<RoomStats>().Length > minRoomPlacedToContinue;

        CheckForMinCrew();
    }

    public void CheckForMinCrew()
    {
        sceneButtons[0].GetComponent<ButtonTwoBehaviour>().SetButtonInteractable(passedRoomCount && FindObjectsOfType<RoomStats>().All(room => room.minCrew <= room.currentCrew));
    }

    public void ClearRoomDetails()
    {
        FindObjectOfType<CrewManagementRoomDetailsMenu>()?.ClearUI();
    }
}
