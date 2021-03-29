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
    private TMP_Text jobPayText;
    private TMP_Text roomPayText;

    private ShipStats ship;

    void Start()
    {
        ship = FindObjectOfType<ShipStats>();
        
        int payment = FindObjectOfType<JobManager>().
    }

    public void GetPaid()
    {
        ship.CashPayout();
    }
}
