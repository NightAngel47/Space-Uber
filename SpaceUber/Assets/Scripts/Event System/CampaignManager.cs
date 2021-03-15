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

    private int currentCampaignNumber;
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
        switch(currentCamp)
        {
            case Campaigns.CateringToTheRich:
                return 0;
            case Campaigns.MysteriousEntity:
                return 1;
            case Campaigns.FinalTest:
                return 2;
            default:
                Debug.LogError("Current Campaign Index for " + currentCamp + " not setup.");
                return 10;
        }
    }

    /// <summary>
    /// Sets currentCampaign to the specified index and resets its job index to 0
    /// </summary>
    /// <param name="direction"></param>
    public void CycleCampaigns(int direction)
    {
        if(direction > 0) //cycle upward
        {
            switch(currentCamp)
            {
                case Campaigns.CateringToTheRich:
                    currentCamp = Campaigns.MysteriousEntity;
                    Debug.Log("The campaign is now Mysterious Entity");
                    break;
                case Campaigns.MysteriousEntity:
                    currentCamp = Campaigns.FinalTest;
                    Debug.Log("The campaign is now Final Test");
                    break;
                case Campaigns.FinalTest:
                    currentCamp = Campaigns.CateringToTheRich;
                    Debug.Log("The campaign is now catering to the rich");
                    break;

            }
        }
        else //cycle backward
        {
            switch (currentCamp)
            {
                case Campaigns.CateringToTheRich:
                    currentCamp = Campaigns.FinalTest;
                    Debug.Log("The campaign is now Final Test");
                    break;
                case Campaigns.MysteriousEntity:
                    currentCamp = Campaigns.CateringToTheRich;
                    Debug.Log("The campaign is now catering to the rich");
                    break;
                case Campaigns.FinalTest:
                    currentCamp = Campaigns.MysteriousEntity;
                    Debug.Log("The campaign is now MysteriousEntity");
                    break;

            }
        }

        
        GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
    }

    public void CycleJobIndex(int direction)
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.jobIndex+= direction;

                if ( cateringToTheRich.jobIndex > cateringToTheRich.campaignJobs.Count)
                {
                    cateringToTheRich.jobIndex = 0;
                }
                else if (cateringToTheRich.jobIndex < 0)
                {
                    print("Went below " + cateringToTheRich.campaignJobs.Count + " jobs");
                    cateringToTheRich.jobIndex = cateringToTheRich.campaignJobs.Count - 1;
                }

                Debug.Log("Now doing job " + cateringToTheRich.jobIndex + " in catering to the rich");
                break;

            case Campaigns.MysteriousEntity:
                mysteriousEntity.jobIndex+= direction;

                if (mysteriousEntity.jobIndex > mysteriousEntity.campaignJobs.Count)
                {
                    mysteriousEntity.jobIndex = 0;
                }
                else if (mysteriousEntity.jobIndex < 0)
                {
                    mysteriousEntity.jobIndex = mysteriousEntity.campaignJobs.Count - 1;
                }
                Debug.Log("Now doing job " + mysteriousEntity.jobIndex + " in mysterious entity");
                break;

            case Campaigns.FinalTest:
                finalTest.jobIndex+= direction;

                if (finalTest.jobIndex > finalTest.campaignJobs.Count)
                {
                    finalTest.jobIndex = 0;
                }
                else if (finalTest.jobIndex < 0)
                {
                    finalTest.jobIndex = finalTest.campaignJobs.Count - 1;
                }
                Debug.Log("Now doing job " + finalTest.jobIndex + " in final test");
                break;


            
        }
        
    }

    public int GetCurrentJobIndex()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                return cateringToTheRich.jobIndex;
            case Campaigns.MysteriousEntity:
                return mysteriousEntity.jobIndex;
            case Campaigns.FinalTest:
                return finalTest.jobIndex;
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

        SaveCampaignData();
    }
    public void GoToNextJob()
    {
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.jobIndex++;

                if (cateringToTheRich.jobIndex >= cateringToTheRich.campaignJobs.Count)
                {
                    GoToNextCampaign();
                }
                else
                {
                    GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                }
                break;

            case Campaigns.MysteriousEntity:
                mysteriousEntity.jobIndex++;

                if (mysteriousEntity.jobIndex >= mysteriousEntity.campaignJobs.Count)
                {
                    GoToNextCampaign();
                }
                else
                {
                    GameManager.instance.ChangeInGameState(InGameStates.JobSelect);
                }
                break;

            case Campaigns.FinalTest:
                finalTest.jobIndex++;

                if(finalTest.jobIndex >= finalTest.campaignJobs.Count)
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

        SaveCampaignData();
    }

    public void AlterNarrativeVariable(MysteriousEntity.NarrativeVariables meMainOutcomes, string newText)
    {

    }

    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            LoadCampaignData();
        }
        else
        {
            SaveCampaignData();
        }
    }

    public void SaveCampaignData()
    {
        cateringToTheRich.SaveEventChoices();
        mysteriousEntity.SaveEventChoices();
        finalTest.SaveEventChoices();

        SavingLoadingManager.instance.Save<Campaigns>("currentCamp", currentCamp);

        int currentJob;
        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                currentJob = cateringToTheRich.jobIndex;
                break;
            case Campaigns.MysteriousEntity:
                currentJob = mysteriousEntity.jobIndex;
                break;
            case Campaigns.FinalTest:
                currentJob = finalTest.jobIndex;
                break;
            default:
                Debug.LogError("Current Campaign Jobs for " + currentCamp + " not setup.");
                currentJob = 0;
                break;
        }
        SavingLoadingManager.instance.Save<int>("currentJob", currentJob);
    }

    private void LoadCampaignData()
    {
        cateringToTheRich.ResetEventChoicesToJobStart();
        mysteriousEntity.ResetEventChoicesToJobStart();
        finalTest.ResetEventChoicesToJobStart();

        currentCamp = SavingLoadingManager.instance.Load<Campaigns>("currentCamp");

        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.jobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(cateringToTheRich.campaignJobs[cateringToTheRich.jobIndex]);
                break;
            case Campaigns.MysteriousEntity:
                mysteriousEntity.jobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(mysteriousEntity.campaignJobs[mysteriousEntity.jobIndex]);
                break;
            case Campaigns.FinalTest:
                finalTest.jobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(finalTest.campaignJobs[finalTest.jobIndex]);
                break;
            default:
                Debug.LogError("Current Campaign Jobs for " + currentCamp + " not setup.");
                break;
        }

    }

    [Serializable]
    public class CateringToTheRich
    {
        [ReadOnly] public int jobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public enum NarrativeOutcomes { NA = -1, SideWithScientist, KillBeckett, LetBalePilot, KilledAtSafari, KilledOnce, TellVIPsAboutClones, VIPTrust, CloneTrust}

        private bool[] ctrNarrativeOutcomes = new bool[6];

        public int ctr_VIPTrust = 50;
        public int ctr_cloneTrust = 50;

        public bool GetCtrNarrativeOutcome(NarrativeOutcomes outcome)
        {
            return ctrNarrativeOutcomes[(int) outcome];
        }

        public void SetCtrNarrativeOutcome(NarrativeOutcomes outcome, bool state)
        {
            ctrNarrativeOutcomes[(int) outcome] = state;
        }

        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool[]>("ctrNarrativeOutcomes", ctrNarrativeOutcomes);
            SavingLoadingManager.instance.Save<int>("ctr_VIPTrust", ctr_VIPTrust);
            SavingLoadingManager.instance.Save<int>("ctr_cloneTrust", ctr_cloneTrust);
        }

        public void ResetEventChoicesToJobStart()
        {
            ctrNarrativeOutcomes = SavingLoadingManager.instance.Load<bool[]>("ctrNarrativeOutcomes");
            ctr_VIPTrust = SavingLoadingManager.instance.Load<int>("ctr_VIPTrust");
            ctr_cloneTrust = SavingLoadingManager.instance.Load<int>("ctr_cloneTrust");
        }
    }

    [Serializable]
    public class MysteriousEntity
    {
        [ReadOnly] public int jobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public enum NarrativeVariables
        {
            NA = -1,
            //job 1, Event 2
            KuonInvestigates,
            Decline_Bribe,
            Decline_Fire,
            Accept,
            OpenedCargo
        }

        private bool[] meNarrativeVariables = new bool[5];

        public bool GetMeNarrativeVariable(NarrativeVariables variable)
        {
            return meNarrativeVariables[(int) variable];
        }

        public void SetMeNarrativeVariable(NarrativeVariables variable, bool state)
        {
            meNarrativeVariables[(int) variable] = state;
        }

        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool[]>("narrativeVariables", meNarrativeVariables);
        }

        public void ResetEventChoicesToJobStart()
        {
            meNarrativeVariables = SavingLoadingManager.instance.Load<bool[]>("narrativeVariables");
        }
    }

    [Serializable]
    public class FinalTest
    {
        [ReadOnly] public int jobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();

        public int assetCount = 0;
        public enum NarrativeVariables
        {
            NA = -1,
            LexaDoomed,
            LanriExperiment,
            TruthTold,
            ScienceSavior,
            KellisLoyalty,
            LexaPlan,
            MateoPlan,
            LanriRipleyPlan,
            KuonPlan,
            ResearchShared
        }

        private bool[] ftNarrativeVariables = new bool[10];

        public bool GetFtNarrativeVariable(NarrativeVariables variable)
        {
            return ftNarrativeVariables[(int) variable];
        }

        public void SetFtNarrativeVariable(NarrativeVariables variable, bool state)
        {
            ftNarrativeVariables[(int) variable] = state;
        }

        public void SaveEventChoices()
        {
            SavingLoadingManager.instance.Save<bool[]>("ftNarrativeVariables", ftNarrativeVariables);
        }

        public void ResetEventChoicesToJobStart()
        {
            ftNarrativeVariables = SavingLoadingManager.instance.Load<bool[]>("ftNarrativeVariables");
        }
    }
}
