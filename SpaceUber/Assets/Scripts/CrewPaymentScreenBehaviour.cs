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
        
        // continue to next job, or end game
        if(campaignManager.cateringToTheRich.currentCampaignJobIndex < campaignManager.cateringToTheRich.campaignJobs.Count)
        {
            GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
        }
        else if(false)
        {
            // once the campaignManager refactor is done there should be an option here to continue to the next campaign
        }
        else
        {
            GameManager.instance.ChangeInGameState(InGameStates.MoneyEnding);
        }
    }
}
