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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanelInfo()
    {
        roomImage.sprite = selectedRoom.GetComponentInChildren<SpriteRenderer>().sprite;

        roomName.text = selectedRoom.GetComponent<RoomStats>().roomName;
        roomDesc.text = selectedRoom.GetComponent<RoomStats>().roomDescription;
        roomSize.text = selectedRoom.GetComponent<ObjectScript>().roomSize;
        needsCredits.text = "" + selectedRoom.GetComponent<RoomStats>().price;
        needsPower.text = "" + selectedRoom.GetComponent<RoomStats>().minPower;
        needsCrew.text = "" + selectedRoom.GetComponent<RoomStats>().minCrew;
        currentCrew.text = "" + selectedRoom.GetComponent<RoomStats>().currentCrew;

        if (selectedRoom.GetComponent<Resource>() != null)
        {
            producesResource.text = selectedRoom.GetComponent<Resource>().resourceType.resourceName;
            producesAmount.text = "" + selectedRoom.GetComponent<Resource>().amount;
        }
        else
        {
            producesResource.text = "No Production";
            producesAmount.text = "";
        }
    }

    public void ChangeCurrentRoom(GameObject room)
    {
        selectedRoom = room;
    }

    public void AddCrew()
    {
        selectedRoom.GetComponent<RoomStats>().UpdateCurrentCrew(1);
        UpdatePanelInfo();
    }

    public void RemoveCrew()
    {
        selectedRoom.GetComponent<RoomStats>().UpdateCurrentCrew(-1);
        UpdatePanelInfo();
    }
}
