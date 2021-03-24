using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadioDial : MonoBehaviour
{
    private Image myImage;
    [SerializeField] RectTransform rotator;
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        myImage.fillAmount = slider.value;
        rotator.rotation = Quaternion.Euler(0f, 0f, - slider.value * 360);
    }
}
