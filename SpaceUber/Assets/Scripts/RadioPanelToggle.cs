using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPanelToggle : MonoBehaviour
{
    public bool isVisible;
    private RectTransform location;

    private void Start()
    {
        location = GetComponent<RectTransform>();
        isVisible = false;
    }

    public void TogglePanelVis()
    {
        if(isVisible == true)
        {
            location.localPosition = new Vector3(location.localPosition.x + 90, location.localPosition.y, location.localPosition.z);
            isVisible = false;
        }
        else
        {
            location.localPosition = new Vector3(location.localPosition.x - 90, location.localPosition.y, location.localPosition.z);
            isVisible = true;
        }
    }
}
