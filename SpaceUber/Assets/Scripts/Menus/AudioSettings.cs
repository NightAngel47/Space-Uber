using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    static float masterVol = 1;
    static float sfxVol = 1;
    static float bgmVol = 1;
    static float radioVol = 0;
    static float ambientVol = 1;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider radioSlider;
    [SerializeField] Slider ambientSlider;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => AudioManager.instance != null);

        if (SavingLoadingManager.instance.GetHasSave())
        {
            LoadAudioSettings();
        }
        else //
        {
            //masterVol = 1;
            //sfxVol = 1;
            //bgmVol = 1;
            //radioVol = 0;
            //ambientVol = 1;

            //SaveAudioSettings();
        }

        masterSlider.value = masterVol;
        sfxSlider.value = sfxVol;
        bgmSlider.value = bgmVol;
        radioSlider.value = radioVol;
        ambientSlider.value = ambientVol;
    }
    private void OnEnable()
    {
        masterSlider.value = masterVol;
        sfxSlider.value = sfxVol;
        bgmSlider.value = bgmVol;
        radioSlider.value = radioVol;
        ambientSlider.value = ambientVol;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void MasterVolSlider(float volume)
    {
        AudioManager.instance.masterVolume = volume;
        masterVol = volume;
        SaveAudioSettings();
    }
    public void RadioVolSlider(float volume)
    {
        AudioManager.instance.radioVolume = volume;
        radioVol = volume;
        SaveAudioSettings();
    }
    public void BGMVolSlider(float volume)
    {
        AudioManager.instance.musicVolume = volume;
        bgmVol = volume;
        SaveAudioSettings();
    }
    public void SFXVolSlider(float volume)
    {
        AudioManager.instance.sfxVolume = volume;
        sfxVol = volume;
        SaveAudioSettings();
    }
    public void AmbientVolSlider(float volume)
    {
        AudioManager.instance.ambienceVolume = volume;
        ambientVol = volume;
        SaveAudioSettings();
    }

    public void SaveAudioSettings()
    {
        SavingLoadingManager.instance.Save<float>("masterVol", masterVol);
        SavingLoadingManager.instance.Save<float>("sfxVol", sfxVol);
        SavingLoadingManager.instance.Save<float>("bgmVol", bgmVol);
        SavingLoadingManager.instance.Save<float>("radioVol", radioVol);
        SavingLoadingManager.instance.Save<float>("ambientVol", ambientVol);
    }
    public void LoadAudioSettings()
    {
        masterVol = SavingLoadingManager.instance.Load<float>("masterVol");
        sfxVol = SavingLoadingManager.instance.Load<float>("sfxVol");
        bgmVol = SavingLoadingManager.instance.Load<float>("bgmVol");
        radioVol = SavingLoadingManager.instance.Load<float>("radioVol");
        ambientVol = SavingLoadingManager.instance.Load<float>("ambientVol");
    }
}
