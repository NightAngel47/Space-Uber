/*
 * EditCrewButton.cs
 * Author(s): Steven Drovie []
 * Created on: 11/9/2020 (en-US)
 * Description: 
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EditCrewButton : MonoBehaviour
{
    private ButtonTwoBehaviour editCrewButton;
    [SerializeField] private int minRoomPlacedToContinue = 1;

    private void Awake()
    {
        editCrewButton = GetComponent<ButtonTwoBehaviour>();
        editCrewButton.SetButtonInteractable(false);
    }

    private void Start()
    {
        CheckForRoomsCall();
    }

    public void CheckForRoomsCall()
    {
        StartCoroutine(CheckForRooms());
    }

    public IEnumerator CheckForRooms()
    {
        yield return new WaitUntil(() => FindObjectOfType<SpotChecker>());
        editCrewButton.SetButtonInteractable(FindObjectsOfType<RoomStats>().Length > minRoomPlacedToContinue);
    }
}
