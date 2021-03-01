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
    
    public void CheckForRooms()
    {
        editCrewButton.SetButtonInteractable(FindObjectsOfType<RoomStats>().Length > minRoomPlacedToContinue);
    }
}
