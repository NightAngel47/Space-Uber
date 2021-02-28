using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewPaymentScreenBehaviour : MonoBehaviour
{
    private ShipStats ship;
    private CampaignManager campaignManager;
    
    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
        campaignManager = FindObjectOfType<CampaignManager>();
    }
    
    public void PayCrew(int amount)
    {
        // pay crew
        ship.PayCrew(amount);
        
        ship.SaveStats();
        
        campaignManager.GoToNextJob(); //tells campaign manager to activate the next available job
    }
}
