/* Frank Calabrese
 * 3/11/21
 * RoomHighlight.cs
 * script to highlight selected room. Works with CrewManagementDetailsMenu.cs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHighlight : MonoBehaviour
{
    [SerializeField] GameObject highlightSprite;
    [SerializeField] GameObject crewViewSprite;

    public void Highlight()
    {
        highlightSprite.SetActive(true);
        crewViewSprite.SetActive(false);
    }
    public void Unhighlight()
    {
        highlightSprite.SetActive(false);
        crewViewSprite.SetActive(true);
    }

    public void ToggleHighlight()
    {
        if(highlightSprite.activeSelf == true)
        {
            highlightSprite.SetActive(false);
            crewViewSprite.SetActive(true);
        }
        else
        {
            highlightSprite.SetActive(true);
            crewViewSprite.SetActive(false);
        }
    }
}
