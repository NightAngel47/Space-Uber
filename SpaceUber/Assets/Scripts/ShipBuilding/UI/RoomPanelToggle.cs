/* Frank Calabrese
 * RoomPanelToggle.cs
 * various controls for the shipbuilding, radio, and crew management UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanelToggle : MonoBehaviour
{
    [SerializeField] private Animator shopAnimator;
    private bool isOpen;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    private void Start()
    {
        isOpen = false;
    }


    public void TogglePanelVis()
    {
        if (isOpen == false)
        {
            shopAnimator.SetBool(IsOpen, true);
            isOpen = true;
        }
        else
        {
            shopAnimator.SetBool(IsOpen, false);
            isOpen = false;
        }
    }

    public void OpenPanel()
    {
        if (isOpen == false)
        {
            shopAnimator.SetBool(IsOpen, true);
            isOpen = true;
        }
    }
}
