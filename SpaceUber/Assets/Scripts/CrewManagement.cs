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
    }

    public void CheckForRoomsCall()
    {
        StartCoroutine(CheckForRooms());
    }

    private IEnumerator CheckForRooms()
    {
        yield return new WaitUntil(() => FindObjectOfType<SpotChecker>() && GameManager.instance.hasLoadedRooms);
        passedRoomCount = FindObjectsOfType<RoomStats>().Length > minRoomPlacedToContinue;

        CheckForMinCrew();
    }

    public void CheckForMinCrew()
    {
        sceneButtons[0].GetComponent<ButtonTwoBehaviour>().SetButtonInteractable(passedRoomCount && ObjectMover.hasPlaced && FindObjectsOfType<RoomStats>().All(room => room.minCrew <= room.currentCrew));
    }

    public void ClearRoomDetails()
    {
        FindObjectOfType<CrewManagementRoomDetailsMenu>()?.ClearUI();
    }
}
