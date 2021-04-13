/* Frank Calabrese
 * RadioDial.cs
 * 3/24/21
 * controls the dials on the radio
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RadioDial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] Image myImage;
    [SerializeField] RectTransform rotator;
    [SerializeField] private DialType dial;
    
    enum DialType
    {
        Station,
        Master,
        Radio,
        BGM,
        SFX
    };
    
    private bool isMouseOverObject;
    private bool locked = false;
    private RadioManager radioManager;
    private AudioSettings audioSettings;

    private float value;

    private void Awake()
    {
        radioManager = GetComponentInParent<RadioManager>();
        audioSettings = FindObjectOfType<AudioSettings>();

        if (dial == DialType.Station)//SavingLoadingManager.instance.GetHasSave() 
        {
            LoadRadioSettings();
        }
        else SetAudioSettingsValues();
    }
    
    private void OnEnable()
    {
        audioSettings = FindObjectOfType<AudioSettings>();

        if (dial == DialType.Station)//SavingLoadingManager.instance.GetHasSave() 
        {
            LoadRadioSettings();
        }
        else SetAudioSettingsValues();
    }
    
    private void OnDisable()
    {
        SendAudioSettingsValues();
        if (dial == DialType.Station) SaveRadioSettings();
    }
    private void OnDestroy()
    {
        SendAudioSettingsValues();
        if (dial == DialType.Station) SaveRadioSettings();
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


            /*if within 3 degrees of 0. Lock dial, set volume to min or max, return dial to unlocked position, return
            if (!stationDial && rotator.rotation.eulerAngles.z < 3)
            {
                locked = true;
                rotator.transform.Rotate(0f, 0f, 3);
                isMouseOverObject = false;
                if (!stationDial) myImage.fillAmount = slider.value;
                slider.value = 100; return;
            }
            else if (!stationDial && rotator.rotation.eulerAngles.z > 357)
            {
                locked = true;
                rotator.transform.Rotate(0f, 0f, -3);
                isMouseOverObject = false;
                if (!stationDial) myImage.fillAmount = slider.value;
                slider.value = 0; return;
            }*/

            //set slider values based on rotation
            if (dial == DialType.Station) value = (Mathf.Abs(rotator.rotation.z) / 360f) * 100f * 20f;
            else value = 1 - (rotator.rotation.eulerAngles.z / 360);

            UpdateRadioManager();
            
            //fill volume bar based on slider value
            if (dial != DialType.Station) myImage.fillAmount = value;
        }

        //if player lets go outside of hitbox, let go
        if(Input.GetMouseButtonUp(0)) isMouseOverObject = false;

        if(Input.GetKeyDown(KeyCode.Escape)) SendAudioSettingsValues();
    }

    private void UpdateRadioManager()
    {
        switch (dial)
        {
            case DialType.Station:
                Debug.LogError("blah");
                radioManager.RadioStationSlider(value);
                return;
            case DialType.Master:
                radioManager.MasterVolSlider(value);
                break;
            case DialType.Radio:
                radioManager.RadioVolSlider(value);
                break;
            case DialType.BGM:
                radioManager.BGMVolSlider(value);
                break;
            case DialType.SFX:
                radioManager.SFXVolSlider(value);
                break;
            default:
                Debug.LogError("Radio Dial Type Not Set");
                break;
        }
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

    private void SaveRadioSettings()
    {
        SavingLoadingManager.instance.Save<float>("sliderVal", value);
    }

    private void LoadRadioSettings()
    {
        value = SavingLoadingManager.instance.Load<float>("sliderVal");
        
        UpdateRadioManager();
    }

    public void SetAudioSettingsValues()
    {
        switch (dial)
        {
            case DialType.Station:
                return;
            case DialType.Master:
                value = AudioSettings.masterVol;
                break;
            case DialType.Radio:
                value = AudioSettings.radioVol;
                break;
            case DialType.BGM:
                value = AudioSettings.bgmVol;
                break;
            case DialType.SFX:
                value = AudioSettings.sfxVol;
                break;
            default:
                Debug.LogError("Radio Dial Type Not Set");
                break;
        }
        
        UpdateRadioManager();

        rotator.transform.rotation = Quaternion.identity;
        rotator.transform.Rotate(0f, 0f, -(value * 360f));
        if (dial != DialType.Station) myImage.fillAmount = value;
    }

    private void SendAudioSettingsValues()
    {
        switch (dial)
        {
            case DialType.Station:
                return;
            case DialType.Master:
                AudioSettings.masterVol = value;
                break;
            case DialType.Radio:
                AudioSettings.radioVol = value;
                break;
            case DialType.BGM:
                AudioSettings.bgmVol = value;
                break;
            case DialType.SFX:
                AudioSettings.sfxVol = value;
                break;
            default:
                Debug.LogError("Radio Dial Type Not Set");
                break;
        }
        
        audioSettings?.SaveAudioSettings();
    }
}