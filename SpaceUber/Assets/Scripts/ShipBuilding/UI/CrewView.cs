/* Frank Calabrese
 * 3/9/21
 * CrewView.cs
 * handles adding/removing crew visual changes in crewView. Also assists CrewViewAutoPopulator.cs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewView : MonoBehaviour
{
    [SerializeField] Sprite vacantSprite;
    [SerializeField] Sprite occupiedSprite;

    [SerializeField] GameObject[] crewSlots = new GameObject[10];
    [SerializeField] GameObject crewView;
    private bool finishedPopulating = false;
    [SerializeField] GameObject redOverlay;
    [SerializeField] float vacantOpacity;

    
    private void Update()
    {
        if (CrewViewManager.Instance.GetCrewViewStatus() == false)
        {
            crewView.SetActive(false);
        }
        else
        {
            crewView.SetActive(true);

            if(finishedPopulating)
            {
                for (int i = 0; i < gameObject.GetComponent<RoomStats>().currentCrew; i++)
                {
                    //crewSlots[i].gameObject.SetActive(true);
                    //crewSlots[i].GetComponent<Image>().sprite = occupiedSprite;
                    var tempcolor = crewSlots[i].GetComponent<Image>().color;
                    tempcolor.a = 1f;
                    crewSlots[i].GetComponent<Image>().color = tempcolor;
                }


                for (int i = gameObject.GetComponent<RoomStats>().currentCrew; i < gameObject.GetComponent<RoomStats>().maxCrew; i++)
                {
                    //crewSlots[i].gameObject.SetActive(false);
                    //crewSlots[i].GetComponent<Image>().sprite = vacantSprite;
                    var tempcolor = crewSlots[i].GetComponent<Image>().color;
                    tempcolor.a = vacantOpacity;
                    crewSlots[i].GetComponent<Image>().color = tempcolor;
                }

                updateCrewViewRotation();
            }
        }
    }

    //Rotates crew icons upright relative to how their room is rotated
    public void updateCrewViewRotation()
    {
        for (int i = 0; i < gameObject.GetComponent<RoomStats>().maxCrew; i++)
        {
            Quaternion temp = crewSlots[i].GetComponentInParent<Transform>().transform.rotation;
            temp.z = - gameObject.GetComponentInParent<Transform>().transform.rotation.z;
            crewSlots[i].GetComponentInParent<Transform>().transform.rotation = temp;
        }
    }

    //called by AutoPopulator to populate list with icons
    public void activateCrewSlot(int index, GameObject crewViewSlotPrefab)
    {
        crewSlots[index] = crewViewSlotPrefab;
    }

    public void finishPopulatingCrewSlots()
    {
        finishedPopulating = true;
    }

    //public void toggleOverlay()
    //{
        //if (redOverlay.activeSelf == true) redOverlay.SetActive(false);
        //else redOverlay.SetActive(true);
    //}
    //public void turnOverlayOff()
    //{
        //redOverlay.SetActive(false);
    //}

        
}
