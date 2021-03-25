using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RadioDial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image myImage;
    [SerializeField] RectTransform rotator;
    [SerializeField] Slider slider;
    [SerializeField] bool stationDial;
    private bool isMouseOverObject;
    

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isMouseOverObject)
        {
            Vector3 difference = rotator.transform.InverseTransformPoint(Input.mousePosition);
            var angle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            rotator.transform.Rotate(0f, 0f, -angle);
        }

        if (stationDial) slider.value = (Mathf.Abs(rotator.rotation.z) / 360f) * 100f * 20f;
        else slider.value = (Mathf.Abs(rotator.rotation.z)/360f) * 100f * 4f;

        if(!stationDial)myImage.fillAmount = slider.value;
        
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverObject = false;
    }

}