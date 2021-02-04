/* RadioManager.cs
 * Frank Calabrese 
 * 2/2/21
 * Contains methods used by the buttons of the radio 
 * to control volume and change stations (to be added)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioManager : MonoBehaviour
{
    private int currentStation = 0;
    public Text currentStationName;
    public bool muted = false;
    string[] stations = new string[] {"POPZ", "SMTH", "CTRY", "ZERO", "NOIS",
    "LBTC"};

    private void Update()
    {
        currentStationName.text = stations[currentStation];
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

    public void ChangeStation()
    {
        if (currentStation < stations.Length - 1) currentStation++;
        else currentStation = 0;
    }

    public void Mute()
    {
        if(muted == true)
        {
            MasterVolSlider(1f);
            muted = false;
        }
        else
        {
            MasterVolSlider(0f);
            muted = true;
        }
    }
}
