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
using TMPro;

public class RadioManager : MonoBehaviour
{
    private int currentStation = 0;
    public TextMeshProUGUI currentStationName;
    public TextMeshProUGUI currentStationDesc;
    public bool muted = false;

    string[] stations = new string[] {"POPZ", "SMTH", "CTRY", "ZERO", "NOIS",
    "LBTC"};
    string[] stationsDescs = new string[] {"Space Pop\nA simulation of popular music in the void of space.",
        "Star Jazz\nSmooth listening for all varieties of space-dwelling individuals.",
        "Void Country\nSomehow, the tenets of twang and common compositional ideas made their way out to the stars.",
        "Robot Jams\nRhythm-heavy music with heavier synthetic sounds and less melodic theming throughout.",
        "Alien Noises\nWeird, often overtly complex “music” that pushes the boundaries of conventional-sounding stations.",
    "Lo-Fi Beats to Transport Cargo to\nExactly what is sounds like. The phenomenon of Lo-Fi music focused on consistent rhythms and interesting patterns never went away."};

    private void Update()
    {
        currentStationName.text = stations[currentStation];
        currentStationDesc.text = stationsDescs[currentStation];
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
        AudioManager.instance.isMuted = !AudioManager.instance.isMuted;
    }
}
