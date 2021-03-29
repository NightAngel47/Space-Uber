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


    [SerializeField, Tooltip("0 = food | 1 = security | 2 = weapons")] private List<int> thresholdAmounts = new List<int>();

    //Color tempcolor;

    int[] stats;

    public void CheckStats()
    {
        //tempcolor = crewAlert.GetComponent<MeshRenderer>().material.color;
        //tempcolor.a = 1f;


        stats = FindObjectOfType<ShipStats>().GetCoreStats();

        if (stats[0] != 0) //if the player does not have crew max and can replace them
        {
            alertPanel.gameObject.SetActive(true);
            //crewAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            crewAlert.SetActive(true);
        }
        if (stats[1] != 0) //is the ship not a full hull durablility
        {
            alertPanel.gameObject.SetActive(true);
            //hullAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            hullAlert.SetActive(true);
        }
        if (stats[2] <= thresholdAmounts[0])
        {
            alertPanel.gameObject.SetActive(true);
            //foodAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            foodAlert.SetActive(true);
        }
        if (stats[3] - stats[8] < 0) //negative food production
        {
            alertPanel.gameObject.SetActive(true);
            //foodPerTickAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            foodPerTickAlert.SetActive(true);
        }
        if (stats[4] <= thresholdAmounts[1])
        {
            alertPanel.gameObject.SetActive(true);
            //securityAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            securityAlert.SetActive(true);
        }
        if (stats[5] <= thresholdAmounts[2])
        {
            alertPanel.gameObject.SetActive(true);
            //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            weaponsAlert.SetActive(true);
        }
        if (stats[7] != 0) //ship has less remaining power than unassigned power
        {
            alertPanel.gameObject.SetActive(true);
            //weaponsAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            weaponsAlert.SetActive(true);
        }

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
