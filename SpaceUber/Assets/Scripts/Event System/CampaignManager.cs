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

    #region
    [Header("Campaign 2 multipliers")]

    [Tooltip("How credits should be multiplied for second campaign"), SerializeField] private float creditsMult2 = 1;
    [Tooltip("How security should be multiplied for second campaign"), SerializeField] private float securityMult2 = 1;
    [Tooltip("How weapons should be multiplied for second campaign"), SerializeField] private float weaponsMult2 = 1;
    [Tooltip("How food should be multiplied for second campaign"), SerializeField] private float foodMult2 = 1;
    [Tooltip("How foodPerTick should be multiplied for second campaign"), SerializeField] private float foodPerTickMult2 = 1;
    [Tooltip("How crew should be multiplied for second campaign"), SerializeField] private float crewMult2 = 1;
    [Tooltip("How energy should be multiplied for second campaign"), SerializeField] private float energyMult2 = 1;
    [Tooltip("How hull should be multiplied for second campaign"), SerializeField] private float hullMult2 = 1;

    [Header("Campaign 3 multipliers")]
    [Tooltip("How credits should be multiplied for third campaign"), SerializeField] private float creditsMult3 = 1;
    [Tooltip("How security should be multiplied for third campaign"), SerializeField] private float securityMult3 = 1;
    [Tooltip("How weapons should be multiplied for third campaign"), SerializeField] private float weaponsMult3 = 1;
    [Tooltip("How food should be multiplied for third campaign"), SerializeField] private float foodMult3 = 1;
    [Tooltip("How food per tick should be multiplied for third campaign"), SerializeField] private float foodPerTickMult3 = 1;
    [Tooltip("How crew should be multiplied for third campaign"), SerializeField] private float crewMult3 = 1;
    [Tooltip("How energy should be multiplied for third campaign"), SerializeField] private float energyMult3 = 1;
    [Tooltip("How hull should be multiplied for third campaign"), SerializeField] private float hullMult3 = 1;

    #endregion

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

    public float GetMultiplier(ResourceDataTypes resource)
    {
        float multiplier = 1;

        if (GetCurrentCampaignIndex() == 1)
        {
            multiplier = 1;
        }
        else if (GetCurrentCampaignIndex() == 2)
        {
            switch (resource)
            {
                case ResourceDataTypes._Credits:
                    multiplier = creditsMult2;
                    break;
                case ResourceDataTypes._Crew:
                    multiplier = crewMult2;
                    break;
                case ResourceDataTypes._Energy:
                    multiplier = energyMult2;
                    break;
                case ResourceDataTypes._Food:
                    multiplier = foodMult2;
                    break;
                case ResourceDataTypes._FoodPerTick:
                    multiplier = foodPerTickMult2;
                    break;
                case ResourceDataTypes._HullDurability:
                    multiplier = hullMult2;
                    break;
                case ResourceDataTypes._Security:
                    multiplier = securityMult2;
                    break;
                case ResourceDataTypes._ShipWeapons:
                    multiplier = weaponsMult2;
                    break;
            }
        }
        else if (GetCurrentCampaignIndex() == 3)
        {
            switch (resource)
            {
                case ResourceDataTypes._Credits:
                    multiplier = creditsMult3;
                    break;
                case ResourceDataTypes._Crew:
                    multiplier = crewMult3;
                    break;
                case ResourceDataTypes._Energy:
                    multiplier = energyMult3;
                    break;
                case ResourceDataTypes._Food:
                    multiplier = foodMult3;
                    break;
                case ResourceDataTypes._FoodPerTick:
                    multiplier = foodPerTickMult3;
                    break;
                case ResourceDataTypes._HullDurability:
                    multiplier = hullMult3;
                    break;
                case ResourceDataTypes._Security:
                    multiplier = securityMult3;
                    break;
                case ResourceDataTypes._ShipWeapons:
                    multiplier = weaponsMult3;
                    break;
            }
        }

        return multiplier;
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

    private void LoadCampaignData()
    {
        cateringToTheRich.ResetEventChoicesToJobStart();
        mysteriousEntity.ResetEventChoicesToJobStart();
        finalTest.ResetEventChoicesToJobStart();

        currentCamp = SavingLoadingManager.instance.Load<Campaigns>("currentCamp");

        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                cateringToTheRich.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(cateringToTheRich.campaignJobs[cateringToTheRich.currentCampaignJobIndex]);
                break;
            case Campaigns.MysteriousEntity:
                mysteriousEntity.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(mysteriousEntity.campaignJobs[mysteriousEntity.currentCampaignJobIndex]);
                break;
            case Campaigns.FinalTest:
                finalTest.currentCampaignJobIndex = SavingLoadingManager.instance.Load<int>("currentJob");
                EventSystem.instance.TakeStoryJobEvents(finalTest.campaignJobs[finalTest.currentCampaignJobIndex]);
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
        [ReadOnly] public int currentCampaignJobIndex = 0;
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
        [ReadOnly] public int currentCampaignJobIndex = 0;
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
