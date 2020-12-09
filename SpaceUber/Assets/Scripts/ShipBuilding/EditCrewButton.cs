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
    private Button editCrewButton;

    private void Awake()
    {
        editCrewButton = GetComponent<Button>();
        editCrewButton.interactable = false;
    }

    private void Start()
    {
        StartCoroutine(CheckForRooms());
    }

    public IEnumerator CheckForRooms()
    {
        yield return new WaitForSeconds(.25f);

        if (FindObjectsOfType<RoomStats>().Length > 0)
        {
            editCrewButton.interactable = true;
        }
        else
        {
            editCrewButton.interactable = false;
        }
    }
}
