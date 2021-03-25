using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoraleManager : MonoBehaviour
{
    [SerializeField, Tooltip("Starting amount of crewMorale")] private int startingMorale = 100;
    
    private int crewMorale;
    
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
    
    [SerializeField, Tooltip("Values for determining the modifiers to apply to room and minigame output. X is the minimum bound of the tier and Y is the modifier value for that tier")] 
    private List<Vector2> outputModifierInfo;
    
    public List<Vector2> OutputModifierInfo => outputModifierInfo;

    private ShipStats ship;
    private ShipStatsUI shipStatsUI;
    
    public static MoraleManager instance;
   
    [Tooltip("The morale modifier for each level 1 medbay"),SerializeField]
    private int MedBayBoost1;
    [Tooltip("The morale modifier for each level 2 medbay"), SerializeField]
    private int MedBayBoost2;

    private void Awake()
    {
        // Singleton pattern that makes sure that there is only one MoraleManager
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        shipStatsUI = FindObjectOfType<ShipStatsUI>();
        ship = GameObject.FindObjectOfType<ShipStats>();
        if(!ship)
        {
            print("Could not find ship");
        }
    }
    
    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetMorale();
        }
        else
        {
            CrewMorale = startingMorale;
            SaveMorale();
        }
    }
    
    public int CrewMorale
    {
        get => crewMorale;
        set
        {
            int prevValue = crewMorale;
            float prevMoraleModifier = GetMoraleModifier();

            int medCount1 = 0;
            int medCount2 = 0;
            int medCount3 = 0;

            ship.RoomsOfTypeLevel("Medbay", medCount1, medCount2, medCount3);
            int modifier = medCount1 * MedBayBoost1 + medCount2 * MedBayBoost2;

            crewMorale = value + modifier;

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

            // update room outputs if there is a change in morale
            if (value - prevValue != 0 && prevMoraleModifier != GetMoraleModifier())
            {
                RoomStats[] rooms = FindObjectsOfType<RoomStats>();

                foreach (RoomStats room in rooms)
                { 
                    if (!room.ignoreMorale && room.gameObject.GetComponent<ObjectScript>().preplacedRoom == false)
                    {
                        room.KeepRoomStatsUpToDateWithMorale();
                    }
                }
            }
            
            if (crewMorale < 0)
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
            if(DevelopmentAccess.instance.cheatModeActive && CheatsMenu.instance != null && CheatsMenu.instance.mutinyDisabled)
            {
                Debug.Log("Cheated Mutiny");
            }
            else
            {
                mutinyCount++;
                int mutinyCost = Mathf.RoundToInt((baseMutinyCost * ((100 - crewMorale) / 100.0f)) * mutinyCount);
                mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].choiceRequirements[0].requiredAmount = mutinyCost;
                mutinyEvent.GetComponent<InkDriverBase>().nextChoices[0].outcomes[0].amount = -mutinyCost;
                EventSystem.instance.CreateMutinyEvent(mutinyEvent);
            }
        }
    }
    
    public float GetMoraleModifier(bool ignoreMorale = false)
    {
        if (ignoreMorale) return 1;
        
        foreach (var info in outputModifierInfo.Where(info => crewMorale >= info.x))
        {
            return info.y;
        }

        return 1;
    }
    
    public void SaveMorale()
    {
        SavingLoadingManager.instance.Save<int>("crewMorale", crewMorale);
        SavingLoadingManager.instance.Save<int>("mutinyCount", mutinyCount);
    }
    
    public void ResetMorale()
    {
        crewMorale = SavingLoadingManager.instance.Load<int>("crewMorale");
        shipStatsUI.UpdateCrewMoraleUI(crewMorale);
        mutinyCount = SavingLoadingManager.instance.Load<int>("mutinyCount");
    }
}
