/* Frank Calabrese
 * 3/9/21
 * CrewViewAutoPopulator.cs
 * adds amount of crewView slots to room's layout group based off the room's maxCrew. 
 */

using UnityEngine;

public class CriewViewAutoPopulator : MonoBehaviour
{
    [SerializeField] GameObject crewViewSlotPrefab;

    private CrewView crewView;

    private void Awake()
    {
        crewView = GetComponentInParent<CrewView>();
    }

    void Start()
    {
        for(int i = 0; i < gameObject.GetComponentInParent<RoomStats>().maxCrew; i++)
        {
            GameObject newSlot = Instantiate(crewViewSlotPrefab, transform.position, Quaternion.identity, transform);
            crewView.ActivateCrewSlot(newSlot.gameObject);
        }

        crewView.FinishPopulatingCrewSlots();
    }
}
