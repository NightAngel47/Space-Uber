/*
 * CampaignManager.cs
 * Author(s): Scott Acker
 * Created on: 11/3/2020 (en-US)
 * Description: Keeps information about which campaign a player is currently on.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Campaigns
{
    N_A = -1,
    CateringToTheRich = 0
}

public class CampaignManager : MonoBehaviour
{
    public Campaigns currentCamp;

    public List<Campaign> campaigns = new List<Campaign>();
    
    [Serializable]
    public class Campaign
    {
        [HideInInspector] public int currentCampaignJobIndex = 0;
        public List<Job> campaignJobs = new List<Job>();
    }
    
    public class CateringToTheRich : Campaign
    {
        public bool ctr_sideWithScientist;
        public bool ctr_killBeckett;
        public bool ctr_letBalePilot;
        public bool ctr_killedAtSafari;
        public bool ctr_tellVIPsAboutClones;

        public int ctr_VIPTrust = 50;
        public int ctr_cloneTrust = 50;
    }

}
