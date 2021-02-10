using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoraleManager : MonoBehaviour
{
    [SerializeField, Tooltip("Starting amount of crewMorale")] private int startingMorale;
    
    private int crewMorale;
    
    private int startCrewMorale;
    
    [SerializeField, Tooltip("Value of the \"normal\" payment that won't affect morale")] private int crewPaymentDefault = 5;
    [SerializeField, Tooltip("Amount crew morale is affected by different payments")] private float crewPaymentMoraleMultiplier = 2;
    [SerializeField, Tooltip("Amount crew morale is affected by crew loss")] private int crewLossMoraleMultiplier = 10;
    [SerializeField, Tooltip("Amount crew morale is affected by starving")] private int foodMoraleDamageMultiplier = 1;
    
    //mutiny variables
    [SerializeField, Tooltip("The highest morale at which mutiny is possible")] private int maxMutinyMorale = 39;
    [SerializeField, Tooltip("The chance of mutiny each tick at the max mutiny morale")] private float maxMutinyMoraleMutinyChance = 0.2f;
    [SerializeField, Tooltip("The chance of mutiny each tick at zero morale")] private float zeroMoraleMutinyChance = 1f;
    [SerializeField, Tooltip("The base cost of paying off mutinies which is modified by other variables")] private int baseMutinyCost = 100;
    private int mutinyCount = 0;
    [SerializeField, Tooltip("The mutiny event")] private GameObject mutinyEvent;
    
    [SerializeField, Tooltip("Values for determining the modifiers to apply to room and minigame output. X is the minimum bound of the tier and Y is the modifier value for that tier")] private List<Vector2> outputModifierInfo;
    
    private ShipStatsUI shipStatsUI;
    
    public static MoraleManager instance;
    
    private void Awake()
    {
        // Singleton pattern that makes sure that there is only one MoraleManager
        if (instance) { Destroy(gameObject); }
        else { instance = this; }
    }
    
    private void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    private void Start()
    {
        CrewMorale = startingMorale;
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
    }
    
    public int CrewMorale
    {
        get => crewMorale;
        set
        {
            int prevValue = crewMorale;
            crewMorale = value;
        
            /*
            if (crewMoraleAmount >= 0)
            {
                AudioManager.instance.PlaySFX("Gain Morale");
            }
            else
            {
                AudioManager.instance.PlaySFX("Lose Morale");
            }
            */
        
            if(crewMorale < 0)
            {
                crewMorale = 0;
            }
        
            if(crewMorale > 100)
            {
                crewMorale = 100;
            }
        
            shipStatsUI.UpdateCrewMoraleUI(crewMorale);
            shipStatsUI.ShowCrewMoraleUIChange(value - prevValue);
        }
    }
    
    public void CrewLoss(int amount)
    {
        CrewMorale += amount * crewLossMoraleMultiplier;
    }
    
    public void CrewPayment(int amount)
    {
        CrewMorale += Mathf.RoundToInt((amount - crewPaymentDefault) * crewPaymentMoraleMultiplier / crewPaymentDefault);
    }
    
    public void CrewStarving(int amount)
    {
        CrewMorale += amount * foodMoraleDamageMultiplier;
    }
    
    public void CheckMutiny()
    {
        float mutinyChance = (maxMutinyMorale - crewMorale) * zeroMoraleMutinyChance * (1 - maxMutinyMoraleMutinyChance) / maxMutinyMorale + maxMutinyMoraleMutinyChance;
        if(mutinyChance > UnityEngine.Random.value)
        {
            mutinyCount++;
            int mutinyCost = Mathf.RoundToInt((baseMutinyCost * ((100 - crewMorale) / 100.0f)) * mutinyCount);
            mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].choiceRequirements[0].requiredAmount = mutinyCost;
            mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].outcomes[0].amount = -mutinyCost;
            EventSystem.instance.CreateMutinyEvent(mutinyEvent);
        }
    }
    
    public float GetMoraleModifier(bool ignoreMorale = false)
    {
        if(ignoreMorale)
        {
            return 1;
        }
        
        foreach(Vector2 info in outputModifierInfo)
        {
            if(crewMorale >= info.x)
            {
                return info.y;
            }
        }
        
        return 1;
    }
    
    public void SaveMorale()
    {
        startCrewMorale = crewMorale;
    }
    
    public void ResetMorale()
    {
        CrewMorale = startCrewMorale;
    }
}
