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

    public Campaigns currentCamp = Campaigns.CateringToTheRich;

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
        List<Job> available = new List<Job>();
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                available = cateringToTheRich.campaignJobs;
                break;
            case Campaigns.MysteriousEntity:
                available = mysteriousEntity.campaignJobs;
                break;
            case Campaigns.FinalTest:
                available = finalTest.campaignJobs;
                break;
        }

        return available;
    }

    public int GetCurrentCampaignIndex()
    {
        int index = 0;
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                index = cateringToTheRich.currentCampaignJobIndex;
                break;
            case Campaigns.MysteriousEntity:
                index = mysteriousEntity.currentCampaignJobIndex;
                break;
            case Campaigns.FinalTest:
                index = finalTest.currentCampaignJobIndex;
                break;
        }
        return index;
    }

    public void GoToNextCampaign()
    {
        switch(currentCamp)
        {
            case Campaigns.CateringToTheRich:
                currentCamp = Campaigns.MysteriousEntity;
                break;
            case Campaigns.MysteriousEntity:
                currentCamp = Campaigns.FinalTest;
                break;
            case Campaigns.FinalTest:
                //do ending stuff
                break;
        }
    }
    public void GoToNextJob()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.currentCampaignJobIndex++;

                if (finalTest.currentCampaignJobIndex > 3)
                {
                    GoToNextCampaign();
                }
                break;

            case Campaigns.MysteriousEntity:
                mysteriousEntity.currentCampaignJobIndex++;
                
                if (finalTest.currentCampaignJobIndex > 3)
                {
                    GoToNextCampaign();
                }
                break;

            case Campaigns.FinalTest:
                finalTest.currentCampaignJobIndex++;
                
                if(finalTest.currentCampaignJobIndex > 3)
                {
                    //ENd game
                }
                break;
        }
    }

    [Serializable]
    public class CateringToTheRich
    {
        [HideInInspector] public int currentCampaignJobIndex = 0;
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
        [HideInInspector] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public enum NarrativeOutcomes
        {
            NA,
            //job 1, Event 2
            KuonInvestigates,
            //No listed titles for j2E3
            OpenedCargo

        }

        public enum J2E3Outcomes
        {
            Decline_Bribe,
            Decline_Fire,
            Accept
        }

        public bool me_kuonInvestigates;

        public bool me_declineOffer;
        public bool me_acceptOffer;
        public bool me_bribeLoudon;
        public bool me_blackmailLoudon;
        public bool me_fireLOudon;
        public bool me_blackmailEquinox;
        public bool me_keepLoudon;

    }

    [Serializable]
    public class FinalTest
    {
        [HideInInspector] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public int assetCount = 0;
        public enum NarrativeVariables
        {
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
