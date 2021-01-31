/*
 * ShipStats.cs
 * Author(s): Grant Frey
 * Created on: 9/14/2020 (en-US)
 * Description:
 */

using System;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;

public class ShipStats : MonoBehaviour
{
    public enum Resources{ Credits, Energy, Security, ShipWeapons, Crew, Food, FoodPerTick, HullDurability, Stock}

    [SerializeField ,Tooltip("Starting amount of credits"), Foldout("Starting Ship Stats")]
    private int startingCredits;
    [SerializeField, Tooltip("Starting amount of energy"), Foldout("Starting Ship Stats")]
    private int startingEnergy;
    [SerializeField, Tooltip("Starting value of security"), Foldout("Starting Ship Stats")]
    private int startingSecurity;
    [SerializeField, Tooltip("Starting value of ship weapons"), Foldout("Starting Ship Stats")]
    private int startingShipWeapons;
    [SerializeField, Tooltip("Starting amount of crew"), Foldout("Starting Ship Stats")]
    private int startingCrew;
    [SerializeField, Tooltip("Starting amount of food"), Foldout("Starting Ship Stats")]
    private int startingFood;
    [SerializeField, Tooltip("Starting amount of ship health"), Foldout("Starting Ship Stats")]
    private int startingShipHealth;
    [SerializeField, Tooltip("Starting amount of crewMorale"), Foldout("Starting Ship Stats")]
    private int startingMorale;

    #region Character Approval
    public enum Characters
    {
        KUON,
        LANRI,
        LEXA,
        MATEO,
        RIPLEY
    }

    private int kuonApproval;
    private int lanriApproval;
    private int lexaApproval;
    private int mateoApproval;
    private int ripleyApproval;

    private int approvalMax = 10;
    private int approvalMin = -10;
    #endregion

    public GameObject cantPlaceText;
    public Sprite[] statIcons;

    private List<RoomStats> rooms;

    public GameObject roomBeingPlaced;

    private int credits;
    private int payout;
    private int crewPaymentDefault = 5;
    private int crewPaymentMoraleMultiplier = 10;
    private int energyMax;
    private int energyRemaining;
    private int security;
    private int shipWeapons;
    private int crewCapacity;
    private int crewCurrent;
    private int crewUnassigned;
    private int food;
    private int foodPerTick;
    private int foodMoraleDamageMultiplier = 2;
    private int shipHealthMax;
    private int shipHealthCurrent;
    //private int crewMorale;

    /// <summary>
    /// Reference to the ship stats UI class.
    /// </summary>
    private ShipStatsUI shipStatsUI;

    //tick variables
    private int secondsPerTick = 5;
    private bool ticksPaused;
    private bool tickStop = true;

    public int daysSince;
    [SerializeField] private TMP_Text daysSinceDisplay;

    //mutiny calculations
    private int maxMutinyMorale = 60;
    private float zeroMoraleMutinyChance = 0.75f;

    //stats at the start of the job
    private int startCredits;
    private int startPayout;
    private int startEnergyMax;
    private int startEnergyRemaining;
    private int startSecurity;
    private int startShipWeapons;
    private int startCrewCapacity;
    private int startCrewCurrent;
    private int startCrewUnassigned;
    private int startFood;
    private int startFoodPerTick;
    private int startShipHealthMax;
    private int startShipHealthCurrent;
    //private int startCrewMorale;

    private void Awake()
    {
        shipStatsUI = GetComponent<ShipStatsUI>();
    }

    private void Start()
    {
        UpdateCreditsAmount(startingCredits);
        UpdatePayoutAmount(0);
        UpdateEnergyAmount(startingEnergy, startingEnergy);
        UpdateSecurityAmount(startingSecurity);
        UpdateShipWeaponsAmount(startingShipWeapons);
        UpdateCrewAmount(startingCrew, startingCrew, startingCrew);
        UpdateFoodAmount(startingFood);
        UpdateHullDurabilityAmount(startingShipHealth, startingShipHealth);
        //UpdateCrewMorale(startingMorale);
    }
    public void PauseTickEvents()
    {
        ticksPaused = true;
    }

    public void UnpauseTickEvents()
    {
        ticksPaused = false;
    }

    public void StopTickEvents()
    {
        tickStop = true;
    }

    public void StartTickEvents()
    {
        if (tickStop)
        {
            tickStop = false;
            ticksPaused = false;
            StartCoroutine(TickUpdate());
        }
    }

    public void ResetDaysSince()
    {
        daysSince = 0;
        daysSinceDisplay.text = daysSince.ToString();
    }

    private IEnumerator<YieldInstruction> CheckDeathOnUnpause()
    {
        while (ticksPaused || tickStop)
        {
            yield return new WaitForFixedUpdate();
        }

        if (shipHealthCurrent <= 0)
        {
            GameManager.instance.ChangeInGameState(InGameStates.Death);

        }
    }

    public void SetObjectBeingPlaced()
    {
        shipStatsUI.roomBeingPlaced = roomBeingPlaced;
    }

    private IEnumerator<YieldInstruction> TickUpdate()
    {
        while(!tickStop)
        {
            while(ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(secondsPerTick);

            while(ticksPaused)
            {
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitWhile(() => ticksPaused);

            food += foodPerTick - crewCurrent;
            if(food < 0)
            {
                //crewMorale += (food * foodMoraleDamageMultiplier);
                food = 0;
            }
            shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);

            // increment days since events
            daysSince++;
            daysSinceDisplay.text = daysSince.ToString();

            //if(crewMorale < 0)
            //{
            //    crewMorale = 0;
            //}

            //UpdateMoraleShipStatsUI();

            //float mutinyChance = (maxMutinyMorale - crewMorale) * zeroMoraleMutinyChance / maxMutinyMorale;
            //if(mutinyChance > UnityEngine.Random.value)
            //{
            //    GameManager.instance.ChangeInGameState(InGameStates.Mutiny);
            //}

            if(shipHealthCurrent <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
                AudioManager.instance.PlaySFX("Hull Death");
                AudioManager.instance.PlayMusicWithTransition("Death Theme");
            }
        }
    }


    #region Update Stat Functions
    public void UpdateCreditsAmount(int creditAddition)
    {
        SetObjectBeingPlaced();
        credits += creditAddition;
        /*
        if (creditAddition >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Credits");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Credits");
        }
        */
        if(credits <= 0)
        {
            credits = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(creditAddition);
    }

    public void UpdateEnergyAmount(int energyRemainingChange, int energyMaxChange = 0)
    {
        energyMax += energyMaxChange;
        energyRemaining += energyRemainingChange;
        /*
        if (energyRemainingAddition >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Energy");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Energy");
        }
        */
        Mathf.Clamp(energyRemaining, 0, energyMax);

        shipStatsUI.UpdateEnergyUI(energyRemaining, energyMax);
        shipStatsUI.ShowEnergyUIChange(energyRemainingChange, energyMaxChange);
    }

    public void UpdateSecurityAmount(int securityAmount)
    {
        security += securityAmount;
        /*
        if (securityAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Security");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Security");
        }
        */
        if (security <= 0)
        {
            security = 0;
        }

        shipStatsUI.UpdateSecurityUI(security);
        shipStatsUI.ShowSecurityUIChange(securityAmount);
    }

    public void UpdateShipWeaponsAmount(int shipWeaponsAmount)
    {
        shipWeapons += shipWeaponsAmount;
        /*
        if (shipWeaponsAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Weapons");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Weapons");
        }
        */
        if (shipWeapons <= 0)
        {
            shipWeapons = 0;
        }

        shipStatsUI.UpdateShipWeaponsUI(shipWeapons);
        shipStatsUI.ShowShipWeaponsUIChange(shipWeaponsAmount);
    }

    /// <summary>
    /// Updates the amount of crew there are: total and unassigned.
    /// </summary>
    /// <param name="unassignedChange">How much the unassigned number of crew will change</param>
    /// <param name="crewCurrentChange">How much the current number of crew will change by</param>
    /// <param name="crewCapacityChange">How much the crew capacity will change</param>
    public void UpdateCrewAmount(int unassignedChange, int crewCurrentChange = 0, int crewCapacityChange = 0)
    {
        if(GameManager.instance.currentGameState == InGameStates.CrewManagement)
        {
            SetObjectBeingPlaced();
        }

        crewCapacity += crewCapacityChange;
        crewCurrent += crewCurrentChange;
        crewUnassigned += unassignedChange;

        if (crewCurrentChange < 0)
        {
            if(crewUnassigned < 0)
            {
                RemoveRandomCrew(Mathf.Abs(crewUnassigned));
            }  
        }

        /*
        if (crewRemainingAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Crew");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Crew");
        }
        */

        Mathf.Clamp(crewCurrent, 0, crewCapacity);
        Mathf.Clamp(crewUnassigned, 0, crewCurrent);

        shipStatsUI.UpdateCrewUI(crewUnassigned, crewCurrent, crewCapacity);
        shipStatsUI.ShowCrewUIChange(unassignedChange, crewCurrentChange, crewCapacityChange);
    }

    public void UpdateFoodAmount(int foodAmount)
    {
        food += foodAmount;
        /*
        if (foodAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Food");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Food");
        }
        */
        if (food <= 0)
        {
            food = 0;
        }

        shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
        shipStatsUI.ShowFoodUIChange(foodAmount, 0);
    }

    public void UpdateFoodPerTickAmount(int foodPerTickAmount)
    {
        foodPerTick += foodPerTickAmount;

        shipStatsUI.UpdateFoodUI(food, foodPerTick, crewCurrent);
        shipStatsUI.ShowFoodUIChange(0, foodPerTickAmount);
    }

    public void UpdateHullDurabilityAmount(int hullDurabilityRemainingChange, int hullDurabilityMaxChange = 0, bool checkImmediately = true)
    {
        shipHealthMax += hullDurabilityMaxChange;
        shipHealthCurrent += hullDurabilityRemainingChange;

        Mathf.Clamp(shipHealthCurrent, -1, shipHealthMax);

        /*
        if (hullDurabilityRemainingAmount >= 0)
        {
            AudioManager.instance.PlaySFX("Gain Hull");
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose Hull");
        }
        */

        shipStatsUI.UpdateHullUI(shipHealthCurrent, shipHealthMax);
        shipStatsUI.ShowHullUIChange(hullDurabilityRemainingChange, hullDurabilityMaxChange);

        if(checkImmediately)
        {
            if(shipHealthCurrent <= 0)
            {
                GameManager.instance.ChangeInGameState(InGameStates.Death);
            }
        }
        else
        {
            CheckDeathOnUnpause();
        }
    }

    /// <summary>
    /// Adds 'approvalChange' to the approval rating of the selected character, but will never go beyond
    /// the set minimum and maximum values
    /// </summary>
    /// <param name="thisCharacter">The character whose approval rating will be affected</param>
    /// <param name="approvalChange">The amount by which approval will chance</param>
    public void UpdateCrewMemberApproval(Characters thisCharacter, int approvalChange)
    {
        switch (thisCharacter)
        {
            case Characters.KUON:
                kuonApproval += approvalChange;
                Mathf.Clamp(kuonApproval, approvalMin, approvalMax);
                break;
            case Characters.MATEO:
                mateoApproval += approvalChange;
                Mathf.Clamp(mateoApproval, approvalMin, approvalMax);
                break;
            case Characters.LANRI:
                lanriApproval += approvalChange;
                Mathf.Clamp(lanriApproval, approvalMin, approvalMax);
                break;
            case Characters.LEXA:
                lexaApproval += approvalChange;
                Mathf.Clamp(lexaApproval, approvalMin, approvalMax);
                break;
            case Characters.RIPLEY:
                ripleyApproval += approvalChange;
                Mathf.Clamp(ripleyApproval, approvalMin, approvalMax);
                break;
        }
    }

    public int GetCrewMemberApproval(Characters thisCharacter)
    {
        switch (thisCharacter)
        {
            case Characters.KUON:
                return kuonApproval;
            case Characters.MATEO:
                return mateoApproval;
            case Characters.LANRI:
                return lanriApproval;
            case Characters.LEXA:
                return lexaApproval;
            case Characters.RIPLEY:
                return ripleyApproval;
            default:
                Debug.Log("The character whose approval you wanted does not exist");
                return 0;
        }
    }

    public void UpdatePayoutAmount(int ammount)
    {
        int initialPayout = payout;
        payout += ammount;
        if (payout <= 0)
        {
            payout = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(0, payout - initialPayout);
    }

    public void MultiplyPayout(int multiplier)
    {
        int initialPayout = payout;
        payout *= multiplier;
        if (payout <= 0)
        {
            payout = 0;
        }

        shipStatsUI.UpdateCreditsUI(credits, payout);
        shipStatsUI.ShowCreditsUIChange(0, payout - initialPayout);
    }

    public void CashPayout()
    {
        UpdateCreditsAmount(payout);
        payout = 0;
    }
    #endregion

    public bool HasEnoughPower(int power)
    {
        return EnergyRemaining >= power;
    }

    public int Credits
    {
        get { return credits; }
        set { credits = value; }
    }
    public int Payout
    {
        get { return payout; }
        set { payout = value; }
    }
    public int EnergyRemaining
    {
        get { return energyRemaining; }
        set { energyRemaining = value; }
    }
    public int Security
    {
        get { return security; }
        set { security = value; }
    }
    public int ShipWeapons
    {
        get { return shipWeapons; }
        set { shipWeapons = value; }
    }
    public int CrewUnassigned
    {
        get { return crewUnassigned; }
        set { crewUnassigned = value; }
    }
    public int CrewCurrent
    {
        get { return crewCurrent; }
        set { crewCurrent = value; }
    }
    public int Food
    {
        get { return food; }
        set { food = value; }
    }
    public int ShipHealthCurrent
    {
        get { return shipHealthCurrent; }
        set { shipHealthCurrent = value; }
    }

    //public void UpdateCrewMorale(int crewMoraleAmount)
    //{
    //    crewMorale += crewMoraleAmount;
    //
    //    if (crewMoraleAmount >= 0)
    //    {
    //       AudioManager.instance.PlaySFX("Gain Morale");
    //    }
    //    else
    //    {
    //         AudioManager.instance.PlaySFX("Lose Morale");
    //    }
    //
    //    if(crewMorale < 0)
    //    {
    //        crewMorale = 0;
    //    }
    //    // TODO update to work with changes from development
    //    //UpdateShipStatsUI();
    //}

    public void PrintShipStats()
    {
        Debug.Log("Credits " + Credits);
        Debug.Log("Energy " + EnergyRemaining);
        Debug.Log("Security " + Security);
        Debug.Log("ShipWeapons " + ShipWeapons);
        Debug.Log("CrewUnassigned " + CrewUnassigned);
        Debug.Log("CrewCurrent " + CrewCurrent);
        Debug.Log("Food " + Food);
        Debug.Log("ShipHealthCurrent " + ShipHealthCurrent);
        Debug.Log("Payout " + Payout);
    }

    public void PayCrew(int amount)
    {
        UpdateCreditsAmount(-amount * crewCurrent);
        //int BadMoraleMultiplier = (maxMutinyMorale - crewMorale) * crewPaymentMoraleMultiplier / maxMutinyMorale;
        //UpdateCrewMorale(BadMoraleMultiplier * (ammount - crewPaymentDefault));
    }

    public void RemoveRandomCrew(int amount)
    {
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();
        int[] crewLost = new int[rooms.Length];
        int crewAssigned = crewCurrent - crewUnassigned;

        for(int i = 0; i < amount; i++)
        {
            int selection = Mathf.FloorToInt(UnityEngine.Random.value * crewAssigned);
            if(selection == crewAssigned) // because Unity's Random.value includes 1, we have to do this
            {
                selection -= 1;
            }

            int index = 0;
            int crewChecked = 0;
            while(crewChecked <= selection)
            {
                crewChecked += rooms[index].currentCrew;
                
                if(crewChecked > selection)
                {
                    rooms[index].UpdateCurrentCrew(-1);
                    crewLost[index] += 1;
                    crewAssigned -= 1;
                }
                
                index += 1;
            }
        }
        
        for(int i = 0; i < crewLost.Length; i++)
        {
            if(crewLost[i] != 0)
            {
                rooms[i].SpawnStatChangeText(crewLost[i], 4);
            }
        }
    }

    public void SaveStats()
    {
        startCredits = credits;
        startPayout = payout;
        startEnergyMax = energyMax;
        startEnergyRemaining = energyRemaining;
        startSecurity = security;
        startShipWeapons = shipWeapons;
        startCrewCapacity = crewCapacity;
        startCrewCurrent = crewCurrent;
        startCrewUnassigned = crewUnassigned;
        startFood = food;
        startFoodPerTick = foodPerTick;
        startShipHealthMax = shipHealthMax;
        startShipHealthCurrent = shipHealthCurrent;
        //startCrewMorale = crewMorale;
    }

    public void ResetStats()
    {
        credits = 0;
        payout = 0;
        energyRemaining = 0;
        energyMax = 0;
        security = 0;
        shipWeapons = 0;
        crewUnassigned = 0;
        crewCurrent = 0;
        crewCapacity = 0;
        food = 0;
        foodPerTick = 0;
        shipHealthCurrent = 0;
        shipHealthMax = 0;
        //crewMorale = 0;
        
        UpdateCreditsAmount(startCredits);
        UpdatePayoutAmount(startPayout);
        UpdateEnergyAmount(startEnergyRemaining, startEnergyMax);
        UpdateSecurityAmount(startSecurity);
        UpdateShipWeaponsAmount(startShipWeapons);
        UpdateCrewAmount(startCrewUnassigned, startCrewCurrent, startCrewCapacity);
        UpdateFoodAmount(startFood);
        UpdateFoodPerTickAmount(startFoodPerTick);
        UpdateHullDurabilityAmount(startShipHealthCurrent, startShipHealthMax);
        //UpdateCrewMorale(startMorale);
    }
}
