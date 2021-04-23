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

    public Campaigns currentCamp {get; private set;} = Campaigns.CateringToTheRich;

    public CateringToTheRich cateringToTheRich = new CateringToTheRich();
    public MysteriousEntity mysteriousEntity = new MysteriousEntity();
    public FinalTest finalTest = new FinalTest();
    
    [SerializeField, Tooltip("All character Events in the game. Will be supplied to event system")]
    public List<GameObject> charEvents;
    [HideInInspector] public List<GameObject> playedEvents;


    #region Multipliers
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

    public List<GameObject> GetCharacterEvents()
    {
        return charEvents;
    }

    public void RemoveFromCharEvents(GameObject thisEvent)
    {
        charEvents.Remove(thisEvent);
        playedEvents.Add(thisEvent);

        //if(charEvents.Count == 0)
        //{
        //    ResetCharEvents();
        //}
    }

    public void ResetCharEvents()
    {
        charEvents.AddRange(playedEvents);
    }
    /// <summary>
    /// Sets currentCampaign to the specified index and resets its job index to 0
    /// </summary>
    /// <param name="direction"></param>
    public void CycleCampaignsCheat(int direction)
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

    public void CycleJobIndexCheat(int direction)
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

    /// <summary>
    /// Returns the current camp vairable for room level upgrades
    /// </summary>
    public int GetCurrentCampaignIndex()
    {
        return (int)currentCamp;
    }

    public float GetMultiplier(ResourceDataTypes resource)
    {
        float multiplier = 1;

        switch (currentCamp)
        {
            case Campaigns.CateringToTheRich:
                multiplier = 1;

                break;
            case Campaigns.MysteriousEntity:
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

                break;
            case Campaigns.FinalTest:
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

                break;
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

        GameObject.FindGameObjectWithTag("powercore").gameObject.GetComponent<RoomStats>().UpgradePower();
        
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

    public void AlterNarrativeBoolCheat(int variableIndex)
    {
        switch(currentCamp)
        {
            case Campaigns.CateringToTheRich:

                if(variableIndex < 6)
                {
                    if (cateringToTheRich.GetCtrNarrativeOutcome(variableIndex) == true)
                    {
                        cateringToTheRich.SetCtrNarrativeOutcome(variableIndex, false);
                        Debug.Log("Changed " + cateringToTheRich.GetOutcomeName(variableIndex) + " to false");
                    }
                    else
                    {
                        cateringToTheRich.SetCtrNarrativeOutcome(variableIndex, true);
                        Debug.Log("Changed " + cateringToTheRich.GetOutcomeName(variableIndex) + " to true");
                    }
                }

                break;

            case Campaigns.MysteriousEntity:
                if(variableIndex < 5)
                {
                    if (mysteriousEntity.GetMeNarrativeVariable(variableIndex) == true)
                    {
                        mysteriousEntity.SetMeNarrativeVariable(variableIndex, false);
                        Debug.Log("Changed " + mysteriousEntity.GetOutcomeName(variableIndex) + " to false");
                    }
                    else
                    {
                        mysteriousEntity.SetMeNarrativeVariable(variableIndex, true);
                        Debug.Log("Changed " + mysteriousEntity.GetOutcomeName(variableIndex) + " to true");
                    }
                }

                break;

            case Campaigns.FinalTest:
                if (finalTest.GetFtNarrativeVariable(variableIndex) == true)
                {
                    finalTest.SetFtNarrativeVariable(variableIndex, false);
                    Debug.Log("Changed " + finalTest.GetOutcomeName(variableIndex) + " to false");
                }
                else
                {
                    finalTest.SetFtNarrativeVariable(variableIndex, true);
                    Debug.Log("Changed " + finalTest.GetOutcomeName(variableIndex) + " to true");
                }
                break;
        }
    }

    /// <summary>
    /// Alters narrative variables based on the current campaign
    /// </summary>
    /// <param name="direction">Positive or negative 1</param>
    /// <param name="alt">If using side arrows, this should be true. Changes which trust variable is altered</param>
    public void AlterNarrativeNumbersCheat(int direction, bool alt)
    {
        switch(currentCamp)
        {
            case Campaigns.CateringToTheRich:
                if(!alt)
                {
                    cateringToTheRich.ctr_VIPTrust += direction * 10;
                    Debug.Log("VIP trust is now " + cateringToTheRich.ctr_VIPTrust);

                }
                else
                {
                    cateringToTheRich.ctr_cloneTrust += direction * 10;
                    Debug.Log("Clone trust is now " + cateringToTheRich.ctr_cloneTrust);
                }
                break;
            case Campaigns.FinalTest:
                finalTest.assetCount += direction;
                Debug.Log("Asset count is now " + finalTest.assetCount);
                break;
        }
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

        protected bool[] ctrNarrativeOutcomes = new bool[6];

        public int ctr_VIPTrust = 50;
        public int ctr_cloneTrust = 50;

        public string GetOutcomeName(int index)
        {
            return Enum.GetName(typeof(NarrativeOutcomes), index);
        }
        public bool GetCtrNarrativeOutcome(NarrativeOutcomes outcome)
        {
            return ctrNarrativeOutcomes[(int) outcome];
        }

        public bool GetCtrNarrativeOutcome(int index)
        {
            return ctrNarrativeOutcomes[index];
        }

        public void SetCtrNarrativeOutcome(NarrativeOutcomes outcome, bool state)
        {
            ctrNarrativeOutcomes[(int) outcome] = state;
        }
        public void SetCtrNarrativeOutcome(int index, bool state)
        {
            ctrNarrativeOutcomes[index] = state;
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

        public string GetOutcomeName(int index)
        {
            return Enum.GetName(typeof(NarrativeVariables), index);
        }

        public bool GetMeNarrativeVariable(NarrativeVariables variable)
        {
            return meNarrativeVariables[(int) variable];
        }
        public bool GetMeNarrativeVariable(int index)
        {
            return meNarrativeVariables[index];
        }

        public void SetMeNarrativeVariable(NarrativeVariables variable, bool state)
        {
            meNarrativeVariables[(int) variable] = state;
        }

        public void SetMeNarrativeVariable(int index, bool state)
        {
            meNarrativeVariables[index] = state;
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
            ResearchShared,
            DisintegrationRay,
            WarpShields,
            ExoSuits,
            RealityBomb,
            AncientHackingDevice,
            ArtifactAngry
        }

        private bool[] ftNarrativeVariables = new bool[16];

        public string GetOutcomeName(int index)
        {
            return Enum.GetName(typeof(NarrativeVariables), index);
        }

        public bool GetFtNarrativeVariable(NarrativeVariables variable)
        {
            return ftNarrativeVariables[(int) variable];
        }
        public bool GetFtNarrativeVariable(int index)
        {
            return ftNarrativeVariables[index];
        }

        public void SetFtNarrativeVariable(NarrativeVariables variable, bool state)
        {
            ftNarrativeVariables[(int) variable] = state;
        }
        public void SetFtNarrativeVariable(int index, bool state)
        {
            ftNarrativeVariables[index] = state;
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
