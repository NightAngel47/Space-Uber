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
using UnityEngine.UI;
using UnityEngine.Serialization;

public class ShipStatsUI : MonoBehaviour
{
    [SerializeField, Foldout("Ship Credits UI")] private TMP_Text creditsCurrentText;
    [FormerlySerializedAs("creditsTickText")] [SerializeField, Foldout("Ship Credits UI")] private TMP_Text creditsPayoutText;

    [SerializeField, Foldout("Ship Energy UI")] private TMP_Text energyCurrentText;
    [SerializeField, Foldout("Ship Energy UI")] private TMP_Text energyMaxText;

    [SerializeField, Foldout("Ship Security UI")] private TMP_Text securityCurrentText;
    [SerializeField, Foldout("Ship Weapons UI")] private TMP_Text shipWeaponsCurrentText;

    [SerializeField, Foldout("Ship Crew UI")] private TMP_Text crewCurrentText;
    [SerializeField, Foldout("Ship Crew UI")] private TMP_Text crewMaxText;

    [SerializeField, Foldout("Ship Food UI")] private TMP_Text foodCurrentText;
    [SerializeField, Foldout("Ship Food UI")] private TMP_Text foodTickText;
    [SerializeField, Foldout("Ship Food UI")] private TMP_Text foodTickSignText;

    [SerializeField, Foldout("Ship Hull UI")] private TMP_Text hullCurrentText;
    [SerializeField, Foldout("Ship Hull UI")] private TMP_Text hullMaxText;

    [SerializeField, Foldout("Crew Morale UI")] private Image crewMoraleImage;

    //crew morale icon images
    [SerializeField, Foldout("Crew Morale Icons")] private Sprite crewMoraleIcon1;
    [SerializeField, Foldout("Crew Morale Icons")] private Sprite crewMoraleIcon2;
    [SerializeField, Foldout("Crew Morale Icons")] private Sprite crewMoraleIcon3;
    [SerializeField, Foldout("Crew Morale Icons")] private Sprite crewMoraleIcon4;
    [SerializeField, Foldout("Crew Morale Icons")] private Sprite crewMoraleIcon5;

    //stat change text variables
    [Foldout("Stat Change UI")] public GameObject statChangeText;
    [Foldout("Stat Change UI")] public GameObject statChangeTextRoom;
    [Foldout("Stat Change UI")] public Transform canvas;
    [HideInInspector] public GameObject roomBeingPlaced;

    //text jiggle variables
    [SerializeField, Foldout("Stat Change UI")] private float jiggleAmount;
    [SerializeField, Foldout("Stat Change UI")] private float jiggleTime;

    //low hull durability feedback variables
    [SerializeField, Foldout("Ship Hull UI")] private float blinkTime;
    [SerializeField, Foldout("Ship Hull UI")] private float blinkTransitionTime;
    [SerializeField, Foldout("Ship Hull UI")] private float beepTime;
    [SerializeField, Foldout("Ship Hull UI")] private Color hullTextDefault;
    [SerializeField, Foldout("Ship Hull UI")] private Color hullTextRed;
    [SerializeField, Foldout("Ship Hull UI")] private int hullWarningAmount = 25;

    [SerializeField, Foldout("Ship Credits Tooltip")] private TMP_Text creditsCurrentTooltipText;
    [FormerlySerializedAs("creditsTickTooltipText")] [SerializeField, Foldout("Ship Credits Tooltip")] private TMP_Text creditsPayoutTooltipText;

    [SerializeField, Foldout("Ship Energy Tooltip")] private TMP_Text energyCurrentTooltipText;
    [SerializeField, Foldout("Ship Energy Tooltip")] private TMP_Text energyMaxTooltipText;

    [SerializeField, Foldout("Ship Security Tooltip")] private TMP_Text securityCurrentTooltipText;
    [SerializeField, Foldout("Ship Weapons Tooltip")] private TMP_Text shipWeaponsCurrentTooltipText;

    [SerializeField, Foldout("Ship Crew Tooltip")] private TMP_Text crewUnassignedTooltipText;
    [SerializeField, Foldout("Ship Crew Tooltip")] private TMP_Text crewCurrentTooltipText;
    [SerializeField, Foldout("Ship Crew Tooltip")] private TMP_Text crewMaxTooltipText;

    [SerializeField, Foldout("Ship Food Tooltip")] private TMP_Text foodCurrentTooltipText;
    [SerializeField, Foldout("Ship Food Tooltip")] private TMP_Text foodTickTooltipText;
    [SerializeField, Foldout("Ship Food Tooltip")] private TMP_Text foodNetTooltipText;

    [SerializeField, Foldout("Ship Hull Tooltip")] private TMP_Text hullCurrentTooltipText;
    [SerializeField, Foldout("Ship Hull Tooltip")] private TMP_Text hullMaxTooltipText;

    private bool hullWarningActive = false;

    public void UpdateCreditsUI(int current, int payout = 0)
    {
        creditsCurrentText.text = current.ToString();
        creditsPayoutText.text = payout.ToString();
        creditsCurrentTooltipText.text = current.ToString();
        creditsPayoutTooltipText.text = payout.ToString();
    }

    public void ShowCreditsUIChange(int currentChange, int payoutChange = 0)
    {
        if (currentChange != 0)
        {
            SpawnStatChangeText(creditsCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon, 0);
            StartCoroutine(JiggleText(creditsCurrentText));
        }

        if (payoutChange != 0)
        {
            SpawnStatChangeText(creditsPayoutText, payoutChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Credits).resourceIcon, 1);
            StartCoroutine(JiggleText(creditsPayoutText));
        }
    }

    public void UpdateEnergyUI(int current, int max)
    {
        energyCurrentText.text = current.ToString();
        energyMaxText.text = max.ToString();
        energyCurrentTooltipText.text = current.ToString();
        energyMaxTooltipText.text = max.ToString();
    }

    public void ShowEnergyUIChange(int currentChange, int maxChange)
    {
        if (currentChange != 0)
        {
            SpawnStatChangeText(energyCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon, 1);
            StartCoroutine(JiggleText(energyCurrentText));
        }

        if (maxChange != 0)
        {
            SpawnStatChangeText(energyMaxText, maxChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Energy).resourceIcon, 2);
            StartCoroutine(JiggleText(energyMaxText));
        }
    }

    public void UpdateSecurityUI(int current)
    {
        securityCurrentText.text = current.ToString();
        securityCurrentTooltipText.text = current.ToString();
    }

    public void ShowSecurityUIChange(int currentChange)
    {
        if (currentChange != 0)
        {
            SpawnStatChangeText(securityCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Security).resourceIcon, 2);
            StartCoroutine(JiggleText(securityCurrentText));
        }
    }

    public void UpdateShipWeaponsUI(int current)
    {
        shipWeaponsCurrentText.text = current.ToString();
        shipWeaponsCurrentTooltipText.text = current.ToString();
    }

    public void ShowShipWeaponsUIChange(int currentChange)
    {
        if (currentChange != 0)
        {
            SpawnStatChangeText(shipWeaponsCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._ShipWeapons).resourceIcon, 2);
            StartCoroutine(JiggleText(shipWeaponsCurrentText));
        }
    }

    public void UpdateCrewUI(int unassigned, int current, int max)
    {
        crewCurrentText.text = unassigned.ToString();
        crewMaxText.text = current.ToString();
        crewUnassignedTooltipText.text = unassigned.ToString();
        crewCurrentTooltipText.text = current.ToString();
        crewMaxTooltipText.text = max.ToString();
    }

    public void ShowCrewUIChange(int unassignedChange, int currentChange, int maxChange)
    {
        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding)
        {
            SpawnStatChangeText(crewCurrentText, unassignedChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon, 2);
            SpawnStatChangeText(crewMaxText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon, 3);
        }
        else
        {
            SpawnStatChangeText(crewCurrentText, unassignedChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon, 1, true);
            SpawnStatChangeText(crewMaxText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Crew).resourceIcon, 2);
        }

        if(unassignedChange != 0)
        {
            StartCoroutine(JiggleText(crewCurrentText));
        }

        if(currentChange != 0)
        {
            StartCoroutine(JiggleText(crewMaxText));
        }
    }

    public void UpdateFoodUI(int current, int tick, int crew)
    {
        foodCurrentText.text = current.ToString();

        int netFood = tick - crew;
        foodTickText.text = Mathf.Abs(netFood).ToString();
        foodTickSignText.text = netFood >= 0 ? "+" : "-";
        
        foodCurrentTooltipText.text = current.ToString();
        foodTickTooltipText.text = tick.ToString();
        foodNetTooltipText.text = netFood.ToString();
    }

    public void ShowFoodUIChange(int currentChange, int tickChange)
    {
        if (currentChange != 0)
        {
            SpawnStatChangeText(foodCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon, 2);
            StartCoroutine(JiggleText(foodCurrentText));
        }

        if (tickChange != 0)
        {
            SpawnStatChangeText(foodTickText, tickChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._Food).resourceIcon, 2);
            StartCoroutine(JiggleText(foodTickText));
        }
    }

    public void UpdateHullUI(int current, int max)
    {
        hullCurrentText.text = current.ToString();
        hullMaxText.text = max.ToString();
        hullCurrentTooltipText.text = current.ToString();
        hullMaxTooltipText.text = max.ToString();

        if (current <= hullWarningAmount && hullWarningActive == false)
        {
            hullWarningActive = true;
            StartCoroutine(BlinkLoop());
            StartCoroutine(BeepLoop());
        }

        if (current > hullWarningAmount && hullWarningActive == true)
        {
            hullWarningActive = false;
        }
    }

    public void ShowHullUIChange(int currentChange, int maxChange)
    {
        if(currentChange != 0)
        {
            SpawnStatChangeText(hullCurrentText, currentChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon, 2);
            StartCoroutine(JiggleText(hullCurrentText));
        }

        if(maxChange != 0)
        {
            SpawnStatChangeText(hullMaxText, maxChange, GameManager.instance.GetResourceData((int)ResourceDataTypes._HullDurability).resourceIcon, 3);
            StartCoroutine(JiggleText(hullMaxText));
        }
    }

    public void UpdateCrewMoraleUI(int current)
    {
        Sprite moraleIcon = null;

        switch(current)
        {
            case int cur when current >= MoraleManager.instance.OutputModifierInfo[0].x && current <= 100:
                moraleIcon = crewMoraleIcon1;
                break;
            case int cur when current >= MoraleManager.instance.OutputModifierInfo[1].x && current < MoraleManager.instance.OutputModifierInfo[0].x:
                moraleIcon = crewMoraleIcon2;
                break;
            case int cur when current >= MoraleManager.instance.OutputModifierInfo[2].x && current < MoraleManager.instance.OutputModifierInfo[1].x:
                moraleIcon = crewMoraleIcon3;
                break;
            case int cur when current >= MoraleManager.instance.OutputModifierInfo[3].x && current < MoraleManager.instance.OutputModifierInfo[2].x:
                moraleIcon = crewMoraleIcon4;
                break;
            case int cur when current >= MoraleManager.instance.OutputModifierInfo[4].x && current < MoraleManager.instance.OutputModifierInfo[3].x:
                moraleIcon = crewMoraleIcon5;
                break;
            default:
                break;
        }

        crewMoraleImage.sprite = moraleIcon;
    }

    public void ShowCrewMoraleUIChange(int currentChange, bool hidden = false)
    {
        if(currentChange > 0)
        {
            //Maybe Spawn Stat Change Text

            if(currentChange != 0)
            {
                StartCoroutine(JiggleImage(crewMoraleImage));
            }
        }
    }

    /// <summary>
    /// Spawns a pop-up of a number below the stat bar and where the object was clicked. Shows exactly how much a stat changes
    /// </summary>
    /// <param name="statText"> The text that it spawns the next text under</param>
    /// <param name="value"> How much the stat was changed</param>
    /// <param name="icon">The icon it should use when spawning</param>
    /// <param name="canvasNum">The canvas that stat ui should spawn at</param>
    /// <param name="oppositeValueOnRoom">Default false. If true the resouce value will be opposite in -/+ compared to the ship stat bar</param>
    private void SpawnStatChangeText(TMP_Text statText, int value, Sprite icon, int canvasNum, bool oppositeValueOnRoom = false)
    {
        if(value != 0)
        {
            //ERROR: something gets destroyed that it is trying to access
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

            if (roomBeingPlaced != null)
            {
                Vector2 pos = roomBeingPlaced.transform.position;  // get the game object position
                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

                GameObject instanceRoom = Instantiate(statChangeTextRoom, roomBeingPlaced.GetComponent<RoomStats>().statCanvas[canvasNum]);

                RectTransform rectRoom = instanceRoom.GetComponent<RectTransform>();
                RectTransform statRectRoom = statText.gameObject.GetComponent<RectTransform>();

                //rectRoom.anchorMax = viewportPoint;
                //rectRoom.anchorMin = viewportPoint;
                //rectRoom.offsetMax = statRectRoom.offsetMax;
                //rectRoom.offsetMin = statRectRoom.offsetMin;
                //rectRoom.pivot = statRectRoom.pivot;
                //rectRoom.sizeDelta = statRectRoom.sizeDelta;
                //rectRoom.anchoredPosition = roomBeingPlaced.transform.position;

                TMP_Text textRoom = instanceRoom.GetComponent<TMP_Text>();

                //textRoom.fontSize = statText.fontSize;
                //textRoom.alignment = statText.alignment;

                MoveAndFadeBehaviour moveAndFadeBehaviourRoom = instanceRoom.GetComponent<MoveAndFadeBehaviour>();
                moveAndFadeBehaviourRoom.offset = new Vector2(0, -.5f);
                if (oppositeValueOnRoom) // add option to reverse value for stat change UI so that rooms can show removing crew and stat bar can show gaining crew
                {
                    moveAndFadeBehaviourRoom.SetValue(-value, icon);
                }
                else
                {
                    moveAndFadeBehaviourRoom.SetValue(value, icon);
                }
            }
        }
    }

    private IEnumerator JiggleText(TMP_Text text)
    {
        IEnumerator coroutine = TextJiggling(text);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(jiggleTime);
        StopCoroutine(coroutine);
        text.GetComponent<RectTransform>().anchoredPosition = text.GetComponent<StartPositionTracker>().GetPos();
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

    private IEnumerator JiggleImage(Image image)
    {
        IEnumerator coroutine = ImageJiggling(image);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(jiggleTime);
        StopCoroutine(coroutine);
        image.GetComponent<RectTransform>().anchoredPosition = image.GetComponent<StartPositionTracker>().GetPos();
    }

    private IEnumerator ImageJiggling(Image image)
    {
        while(true)
        {
            Vector2 pos = image.GetComponent<RectTransform>().anchoredPosition;
            image.GetComponent<RectTransform>().anchoredPosition = pos + UnityEngine.Random.insideUnitCircle * jiggleAmount;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator BlinkLoop()
    {
        AudioManager.instance.PlaySFX("Low Health");// if you just want to play a beep sound when the flashing first triggers, you can play it here

        while (hullWarningActive)
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
