/*
 * ShipStatsUI.cs
 * Author(s): Steven Drovie []
 * Created on: 10/7/2020 (en-US)
 * Description: Updates the Stats Bar to show the ship's stats.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ShipStatsUI : MonoBehaviour
{
    [SerializeField, Foldout("Ship Credits UI")] private TMP_Text creditsCurrentText;
    [SerializeField, Foldout("Ship Credits UI")] private TMP_Text creditsTickText;
    
    [SerializeField, Foldout("Ship Energy UI")] private TMP_Text energyCurrentText;
    [SerializeField, Foldout("Ship Energy UI")] private TMP_Text energyMaxText;
    
    [SerializeField, Foldout("Ship Security UI")] private TMP_Text securityCurrentText;
    [SerializeField, Foldout("Ship Weapons UI")] private TMP_Text shipWeaponsCurrentText;
    
    [SerializeField, Foldout("Ship Crew UI")] private TMP_Text crewCurrentText;
    [SerializeField, Foldout("Ship Crew UI")] private TMP_Text crewMaxText;
    
    [SerializeField, Foldout("Ship Food UI")] private TMP_Text foodCurrentText;
    [SerializeField, Foldout("Ship Food UI")] private TMP_Text foodTickText;
    
    [SerializeField, Foldout("Ship Hull UI")] private TMP_Text hullCurrentText;
    [SerializeField, Foldout("Ship Hull UI")] private TMP_Text hullMaxText;
    
    //stat change text variables
    public GameObject statChangeText;
    public Transform canvas;
    
    //text jiggle variables
    public float jiggleAmount;
    public float jiggleTime;
    
    //low hull durability feedback variables
    public float blinkTime;
    public float blinkTransitionTime;
    public float beepTime;
    public Color hullTextDefault;
    public Color hullTextRed;
    
    private bool hullWarningActive = false;

    public void UpdateCreditsUI(int current, int tick = 0)
    {
        creditsCurrentText.text = current.ToString();
        creditsTickText.text = tick.ToString();
    }
    
    public void ShowCreditsUIChange(int currentChange, int tickChange = 0)
    {
        SpawnStatChangeText(creditsCurrentText, currentChange);
        SpawnStatChangeText(creditsTickText, tickChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(creditsCurrentText));
        }
        
        if(tickChange != 0)
        {
            StartCoroutine(JiggleText(creditsTickText));
        }
    }
    
    public void UpdateEnergyUI(int current, int max)
    {
        energyCurrentText.text = current.ToString();
        energyMaxText.text = max.ToString();
    }
    
    public void ShowEnergyUIChange(int currentChange, int maxChange)
    {
        SpawnStatChangeText(energyCurrentText, currentChange);
        SpawnStatChangeText(energyMaxText, maxChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(energyCurrentText));
        }
        
        if(maxChange != 0)
        {
            StartCoroutine(JiggleText(energyMaxText));
        }
    }
    
    public void UpdateSecurityUI(int current)
    {
        securityCurrentText.text = current.ToString();
    }
    
    public void ShowSecurityUIChange(int currentChange)
    {
        SpawnStatChangeText(securityCurrentText, currentChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(securityCurrentText));
        }
    }
    
    public void UpdateShipWeaponsUI(int current)
    {
        shipWeaponsCurrentText.text = current.ToString();
    }
    
    public void ShowShipWeaponsUIChange(int currentChange)
    {
        SpawnStatChangeText(shipWeaponsCurrentText, currentChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(shipWeaponsCurrentText));
        }
    }
    
    public void UpdateCrewUI(int current, int max)
    {
        crewCurrentText.text = current.ToString();
        crewMaxText.text = max.ToString();
    }
    
    public void ShowCrewUIChange(int currentChange, int maxChange)
    {
        SpawnStatChangeText(crewCurrentText, currentChange);
        SpawnStatChangeText(crewMaxText, maxChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(crewCurrentText));
        }
        
        if(maxChange != 0)
        {
            StartCoroutine(JiggleText(crewMaxText));
        }
    }
    
    public void UpdateFoodUI(int current, int tick)
    {
        foodCurrentText.text = current.ToString();
        foodTickText.text = tick.ToString();
    }
    
    public void ShowFoodUIChange(int currentChange, int tickChange)
    {
        SpawnStatChangeText(foodCurrentText, currentChange);
        SpawnStatChangeText(foodTickText, tickChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(foodCurrentText));
        }
        
        if(tickChange != 0)
        {
            StartCoroutine(JiggleText(foodTickText));
        }
    }
    
    public void UpdateHullUI(int current, int max)
    {
        hullCurrentText.text = current.ToString();
        hullMaxText.text = max.ToString();
        
        if(current <= 25 && hullWarningActive == false)
        {
            hullWarningActive = true;
            StartCoroutine(BlinkLoop());
            StartCoroutine(BeepLoop());
        }
        
        if(current > 25 && hullWarningActive == true)
        {
            hullWarningActive = false;
        }
    }
    
    public void ShowHullUIChange(int currentChange, int maxChange)
    {
        SpawnStatChangeText(hullCurrentText, currentChange);
        SpawnStatChangeText(hullMaxText, maxChange);
        
        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(hullCurrentText));
        }
        
        if(maxChange != 0)
        {
            StartCoroutine(JiggleText(hullMaxText));
        }
    }
    
    private void SpawnStatChangeText(TMP_Text statText, int value)
    {
        if(value != 0)
        {
            GameObject instance = Instantiate(statChangeText, statText.gameObject.transform.parent);
            
            RectTransform rect = instance.GetComponent<RectTransform>();
            RectTransform statRect = statText.gameObject.GetComponent<RectTransform>();
            
            rect.anchorMax = statRect.anchorMax;
            rect.anchorMin = statRect.anchorMin;
            rect.offsetMax = statRect.offsetMax;
            rect.offsetMin = statRect.offsetMin;
            rect.pivot = statRect.pivot;
            rect.sizeDelta = statRect.sizeDelta;
            rect.anchoredPosition = statRect.anchoredPosition;
            
            TMP_Text text = instance.GetComponent<TMP_Text>();
            
            text.fontSize = statText.fontSize;
            text.alignment = statText.alignment;
            
            MoveAndFadeBehaviour moveAndFadeBehaviour = instance.GetComponent<MoveAndFadeBehaviour>();
            moveAndFadeBehaviour.offset = new Vector2(0, -75);
            moveAndFadeBehaviour.SetValue(value);
        }
    }
    
    private IEnumerator JiggleText(TMP_Text text)
    {
        Vector2 startingPos = text.GetComponent<RectTransform>().anchoredPosition;
        IEnumerator coroutine = TextJiggling(text);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(jiggleTime);
        StopCoroutine(coroutine);
        text.GetComponent<RectTransform>().anchoredPosition = startingPos;
    }
    
    private IEnumerator TextJiggling(TMP_Text text)
    {
        while(true)
        {
            Vector2 pos = text.GetComponent<RectTransform>().anchoredPosition;
            text.GetComponent<RectTransform>().anchoredPosition = pos + UnityEngine.Random.insideUnitCircle * jiggleAmount;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator BlinkLoop()
    {
        // if you just want to play a beep sound when the flashing first triggers, you can play it here
        
        while(hullWarningActive)
        {
            StartCoroutine(BlinkTransition(true));
            yield return new WaitForSeconds(blinkTime);
            StartCoroutine(BlinkTransition(false));
            yield return new WaitForSeconds(blinkTime);
        }
    }
    
    private IEnumerator BlinkTransition(bool toRed)
    {
        float timer = 0;
        Color current;
        while(timer < blinkTransitionTime)
        {
            if(toRed)
            {
                current = Color.Lerp(hullTextDefault, hullTextRed, timer / blinkTransitionTime);
            }
            else
            {
                current = Color.Lerp(hullTextRed, hullTextDefault, timer / blinkTransitionTime);
            }
            
            hullCurrentText.color = current;
            
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        
        if(toRed)
        {
            hullCurrentText.color = hullTextRed;
        }
        else
        {
            hullCurrentText.color = hullTextDefault;
        }
    }
    
    private IEnumerator BeepLoop()
    {
        while(hullWarningActive)
        {
            //if you want to play a contiuous beeping sound while the player is at low heath, play the sound here
            yield return new WaitForSeconds(beepTime);
        }
    }
}
