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
        
        public void ResetEventChoices()
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

}
