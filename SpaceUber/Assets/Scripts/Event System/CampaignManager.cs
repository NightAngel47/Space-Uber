/*
 * CampaignManager.cs
 * Author(s): Scott Acker
 * Created on: 11/3/2020 (en-US)
 * Description: Keeps information about which campaign a player is currently on.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public CateringToTheRich cateringToTheRich = new CateringToTheRich();
    
    [Serializable]
    public class CateringToTheRich
    {
        [HideInInspector] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();
        
        public enum NarrativeOutcomes { NA, SideWithScientist, KillBeckett, LetBalePilot, KilledAtSafari, KilledOnce, TellVIPsAboutClones}
        
        public bool ctr_sideWithScientist;
        public bool ctr_killBeckett;
        public bool ctr_letBalePilot;
        public bool ctr_killedAtSafari;
        public bool ctr_tellVIPsAboutClones;

        public bool ctr_killedOnce;
        public int ctr_VIPTrust = 50;
        public int ctr_cloneTrust = 50;
    }

}
