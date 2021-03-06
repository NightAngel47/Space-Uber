/* RadioManager.cs
 * Frank Calabrese 
 * 2/2/21
 * Contains methods used by the buttons of the radio 
 * to control volume and change stations
 */

using System;
using System.Collections;
using UnityEngine;
using TMPro;

[Serializable]
public struct RadioStation
{
    public string name;
    public string tooltipName;
    [TextArea] public string tooltipDesc;
}

public class RadioManager : MonoBehaviour
{
    private int currentStation = 0;
    [SerializeField] TMP_Text currentStationName;
    [SerializeField] TMP_Text currentStationDesc;

    [SerializeField] private RadioStation[] stations = new RadioStation[6];

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => AudioManager.instance != null);
        AudioManager.instance.PlayRadio(currentStation);
        UpdateRadioUIInfo();
    }

    private void UpdateRadioUIInfo()
    {
        currentStationName.text = stations[currentStation].name;
        currentStationDesc.text = stations[currentStation].tooltipName + '\n' + stations[currentStation].tooltipDesc;
    }

    public void RadioStationSlider(float station)
    {
        currentStation = (int)station;
        UpdateRadioUIInfo();
        AudioManager.instance.PlaySFX("Switch 1");
        AudioManager.instance.PlayRadio(currentStation);
    }

    public void MasterVolSlider(float volume)
    {
        AudioManager.instance.masterVolume = volume;
    }
    public void RadioVolSlider(float volume)
    {
        AudioManager.instance.radioVolume = volume;
    }
    public void BGMVolSlider(float volume)
    {
        AudioManager.instance.musicVolume = volume;
    }
    public void SFXVolSlider(float volume)
    {
        AudioManager.instance.sfxVolume = volume;
    }

    public void Mute()
    {
        AudioManager.instance.isMuted = !AudioManager.instance.isMuted;
    }
}
