/*
 * JobPaymentScreen.cs
 * Author(s): Steven Drovie []
 * Created on: 3/29/2021 (en-US)
 * Description: 
 */

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

        int jobPayout = ship.Payout - EventSystem.instance.CurrentJob.payout;

        int roomPayout;
        foreach (RoomStats room in FindObjectsOfType<RoomStats>())
        {
            if(room)
        }
        
        EventSystem.instance.ClearEventSystemAtEndOfJob();
        
        jobPayText.text = ship.Payout.ToString();
        roomPayText.text = (ship.Payout - jobPayout).ToString();
        totalPayText.text = ship.Payout.ToString();
    }

    public void GetPaid()
    {
        ship.CashPayout();
        GameManager.instance.ChangeInGameState(InGameStates.CrewPayment);
    }
}
