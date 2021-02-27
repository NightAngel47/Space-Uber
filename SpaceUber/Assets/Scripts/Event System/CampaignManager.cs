/*
 * CampaignManager.cs
 * Author(s): Scott Acker
 * Created on: 11/3/2020 (en-US)
 * Description: Keeps information about which campaign a player is currently on.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CampaignManager : MonoBehaviour
{
    public enum Campaigns
    {
        CateringToTheRich,
        MysteriousEntity,
        FinalTest
    }

    [ReadOnly, SerializeField] private Campaigns currentCamp = Campaigns.CateringToTheRich;

    public CateringToTheRich cateringToTheRich = new CateringToTheRich();
    public MysteriousEntity mysteriousEntity = new MysteriousEntity();
    public FinalTest finalTest = new FinalTest();

    /// <summary>
    /// Returns a list of all jobs that are available for the current campaign.
    /// To be used in JobManager
    /// </summary>
    /// <returns></returns>
    public List<Job> GetAvailableJobs()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                return cateringToTheRich.campaignJobs;
            case Campaigns.MysteriousEntity:
                return mysteriousEntity.campaignJobs;
            case Campaigns.FinalTest:
                return finalTest.campaignJobs;
            default:
                Debug.LogError("Current Campaign Available Jobs for " + currentCamp + " not setup.");
                return null;
        }
    }

    public int GetCurrentCampaignIndex()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                return cateringToTheRich.currentCampaignJobIndex;
            case Campaigns.MysteriousEntity:
                return mysteriousEntity.currentCampaignJobIndex;
            case Campaigns.FinalTest:
                return finalTest.currentCampaignJobIndex;
            default:
                Debug.LogError("Current Campaign Index for " + currentCamp + " not setup.");
                return 0;
        }
    }

    private void GoToNextCampaign()
    {
        switch(currentCamp)
        {
            case Campaigns.CateringToTheRich:
                currentCamp = Campaigns.MysteriousEntity;
                GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                break;
            case Campaigns.MysteriousEntity:
                currentCamp = Campaigns.FinalTest;
                GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                break;
            case Campaigns.FinalTest:
                GameManager.instance.ChangeInGameState(InGameStates.MoneyEnding); // go to ending
                break;
            default:
                Debug.LogError("Current Campaign " + currentCamp + " not setup.");
                return;
        }
    }
    public void GoToNextJob()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.currentCampaignJobIndex++;
                
                if (cateringToTheRich.currentCampaignJobIndex >= cateringToTheRich.campaignJobs.Count)
                {
                    GoToNextCampaign();
                }
                else
                {
                    GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                }
                break;

            case Campaigns.MysteriousEntity:
                mysteriousEntity.currentCampaignJobIndex++;
                
                if (mysteriousEntity.currentCampaignJobIndex >= mysteriousEntity.campaignJobs.Count)
                {
                    GoToNextCampaign();
                }
                else
                {
                    GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                }
                break;

            case Campaigns.FinalTest:
                finalTest.currentCampaignJobIndex++;
                
                if(finalTest.currentCampaignJobIndex >= finalTest.campaignJobs.Count)
                {
                    GoToNextCampaign();
                }
                else
                {
                    GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                }
                break;
            default:
                Debug.LogError("Current Campaign Jobs for " + currentCamp + " not setup.");
                return;
        }
    }

    public void AlterNarrativeVariable(MysteriousEntity.NarrativeVariables meMainOutcomes, string newText)
    {

    }

    [Serializable]
    public class CateringToTheRich
    {
        [ReadOnly] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();
        
        public enum NarrativeOutcomes { NA, SideWithScientist, KillBeckett, LetBalePilot, KilledAtSafari, KilledOnce, TellVIPsAboutClones, VIPTrust, CloneTrust}
        
        public bool ctr_sideWithScientist;
        public bool ctr_killBeckett;
        public bool ctr_letBalePilot;
        public bool ctr_killedAtSafari;
        public bool ctr_tellVIPsAboutClones;
        public bool ctr_killedOnce;
        
        public int ctr_VIPTrust = 50;
        public int ctr_cloneTrust = 50;
        
        // temp saving for resesting
        private bool saved_ctr_sideWithScientist;
        private bool saved_ctr_killBeckett;
        private bool saved_ctr_letBalePilot;
        private bool saved_ctr_killedAtSafari;
        private bool saved_ctr_tellVIPsAboutClones;
        private bool saved_ctr_killedOnce;
        
        private int saved_ctr_VIPTrust = 50;
        private int saved_ctr_cloneTrust = 50;

        public void SaveEventChoices()
        {
            saved_ctr_sideWithScientist = ctr_sideWithScientist;
            saved_ctr_killBeckett = ctr_killBeckett;
            saved_ctr_letBalePilot = ctr_letBalePilot;
            saved_ctr_killedAtSafari = ctr_killedAtSafari;
            saved_ctr_tellVIPsAboutClones = ctr_tellVIPsAboutClones;
            saved_ctr_killedOnce = ctr_killedOnce;
            saved_ctr_VIPTrust = ctr_VIPTrust;
            saved_ctr_cloneTrust = ctr_cloneTrust;
        }
        
        public void ResetEventChoicesToJobStart()
        {
            ctr_sideWithScientist = saved_ctr_sideWithScientist;
            ctr_killBeckett = saved_ctr_killBeckett;
            ctr_letBalePilot = saved_ctr_letBalePilot;
            ctr_killedAtSafari = saved_ctr_killedAtSafari;
            ctr_tellVIPsAboutClones = saved_ctr_tellVIPsAboutClones;
            ctr_killedOnce = saved_ctr_killedOnce;
            ctr_VIPTrust = saved_ctr_VIPTrust;
            ctr_cloneTrust = saved_ctr_cloneTrust;
        }
    }

    [Serializable]
    public class MysteriousEntity
    {
        [ReadOnly] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public enum NarrativeVariables
        {
            NA,
            //job 1, Event 2
            KuonInvestigates,
            Decline_Bribe,
            Decline_Fire,
            Accept,
            OpenedCargo

        }
        public bool me_kuonInvestigates;
        public bool me_openedCargo;

        public bool me_declineBribe;
        public bool me_declineFire;
        public bool me_Accept;

    }

    [Serializable]
    public class FinalTest
    {
        [ReadOnly] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public int assetCount = 0;
        public enum NarrativeVariables
        {
            NA,
            LexaDoomed,
            LanriExperiment,
            TruthTold,
            ScienceSavior,
            KellisLoyalty
        }

        public bool ft_lexaDoomed;
        public bool ft_lanriExperiment;
        public bool ft_truthTold;
        public bool ft_scienceSavior;
        public bool ft_kellisLoyalty;
    }
}
