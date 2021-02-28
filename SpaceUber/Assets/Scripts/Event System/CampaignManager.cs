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
        
        SaveData();
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
        
        SaveData();
    }

    public void AlterNarrativeVariable(MysteriousEntity.NarrativeVariables meMainOutcomes, string newText)
    {

    }
    
    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetData();
        }
        else
        {
            SaveData();
        }
    }
    
    private void SaveData()
    {
        cateringToTheRich.SaveEventChoices();
        mysteriousEntity.SaveEventChoices();
        finalTest.SaveEventChoices();
        
        SavingLoadingManager.instance.Save<Campaigns>("currentCamp", currentCamp);
        
        int currentJob;
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                currentJob = cateringToTheRich.currentCampaignJobIndex;
                break;
            case Campaigns.MysteriousEntity:
                currentJob = mysteriousEntity.currentCampaignJobIndex;
                break;
            case Campaigns.FinalTest:
                currentJob = finalTest.currentCampaignJobIndex;
                break;
            default:
                Debug.LogError("Current Campaign Jobs for " + currentCamp + " not setup.");
                currentJob = 0;
                break;
        }
        SavingLoadingManager.instance.Save<int>("currentJob", currentJob);
    }
    
    private void ResetData()
    {
        cateringToTheRich.ResetEventChoicesToJobStart();
        mysteriousEntity.ResetEventChoicesToJobStart();
        finalTest.ResetEventChoicesToJobStart();
        
        currentCamp = SavingLoadingManager.instance.Load<Campaigns>("currentCamp");
        
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");;
                break;
            case Campaigns.MysteriousEntity:
                mysteriousEntity.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");;
                break;
            case Campaigns.FinalTest:
                finalTest.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");;
                break;
            default:
                Debug.LogError("Current Campaign Jobs for " + currentCamp + " not setup.");
                break;
        }
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

        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool>("ctr_sideWithScientist", ctr_sideWithScientist);
            SavingLoadingManager.instance.Save<bool>("ctr_killBeckett", ctr_killBeckett);
            SavingLoadingManager.instance.Save<bool>("ctr_letBalePilot", ctr_letBalePilot);
            SavingLoadingManager.instance.Save<bool>("ctr_killedAtSafari", ctr_killedAtSafari);
            SavingLoadingManager.instance.Save<bool>("ctr_tellVIPsAboutClones", ctr_tellVIPsAboutClones);
            SavingLoadingManager.instance.Save<bool>("ctr_killedOnce", ctr_killedOnce);
            SavingLoadingManager.instance.Save<int>("ctr_VIPTrust", ctr_VIPTrust);
            SavingLoadingManager.instance.Save<int>("ctr_cloneTrust", ctr_cloneTrust);
        }
        
        public void ResetEventChoicesToJobStart()
        {
            ctr_sideWithScientist = SavingLoadingManager.instance.Load<bool>("ctr_sideWithScientist");
            ctr_killBeckett = SavingLoadingManager.instance.Load<bool>("ctr_killBeckett");
            ctr_letBalePilot = SavingLoadingManager.instance.Load<bool>("ctr_letBalePilot");
            ctr_killedAtSafari = SavingLoadingManager.instance.Load<bool>("ctr_killedAtSafari");
            ctr_tellVIPsAboutClones = SavingLoadingManager.instance.Load<bool>("ctr_tellVIPsAboutClones");
            ctr_killedOnce = SavingLoadingManager.instance.Load<bool>("ctr_killedOnce");
            ctr_VIPTrust = SavingLoadingManager.instance.Load<int>("ctr_VIPTrust");
            ctr_cloneTrust = SavingLoadingManager.instance.Load<int>("ctr_cloneTrust");
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
        
        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool>("me_kuonInvestigates", me_kuonInvestigates);
            SavingLoadingManager.instance.Save<bool>("me_openedCargo", me_openedCargo);
            SavingLoadingManager.instance.Save<bool>("me_declineBribe", me_declineBribe);
            SavingLoadingManager.instance.Save<bool>("me_declineFire", me_declineFire);
            SavingLoadingManager.instance.Save<bool>("me_Accept", me_Accept);
        }
        
        public void ResetEventChoicesToJobStart()
        {
            me_kuonInvestigates = SavingLoadingManager.instance.Load<bool>("me_kuonInvestigates");
            me_openedCargo = SavingLoadingManager.instance.Load<bool>("me_openedCargo");
            me_declineBribe = SavingLoadingManager.instance.Load<bool>("me_declineBribe");
            me_declineFire = SavingLoadingManager.instance.Load<bool>("me_declineFire");
            me_Accept = SavingLoadingManager.instance.Load<bool>("me_Accept");
        }
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
            KellisLoyalty,
            EndgamePlan
        }

        public bool ft_lexaDoomed;
        public bool ft_lanriExperiment;
        public bool ft_truthTold;
        public bool ft_scienceSavior;
        public bool ft_kellisLoyalty;
        public bool ft_endgamePlan;
        
        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool>("ft_lexaDoomed", ft_lexaDoomed);
            SavingLoadingManager.instance.Save<bool>("ft_lanriExperiment", ft_lanriExperiment);
            SavingLoadingManager.instance.Save<bool>("ft_truthTold", ft_truthTold);
            SavingLoadingManager.instance.Save<bool>("ft_scienceSavior", ft_scienceSavior);
            SavingLoadingManager.instance.Save<bool>("ft_kellisLoyalty", ft_kellisLoyalty);
            SavingLoadingManager.instance.Save<bool>("ft_endgamePlan", ft_endgamePlan);
        }
        
        public void ResetEventChoicesToJobStart()
        {
            ft_lexaDoomed = SavingLoadingManager.instance.Load<bool>("ft_lexaDoomed");
            ft_lanriExperiment = SavingLoadingManager.instance.Load<bool>("ft_lanriExperiment");
            ft_truthTold = SavingLoadingManager.instance.Load<bool>("ft_truthTold");
            ft_scienceSavior = SavingLoadingManager.instance.Load<bool>("ft_scienceSavior");
            ft_kellisLoyalty = SavingLoadingManager.instance.Load<bool>("ft_kellisLoyalty");
            ft_endgamePlan = SavingLoadingManager.instance.Load<bool>("ft_endgamePlan");
        }
    }
}
