/*
 * CrewManagementAlertConfirmation.cs
 * Author(s): Frank Calabrese
 * Created on: 3/1/2021 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewManagementAlertConfirmation : MonoBehaviour
{
    [SerializeField] GameObject crewAlert;
    [SerializeField] GameObject foodAlert;
    [SerializeField] GameObject foodPerTickAlert;
    [SerializeField] GameObject weaponsAlert;
    [SerializeField] GameObject securityAlert;
    [SerializeField] GameObject hullAlert;
    [SerializeField] GameObject alertPanel;
    [SerializeField] GameObject continuePanel;


    [SerializeField, Tooltip("0-2 = food | 3-5 = security | 6-8 = weapons ::: one for each job for each stat")] private List<int> thresholdAmounts = new List<int>();
    [SerializeField] private int hullRepairPrice = 0;
    [SerializeField] private int crewReplacePrice = 0;

    //Color tempcolor;

    int[] stats;

    public void CheckStats()
    {
        //tempcolor = crewAlert.GetComponent<MeshRenderer>().material.color;
        //tempcolor.a = 1f;


        stats = FindObjectOfType<ShipStats>().GetCoreStats();

        if (stats[0] != 0 && stats[6] > crewReplacePrice) //if the player does not have crew max and can replace them
        {
            alertPanel.gameObject.SetActive(true);
            //crewAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            crewAlert.SetActive(true);
        }
        if (stats[1] != 0 && stats[6] > hullRepairPrice) //is the ship not a full hull durablility
        {
            alertPanel.gameObject.SetActive(true);
            //hullAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            hullAlert.SetActive(true);
        }
        
        if (stats[3] - stats[8] < 0) //negative food production
        {
            alertPanel.gameObject.SetActive(true);
            //foodPerTickAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            foodPerTickAlert.SetActive(true);
        }
        

        switch(FindObjectOfType<CampaignManager>().GetCurrentJobIndex())
        {
            case 0:
                if (stats[2] <= thresholdAmounts[0]) // less food than threshold
                {
                    alertPanel.gameObject.SetActive(true);
                    //foodAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    foodAlert.SetActive(true);
                }
                if (stats[4] <= thresholdAmounts[3]) // less security than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //securityAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    securityAlert.SetActive(true);
                }
                if (stats[5] <= thresholdAmounts[6]) // less weapons than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    weaponsAlert.SetActive(true);
                }
                break;
            case 1:
                if (stats[2] <= thresholdAmounts[1]) // less food than threshold
                {
                    alertPanel.gameObject.SetActive(true);
                    //foodAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    foodAlert.SetActive(true);
                }
                if (stats[4] <= thresholdAmounts[4]) // less security than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //securityAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    securityAlert.SetActive(true);
                }
                if (stats[5] <= thresholdAmounts[7]) // less weapons than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    weaponsAlert.SetActive(true);
                }
                break;
            case 2:
                if (stats[2] <= thresholdAmounts[2]) // less food than threshold
                {
                    alertPanel.gameObject.SetActive(true);
                    //foodAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    foodAlert.SetActive(true);
                }
                if (stats[4] <= thresholdAmounts[5]) // less security than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //securityAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    securityAlert.SetActive(true);
                }
                if (stats[5] <= thresholdAmounts[8]) // less weapons than 
                {
                    alertPanel.gameObject.SetActive(true);
                    //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
                    weaponsAlert.SetActive(true);
                }
                break;
        }

        // if (stats[7] != 0) //ship has less remaining power than unassigned power
        // {
        //     alertPanel.gameObject.SetActive(true);
        //     //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
        //     weaponsAlert.SetActive(true);
        // }

        if (alertPanel.activeSelf == false) continuePanel.SetActive(true);
    }

    public void DeactivateAlerts()
    {
        //tempcolor = crewAlert.GetComponent<MeshRenderer>().material.color;
        //tempcolor.a = 0.5f;

        crewAlert.SetActive(false);
        foodAlert.SetActive(false);
        foodPerTickAlert.SetActive(false);
        securityAlert.SetActive(false);
        weaponsAlert.SetActive(false);
        hullAlert.SetActive(false);
        alertPanel.gameObject.SetActive(false);


    }
}
