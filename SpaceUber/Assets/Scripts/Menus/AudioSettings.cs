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
    }
    public void RadioVolSlider(float volume)
    {
        AudioManager.instance.radioVolume = volume;
        radioVol = volume;
    }
    public void BGMVolSlider(float volume)
    {
        AudioManager.instance.musicVolume = volume;
        bgmVol = volume;
    }
    public void SFXVolSlider(float volume)
    {
        AudioManager.instance.sfxVolume = volume;
        sfxVol = volume;
    }
    public void AmbientVolSlider(float volume)
    {
        AudioManager.instance.ambienceVolume = volume;
        ambientVol = volume;
    }
}
