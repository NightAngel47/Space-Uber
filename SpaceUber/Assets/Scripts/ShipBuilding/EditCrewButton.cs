/*
 * EditCrewButton.cs
 * Author(s): Steven Drovie []
 * Created on: 11/9/2020 (en-US)
 * Description: 
 */

using System;
using UnityEngine;
using UnityEngine.UI;

public class EditCrewButton : MonoBehaviour
{
    private Button editCrewButton;

    private void Awake()
    {
        editCrewButton = GetComponent<Button>();
    }

    private void Start()
    {
        CheckForRooms();
    }

    public void CheckForRooms()
    {
        if (FindObjectsOfType<RoomStats>().Length > 0)
        {
            print("true");
            editCrewButton.interactable = true;
        }
        else
        {
            print("false");
            editCrewButton.interactable = false;
        }
    }
}
