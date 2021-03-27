/*
 * CrewManagementAlertConfirmation.cs
 * Author(s): Frank Calabrese
 * Created on: 3/1/2021 (en-US)
 * Description: 
 */

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
    //Color tempcolor;

    int[] stats;

    public void CheckStats()
    {
        //tempcolor = crewAlert.GetComponent<MeshRenderer>().material.color;
        //tempcolor.a = 1f;


        stats = FindObjectOfType<ShipStats>().GetCoreStats();

        if (stats[0] > 0)
        {
            alertPanel.gameObject.SetActive(true);
            //crewAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            crewAlert.SetActive(true);
        }
        if (stats[1] > 0)
        {
            alertPanel.gameObject.SetActive(true);
            //hullAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            hullAlert.SetActive(true);
        }
        if (stats[2] <= 0)
        {
            alertPanel.gameObject.SetActive(true);
            //foodAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            foodAlert.SetActive(true);
        }
        if (stats[3] <= 0)
        {
            alertPanel.gameObject.SetActive(true);
            //foodPerTickAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            foodPerTickAlert.SetActive(true);
        }
        if (stats[4] <= 0)
        {
            alertPanel.gameObject.SetActive(true);
            //securityAlert.GetComponent<MeshRenderer>().material.color = tempcolor;
            securityAlert.SetActive(true);
        }
        if (stats[5] <= 0)
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
