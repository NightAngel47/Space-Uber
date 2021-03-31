/*
 * JobPaymentScreen.cs
 * Author(s): Steven Drovie []
 * Created on: 3/29/2021 (en-US)
 * Description: 
 */

using System.Linq;
using TMPro;
using UnityEngine;

public class JobPaymentScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text jobPayText;
    [SerializeField] private TMP_Text roomPayText;
    [SerializeField] private TMP_Text totalPayText;

    private ShipStats ship;

    void Start()
    {
        ship = FindObjectOfType<ShipStats>();
        
        int roomPayout = FindObjectsOfType<RoomStats>().Where(room => room.resources[0].resourceType.Rt == ResourceDataTypes._Payout).Sum(room => room.resources[0].activeAmount);

        int jobPayout = ship.Payout - roomPayout;
        
        EventSystem.instance.ClearEventSystemAtEndOfJob();
        
        jobPayText.text = jobPayout.ToString();
        roomPayText.text = roomPayout.ToString();
        totalPayText.text = ship.Payout.ToString();
    }

    public void GetPaid()
    {
        ship.CashPayout();
        GameManager.instance.ChangeInGameState(InGameStates.CrewPayment);
    }
}
