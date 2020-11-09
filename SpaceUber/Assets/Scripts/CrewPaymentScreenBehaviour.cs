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
        
        // continue to next job, or end campaign
        if (campaignManager.campaigns[(int) campaignManager.currentCamp].currentCampaignJobIndex <
            campaignManager.campaigns[(int) campaignManager.currentCamp].campaignJobs.Count)
        {
            GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
        }
        else
        {
            GameManager.instance.ChangeInGameState(InGameStates.Ending);
        }
    }
}
