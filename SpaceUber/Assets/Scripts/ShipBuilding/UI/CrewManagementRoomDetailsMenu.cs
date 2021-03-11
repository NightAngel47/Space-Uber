/* Frank Calabrese
 * CrewManagementRoomDetailsMenu.cs
 * holds various stats of rooms like name, image, etc.
 * swaps them out depending on what room is being viewed
 */

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewManagementRoomDetailsMenu : MonoBehaviour
{
    private GameObject selectedRoom = null;

    [SerializeField, Foldout("UI Elements")] Image roomImage;
    [SerializeField, Foldout("UI Elements")] TMP_Text roomName;
    [SerializeField, Foldout("UI Elements")] TMP_Text roomDesc;
    [SerializeField, Foldout("UI Elements")] TMP_Text roomSize;
    [SerializeField, Foldout("UI Elements")] TMP_Text needsCredits;
    [SerializeField, Foldout("UI Elements")] TMP_Text needsPower;
    [SerializeField, Foldout("UI Elements")] TMP_Text needsCrew;
    [SerializeField, Foldout("UI Elements")] TMP_Text producesResource;
    [SerializeField, Foldout("UI Elements")] TMP_Text producesAmount;
    [SerializeField, Foldout("UI Elements")] TMP_Text currentCrew;

    [SerializeField] private string noRoomSelectedMessage = "Select a room to view its details.";
    [SerializeField] private GameObject[] roomDetailSections = new GameObject[2];


    // Start is called before the first frame update
    void Start()
    {
       roomName.text = noRoomSelectedMessage;
       roomDesc.text = "";
       roomSize.text = "";
       needsCredits.text = "";
       needsPower.text = "";
       needsCrew.text = "";
       producesResource.text = "";
       producesAmount.text = "";

       currentCrew.text = "";

       foreach (GameObject roomDetailSection in roomDetailSections)
       {
           roomDetailSection.SetActive(false);
       }
    }
    public void UpdatePanelInfo()
    {
        foreach (GameObject roomDetailSection in roomDetailSections)
        {
            roomDetailSection.SetActive(true);
        }
        
        roomImage.sprite = selectedRoom.GetComponentInChildren<SpriteRenderer>().sprite;

        RoomStats roomStats = selectedRoom.GetComponent<RoomStats>();
        roomName.text = roomStats.roomName;
        roomDesc.text = roomStats.roomDescription;
        needsCredits.text = roomStats.price.ToString();
        needsPower.text = roomStats.minPower.ToString();
        needsCrew.text = roomStats.minCrew.ToString() + "-" + roomStats.maxCrew.ToString();
        currentCrew.text = roomStats.currentCrew.ToString();
        roomSize.text = selectedRoom.GetComponent<ObjectScript>().shapeDataTemplate.roomSizeName;

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
