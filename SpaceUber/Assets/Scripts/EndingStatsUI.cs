using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text shipWeaponsText;
    [SerializeField] private TMP_Text securityText;
    [SerializeField] private TMP_Text hullDurabilityText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text crewText;
    [SerializeField] private TMP_Text roomsBoughtText;
    [SerializeField] private TMP_Text crewDeathsText;
    
    [System.Serializable]
    public struct ChoicePair
    {
        public GameObject a;
        [SerializeField]
        public GameObject b;
        [SerializeField]
        public int outcomeIndex;
    }
    
    [SerializeField] private ChoicePair[] cttrChoiceText;
    [SerializeField] private ChoicePair[] meChoiceText;
    [SerializeField] private ChoicePair[] kftChoiceText;
    
    private void Start()
    {
        creditsText.text = EndingStats.instance.GetStat(EndingStatTypes.Credits).ToString();
        energyText.text = EndingStats.instance.GetStat(EndingStatTypes.Energy).ToString();
        shipWeaponsText.text = EndingStats.instance.GetStat(EndingStatTypes.ShipWeapons).ToString();
        securityText.text = EndingStats.instance.GetStat(EndingStatTypes.Security).ToString();
        hullDurabilityText.text = EndingStats.instance.GetStat(EndingStatTypes.HullDurability).ToString();
        foodText.text = EndingStats.instance.GetStat(EndingStatTypes.Food).ToString();
        crewText.text = EndingStats.instance.GetStat(EndingStatTypes.Crew).ToString();
        roomsBoughtText.text = EndingStats.instance.GetStat(EndingStatTypes.RoomsBought).ToString();
        crewDeathsText.text = EndingStats.instance.GetStat(EndingStatTypes.CrewDeaths).ToString();
        
        CampaignManager cm = EventSystem.instance.GetComponent<CampaignManager>();
        
        for(int i = 0; i < cttrChoiceText.Length; i++)
        {
            bool choice = cm.cateringToTheRich.GetCtrNarrativeOutcome(cttrChoiceText[i].outcomeIndex);
            cttrChoiceText[i].a.SetActive(choice);
            cttrChoiceText[i].b.SetActive(!choice);
        }
        
        for(int i = 0; i < meChoiceText.Length; i++)
        {
            bool choice = cm.mysteriousEntity.GetMeNarrativeVariable(cttrChoiceText[i].outcomeIndex);
            cttrChoiceText[i].a.SetActive(choice);
            cttrChoiceText[i].b.SetActive(!choice);
        }
        
        for(int i = 0; i < kftChoiceText.Length; i++)
        {
            bool choice = cm.finalTest.GetFtNarrativeVariable(cttrChoiceText[i].outcomeIndex);
            cttrChoiceText[i].a.SetActive(choice);
            cttrChoiceText[i].b.SetActive(!choice);
        }
    }
}
