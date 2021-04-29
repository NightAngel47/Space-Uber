/*
 * CrewManagementAlertConfirmation.cs
 * Author(s): Frank Calabrese, Sydney Foe, Steven Drovie
 * Created on: 3/1/2021 (en-US)
 * Description: Handles the functionality of the alert screen in crew management.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct AlertThresholds
{
    public enum ThresholdStat { Food, Security, ShipWeapons }
    [Tooltip("Food, Security, ShipWeapons")] public int[] thresholds;
}

[Serializable]
public struct AlertCheck
{
    [Tooltip("Associated Resource for the check")] public ResourceDataType checkResource;
    [Tooltip("Prompt shown when check passes"), TextArea] public string passPrompt;
    [Tooltip("Prompt shown when check fails"), TextArea] public string failPrompt;
}

public class CrewManagementAlertConfirmation : MonoBehaviour
{
    // checks
    [SerializeField] private AlertCheck[] checks = new AlertCheck[0];
    
    // refill prices
    [SerializeField, Foldout("Refill Prices")] private int hullRepairPrice = 0;
    [SerializeField, Foldout("Refill Prices")] private int crewReplacePrice = 0;
    [SerializeField, Foldout("Refill Prices")] private int energyReplacePrice = 0;
    
    // UI variables
    [SerializeField, Foldout("UI Variables")] GameObject alertPanel;
    public GameObject AlertPanel => alertPanel;
    [SerializeField, Foldout("UI Variables"), Tooltip("Delay is seconds between checks")] private float checkDelay = 0.1f;
    [SerializeField, Foldout("UI Variables"), Tooltip("Delay is seconds before the error appears in diagnosis")] private float diagnosisDelay = 0.1f;
    [SerializeField, Foldout("UI Variables")] private GameObject navButtons;
    [SerializeField, Foldout("Lists")] private RectTransform preflightList;
    [SerializeField, Foldout("Lists")] private RectTransform diagnosisList;
    [SerializeField, Foldout("Checklist Prefabs")] private GameObject checklistEntry;
    [SerializeField, Foldout("Checklist Prefabs")] private GameObject checklistEntryError;
    [SerializeField, Foldout("Checklist Prefabs")] private GameObject diagnosisNoError;

    private ShipStats ship;
    private bool noErrors = true;
    private float entryHeight = 100;

    public string[] Goods;
    public string[] Bads;

    private List<GameObject> spawnedChecks = new List<GameObject>();

    private void Awake()
    {
        entryHeight = checklistEntry.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Start()
    {
        ship = FindObjectOfType<ShipStats>();
    }

    /// <summary>
    /// Called by begin job button
    /// </summary>
    public void RunPreFlightCheck()
    {
        // reset before checking
        navButtons.SetActive(false);
        preflightList.sizeDelta = new Vector2(preflightList.sizeDelta.x, 0);
        diagnosisList.sizeDelta = new Vector2(diagnosisList.sizeDelta.x, 0);;
        noErrors = true;
        
        alertPanel.SetActive(true);
        StartCoroutine(PreFlightCheck());
    }

    private IEnumerator PreFlightCheck()
    {
        yield return new WaitForSeconds(checkDelay);

        for (int i = 0; i < checks.Length; ++i)
        {
            // lengthen height scroll preflight content container
            Vector2 preflightListSizeDelta = preflightList.sizeDelta;
            preflightListSizeDelta.y += entryHeight;
            preflightList.sizeDelta = preflightListSizeDelta;
            
            if (CheckStat(i))
            {
                // spawn passed check
                GameObject checkEntry = Instantiate(checklistEntry, preflightList);
                SpawnCheckEntry(checkEntry, i, true);
                spawnedChecks.Add(checkEntry);
                AudioManager.instance.PlaySFX(Goods[UnityEngine.Random.Range(0, Goods.Length - 1)]);
            }
            else
            {
                // there was an error while checking
                if (noErrors) noErrors = false;
                
                // spawn error check
                GameObject checkEntryError = Instantiate(checklistEntryError, preflightList);
                SpawnCheckEntry(checkEntryError, i, false);
                spawnedChecks.Add(checkEntryError);
                AudioManager.instance.PlaySFX(Bads[UnityEngine.Random.Range(0, Goods.Length - 1)]);

                yield return new WaitForSeconds(diagnosisDelay);
                
                // lengthen height scroll preflight content container
                Vector2 diagnosisListSizeDelta = diagnosisList.sizeDelta;
                diagnosisListSizeDelta.y += entryHeight;
                diagnosisList.sizeDelta = diagnosisListSizeDelta;
                
                // spawn error diagnosis
                GameObject diagnosisEntryError = Instantiate(checklistEntryError, diagnosisList);
                SpawnCheckEntry(diagnosisEntryError, i, false);
                spawnedChecks.Add(diagnosisEntryError);
            }
            
            yield return new WaitForSeconds(checkDelay);
        }

        // have no error check spawn if no errors reported
        if (noErrors)
        {
            // lengthen height scroll preflight content container
            Vector2 diagnosisListSizeDelta = diagnosisList.sizeDelta;
            diagnosisListSizeDelta.y += entryHeight;
            diagnosisList.sizeDelta = diagnosisListSizeDelta;
            
            GameObject noErrorEntry = Instantiate(diagnosisNoError, diagnosisList);
            spawnedChecks.Add(noErrorEntry);
            AudioManager.instance.PlaySFX("Power Up");
        }
        
        yield return new WaitForSeconds(diagnosisDelay);
        
        navButtons.SetActive(true);
    }

    
    /// <summary>
    /// Checks stats for each check index.
    /// </summary>
    /// <param name="checkIndex">The check index that is being checked</param>
    /// <returns>True if check passes and player is good. False if check fails and player is not good.</returns>
    private bool CheckStat(int checkIndex)
    {
        switch(checkIndex)
        {
            case 0: // positive food production
                return ship.FoodPerTick - ship.CrewCurrent.x >= 0;
            case 1: // greater/equal food than job threshold
                return ship.Food >= EventSystem.instance.CurrentJob.alertThresholds.thresholds[(int)AlertThresholds.ThresholdStat.Food];
            case 2: // greater/equal security than job threshold
                return ship.Security >= EventSystem.instance.CurrentJob.alertThresholds.thresholds[(int) AlertThresholds.ThresholdStat.Security];
            case 3: // greater/equal ship weapons than job threshold
                return ship.Security >= EventSystem.instance.CurrentJob.alertThresholds.thresholds[(int) AlertThresholds.ThresholdStat.ShipWeapons];
            case 4: // current crew equal to crew capacity or below with credits to fix
                if ((int) ship.CrewCurrent.y == (int) ship.CrewCurrent.x) // if current crew is at capacity
                {
                    return true;
                }
                else if(ship.Credits < crewReplacePrice) // if below crew capacity, but can't buy crew: it passes because the player can't fix it.
                {
                    return true;
                }
                return false; // fails if player is below and can buy crew
            case 5: // no unassigned crew with places to assign them
                return !(ship.CrewCurrent.z > 0) || FindObjectsOfType<RoomStats>().Sum(room => room.maxCrew - room.currentCrew) <= 0;
            case 6: // hull durability is at max or below with credits to fix
                if ((int) ship.ShipHealthCurrent.x == (int) ship.ShipHealthCurrent.y) // if current health is at max
                {
                    return true;
                }
                else if(ship.Credits < hullRepairPrice) // if below max, but can't buy hull: it passes because the player can't fix it.
                {
                    return true;
                }
                return false; // fails if player is below and can buy hull
            case 7: // remaining power is at max or below with credits to fix
                if ((int) ship.Energy.x == (int) ship.Energy.z) // if energy remaining is equal to unassigned
                {
                    return true;
                }
                else if(ship.Credits < energyReplacePrice) // if below max, but can't buy energy: it passes because the player can't fix it.
                {
                    return true;
                }
                return false; // fails if player is below and can buy energy
            default:
                return true;
        }
    }

    /// <summary>
    /// Sets the text and icon for the check list entry
    /// </summary>
    /// <param name="checkEntry">Newly spawned check list entry</param>
    /// <param name="index">Check index to get the data from</param>
    /// <param name="didPass">Did the check pass? Used to determine message.</param>
    private void SpawnCheckEntry(GameObject checkEntry, int index, bool didPass)
    {
        checkEntry.GetComponentInChildren<TMP_Text>().text = didPass ? checks[index].passPrompt : checks[index].failPrompt;
        checkEntry.transform.GetChild(0).GetComponent<Image>().sprite = checks[index].checkResource.resourceIcon;
    }

    public void DeactivateAlerts()
    {
        alertPanel.gameObject.SetActive(false);

        foreach (GameObject check in spawnedChecks)
        {
            Destroy(check);
        }
        
        spawnedChecks.Clear();
    }
}
