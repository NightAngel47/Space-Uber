/* Frank Calabrese
 * CrewManagementRoomDetailsMenu.cs
 * holds various stats of rooms like name, image, etc.
 * swaps them out depending on what room is being viewed
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewManagementRoomDetailsMenu : MonoBehaviour
{
    private GameObject selectedRoom = null;

    [SerializeField] Image roomImage;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI roomDesc;
    [SerializeField] TextMeshProUGUI roomSize;
    [SerializeField] TextMeshProUGUI needsCredits;
    [SerializeField] TextMeshProUGUI needsPower;
    [SerializeField] TextMeshProUGUI needsCrew;
    [SerializeField] TextMeshProUGUI producesResource;
    [SerializeField] TextMeshProUGUI producesAmount;

    [SerializeField] TextMeshProUGUI currentCrew;


    // Start is called before the first frame update
    void Start()
    {
       //roomImage = null;
       roomName.text = "";
       roomDesc.text = "";
       roomSize.text = "";
       needsCredits.text = "";
       needsPower.text = "";
       needsCrew.text = "";
       producesResource.text = "";
       producesAmount.text = "";

       currentCrew.text = "";
    }
    public void UpdatePanelInfo()
    {
        roomImage.sprite = selectedRoom.GetComponentInChildren<SpriteRenderer>().sprite;

        RoomStats roomStats = selectedRoom.GetComponent<RoomStats>();
        roomName.text = roomStats.roomName;
        roomDesc.text = roomStats.roomDescription;
        needsCredits.text = roomStats.price.ToString();
        needsPower.text = roomStats.minPower.ToString();
        needsCrew.text = roomStats.minCrew.ToString();
        currentCrew.text = roomStats.currentCrew.ToString();
        roomSize.text = selectedRoom.GetComponent<ObjectScript>().roomSize;

        if (selectedRoom.TryGetComponent(out Resource resource))
        {
            //producesResource.text = resource.resourceType.resourceName;
            //producesAmount.text = "/" + resource.amount + " maximum";
        }
        else
        {
            producesResource.text = "No Production";
            producesAmount.text = "";
        }
    }

    public void UpdateCrewAssignment(int currentCrewAmount)
    {
        currentCrew.text = currentCrewAmount.ToString();
        
        if (selectedRoom.TryGetComponent(out Resource resource))
        {
            producesResource.text = resource.resourceType.resourceName;
            producesAmount.text = resource.activeAmount + "/" + resource.amount + " maximum";
        }
    }

    public void ChangeCurrentRoom(GameObject room)
    {
        selectedRoom = room;
    }
}
