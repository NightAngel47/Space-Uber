using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewView : MonoBehaviour
{
    [SerializeField] GameObject[] crewSlots;
    [SerializeField] GameObject crewView;

    private void Update()
    {
        if (CrewViewManager.Instance.GetCrewViewStatus() == false)
        {
            crewView.SetActive(false);
        }
        else
        {
            crewView.SetActive(true);

            for (int i = 0; i < gameObject.GetComponent<RoomStats>().currentCrew; i++)
            {
                crewSlots[i].gameObject.SetActive(true);
            }

            for (int i = gameObject.GetComponent<RoomStats>().currentCrew; i < gameObject.GetComponent<RoomStats>().maxCrew; i++)
            {
                crewSlots[i].gameObject.SetActive(false);
            }
        }
    }

        
}
