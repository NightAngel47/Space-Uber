/* Frank Calabrese
 * RadioDial.cs
 * 3/24/21
 * controls the dials on the radio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RadioDial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Image myImage;
    [SerializeField] RectTransform rotator;
    [SerializeField] Slider slider;
    [SerializeField] bool stationDial;
    [SerializeField] bool masterDial;
    [SerializeField] bool radioDial;
    [SerializeField] bool bgmDial;
    [SerializeField] bool sfxDial;
    private bool isMouseOverObject;
    private bool locked = false;
    private AudioSettings audioSettings;
    private Quaternion defaultPosition = new Quaternion(0f,0f,0f,0f);

    

    
    void Start()
    {
        if(stationDial)//SavingLoadingManager.instance.GetHasSave() 
        {
            LoadRadioSettings();
        }
        //else if(stationDial)
        //{
            //slider.value = 0;
            //SaveRadioSettings();
        //}


        audioSettings = FindObjectOfType<AudioSettings>();
        SetAudioSettingsValues();


        myImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        if (stationDial)//SavingLoadingManager.instance.GetHasSave() 
        {
            LoadRadioSettings();
        }
        //else if(stationDial)
        //{
        //slider.value = 0;
        //SaveRadioSettings();
        //}


        audioSettings = FindObjectOfType<AudioSettings>();
        SetAudioSettingsValues();
    }
    private void OnDisable()
    {
        if (stationDial) SaveRadioSettings();
    }
    private void OnDestroy()
    {
        if (stationDial) SaveRadioSettings();
    }

    void Update()
    {
        //ensure dial is unlocked
        if (rotator.rotation.eulerAngles.z > 3 && rotator.rotation.eulerAngles.z < 357) locked = false;

        //player click/drag
        if (Input.GetMouseButton(0) && isMouseOverObject && !locked)
        {
            //dial sprite turns towards mouse position
            Vector3 difference = rotator.transform.InverseTransformPoint(Input.mousePosition);
            var angle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            rotator.transform.Rotate(0f, 0f, -angle);


            //if within 3 degrees of 0. Lock dial, set volume to min or max, return dial to unlocked position, return
            //if (!stationDial && rotator.rotation.eulerAngles.z < 3)
            //{
                //locked = true;
                //rotator.transform.Rotate(0f, 0f, 3);
                //isMouseOverObject = false;
                //if (!stationDial) myImage.fillAmount = slider.value;
                //slider.value = 100; return;
            //}
            //else if (!stationDial && rotator.rotation.eulerAngles.z > 357)
            //{
                //locked = true;
                //rotator.transform.Rotate(0f, 0f, -3);
                //isMouseOverObject = false;
                //if (!stationDial) myImage.fillAmount = slider.value;
                //slider.value = 0; return;
            //}

            //set slider values based on rotation
            if (stationDial) slider.value = (Mathf.Abs(rotator.rotation.z) / 360f) * 100f * 20f;
            else slider.value = 1 - (rotator.rotation.eulerAngles.z / 360);
            SendAudioSettingsValues();

        }
        //fill volume bar based on slider value
        if (!stationDial)myImage.fillAmount = slider.value;

        //if player lets go outside of hitbox, let go
        if(Input.GetMouseButtonUp(0)) isMouseOverObject = false;


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseOverObject = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!Input.GetMouseButton(0)) isMouseOverObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!Input.GetMouseButton(0)) isMouseOverObject = false;
    }

    public void SaveRadioSettings()
    {
        SavingLoadingManager.instance.Save<float>("sliderVal", slider.value);
    }
    public void LoadRadioSettings()
    {
        slider.value = SavingLoadingManager.instance.Load<float>("sliderVal");
    }

    public void SetAudioSettingsValues()
    {
        if (masterDial) slider.value = AudioSettings.masterVol;
        else if (radioDial) slider.value = AudioSettings.radioVol;
        else if (bgmDial) slider.value = AudioSettings.bgmVol;
        else if (sfxDial) slider.value = AudioSettings.sfxVol;
        rotator.transform.rotation = defaultPosition;
        rotator.transform.Rotate(0f, 0f, -(slider.value * 360f));
    }

    public void SendAudioSettingsValues()
    {
        if (masterDial) AudioSettings.masterVol = slider.value;
        else if (radioDial) AudioSettings.radioVol = slider.value;
        else if (bgmDial) AudioSettings.bgmVol = slider.value;
        else if (sfxDial) AudioSettings.sfxVol = slider.value;
    }
}