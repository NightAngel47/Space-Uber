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
    
    private void Start()
    {
        roomNameUI.text = roomStats.roomName;
        roomDescUI.text = roomStats.roomDescription;
    }

    private void Update()
    {
        if (gameObject.activeSelf && EventSystem.instance.eventActive)
        {
            gameObject.SetActive(false);
        }
    }
}
