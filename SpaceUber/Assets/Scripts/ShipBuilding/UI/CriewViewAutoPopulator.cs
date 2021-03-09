/* Frank Calabrese
 * 3/9/21
 * CrewViewAutoPopulator.cs
 * adds amount of crewView slots to room's layout group based off the room's maxCrew. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriewViewAutoPopulator : MonoBehaviour
{
    [SerializeField] GameObject crewViewSlotPrefab;
    void Start()
    {
        
        for(int i = 0; i < gameObject.GetComponentInParent<RoomStats>().maxCrew; i++)
        {
            var newSlot = Instantiate(crewViewSlotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            newSlot.transform.parent = gameObject.transform;

            gameObject.GetComponentInParent<CrewView>().activateCrewSlot(i, newSlot.gameObject);
        }

        gameObject.GetComponentInParent<CrewView>().finishPopulatingCrewSlots();
    }
}
