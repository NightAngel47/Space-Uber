/*
 * SlotMachine.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Payouts
{
    public int basePayout111;
    public int basePayout222;
    public int basePayout333;
    public int basePayout444;
    public int basePayout555;
    public int basePayout666;
    public int basePayout11;
    public int basePayout22;
    public int basePayout33;
    public int basePayout44;
    public int basePayout55;
    public int basePayout66;
}

[System.Serializable]
public struct PayoutMultipliers
{
    public float freePayoutMultiplier;
    public float smallPayoutMultiplier;
    public float mediumPayoutMultiplier;
    public float largePayoutMultiplier;
}

public enum BetAmount { Free, Small, Medium, Large}

public class SlotMachine : MiniGame
{
    bool spinning = false;
    bool gameStarted = false;
    bool gameFinished = false;
    [SerializeField] SlotReel[] reels;
    [SerializeField] float spinAfterStopTime = 1;
    [SerializeField] float reelSpeed = 1000;
    [SerializeField] float firstStopSpeedMultiplier = 1.5f;
    [SerializeField] float secondStopSpeedMultiplier = 2f;
    [SerializeField] GameObject bettingPanel;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Slider crank;
    [SerializeField] float crankReturnSpeed = 1;
    [SerializeField] int smallBet = 1;
    [SerializeField] int mediumBet = 5;
    [SerializeField] int largeBet = 10;
    [SerializeField] Payouts payouts;
    [SerializeField] PayoutMultipliers payoutMultipliers;
    ShipStats shipStats;
    int payout = 0;
    BetAmount betAmount = BetAmount.Free;
    List<int> slotValues = new List<int>();

    void Start() 
    {
        errorText.text = "";
        shipStats = OverclockController.instance.ShipStats();
        foreach (SlotReel reel in reels) { reel.SetSpeed(reelSpeed); }
        foreach (SlotReel reel in reels) { reel.SetSpinAfterStopTime(spinAfterStopTime); }
    }

    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != crank.gameObject || !Input.GetKey(KeyCode.Mouse0) )
        {
            crank.value += crankReturnSpeed * Time.deltaTime; 
        }
        if (crank.value == 0) { Spin(); }

        spinning = false;
        int spinningCount = 0;
        foreach(SlotReel reel in reels) { if (reel.Spinning()) { spinning = true; spinningCount++; }  }
        if(spinningCount < reels.Length)
		{
            if(spinningCount == 2)foreach (SlotReel reel in reels) { if (reel.Spinning()) { reel.SetSpeed(reelSpeed * firstStopSpeedMultiplier);  } }
            if (spinningCount == 1) foreach (SlotReel reel in reels) { if (reel.Spinning()) { reel.SetSpeed(reelSpeed * secondStopSpeedMultiplier); } }
        }

        if(!spinning && gameStarted && !gameFinished)
		{
            gameFinished = true;
            foreach(SlotReel reel in reels) { slotValues.Add(reel.GetSlot().GetValue()); }
            PayUp();
            statModification = payout;
            winMessage = "You win " + payout + " credits!";
            EndMiniGameSuccess();
        }
    }

    void PayUp()
	{
        float payoutMultiplier = 0f;
        int oneCount = 0;
        int twoCount = 0;
        int threeCount = 0;
        int fourCount = 0;
        int fiveCount = 0;
        int sixCount = 0;

        GetMultiplier(ref payoutMultiplier);
        CountResults(ref oneCount, ref twoCount, ref threeCount, ref fourCount, ref fiveCount, ref sixCount);
        float basePayout = GetBasePayout(ref oneCount, ref twoCount, ref threeCount, ref fourCount, ref fiveCount, ref sixCount);
        payout =  Mathf.RoundToInt(basePayout * payoutMultiplier);
	}

    void GetMultiplier(ref float payoutMultiplier)
    {
        switch (betAmount)
        {
            case BetAmount.Free: payoutMultiplier = payoutMultipliers.freePayoutMultiplier; break;
            case BetAmount.Small: payoutMultiplier = payoutMultipliers.smallPayoutMultiplier; break;
            case BetAmount.Medium: payoutMultiplier = payoutMultipliers.mediumPayoutMultiplier; break;
            case BetAmount.Large: payoutMultiplier = payoutMultipliers.largePayoutMultiplier; break;
        }
    }

    void CountResults(ref int oneCount, ref int twoCount, ref int threeCount, ref int fourCount, ref int fiveCount, ref int sixCount)
	{
        foreach (SlotReel reel in reels)
        {
            switch (reel.GetSlot().GetValue())
            {
                case 1: oneCount++; break;
                case 2: twoCount++; break;
                case 3: threeCount++; break;
                case 4: fourCount++; break;
                case 5: fiveCount++; break;
                case 6: sixCount++; break;
            }
        }
    }
    
    int GetBasePayout(ref int oneCount, ref int twoCount, ref int threeCount, ref int fourCount, ref int fiveCount, ref int sixCount)
	{
        if (oneCount > 1) { if (oneCount > 2) { return payouts.basePayout111; } else { return payouts.basePayout11; } }
        if (twoCount > 1) { if (twoCount > 2) { return payouts.basePayout222; } else { return payouts.basePayout22; } }
        if (threeCount > 1) { if (threeCount > 2) { return payouts.basePayout333; } else { return payouts.basePayout33; } }
        if (fourCount > 1) { if (fourCount > 2) { return payouts.basePayout444; } else { return payouts.basePayout22; } }
        if (fiveCount > 1) { if (fiveCount > 2) { return payouts.basePayout555; } else { return payouts.basePayout55; } }
        if (sixCount > 1) { if (sixCount > 2) { return payouts.basePayout666; } else { return payouts.basePayout66; } }
        return 0;
	}

    public void Option1()
    {
        bettingPanel.SetActive(false);
        betAmount = BetAmount.Free;
    }

    public void Option2()
    {
        if(shipStats.Credits < smallBet) { StartCoroutine(PromptError("Not enough credits!")); }
        {
            shipStats.UpdateCreditsAmount(-smallBet);
            bettingPanel.SetActive(false);
            betAmount = BetAmount.Small;
        }
    }

    public void Option3()
    {
        if (shipStats.Credits < mediumBet) { StartCoroutine(PromptError("Not enough credits!")); }
        {
            shipStats.UpdateCreditsAmount(-mediumBet);
            bettingPanel.SetActive(false);
            betAmount = BetAmount.Medium;
        }
    }

    public void Option4()
    {
        if (shipStats.Credits < largeBet) { StartCoroutine(PromptError("Not enough credits!")); }
        {
            shipStats.UpdateCreditsAmount(-largeBet);
            bettingPanel.SetActive(false);
            betAmount = BetAmount.Large;
        }
    }

    public void Spin()
	{
        if (!gameStarted)
        {
            gameStarted = true;
            foreach (SlotReel reel in reels) { reel.StartSpining(); }
        }
	}

    IEnumerator PromptError(string error)
	{
        errorText.text = error;
        yield return new WaitForSeconds(3);
        errorText.text = "";
	}
}