/*
 * CampaignManager.cs
 * Author(s): Scott Acker
 * Created on: 11/3/2020 (en-US)
 * Description: Keeps information about which campaign a player is currently on.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public enum Campaign
    {
        N_A,
        CateringToTheRich
    }
    public Campaign currentCamp;

    
    #region Catering to The Rich Variables
    public bool ctr_sideWithScientist;
    public bool ctr_killBeckett;
    public bool ctr_killedAtSafari;
    public bool ctr_tellVIPsAboutClones;

    public int ctr_VIPTrust = 50;
    public int ctr_cloneTrust = 50;

    
    #endregion

}
