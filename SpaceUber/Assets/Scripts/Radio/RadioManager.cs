/* RadioManager.cs
 * Frank Calabrese 
 * 2/2/21
 * Contains methods used by the buttons of the radio 
 * to control volume and change stations
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioManager : MonoBehaviour
{
    private int currentStation = 0;
    [SerializeField] TextMeshProUGUI currentStationName;
    [SerializeField] TextMeshProUGUI currentStationDesc;

    [SerializeField] string[] stations = new string[] { };
    //set in code because there's no newline in Unity inspector
    [SerializeField] string[] stationsDescs = new string[] {"Star Jazz\nSmooth listening for all varieties of space-dwelling individuals.",
        "Space Pop\nA simulation of popular music in the void of space.",
        "Void Country\nSomehow, the tenets of twang and common compositional ideas made their way out to the stars.",
        "Robot Jams\nRhythm-heavy music with heavier synthetic sounds and less melodic theming throughout.",
        "Alien Noises\nWeird, often overtly complex “music” that pushes the boundaries of conventional-sounding stations.",
    "Lo-Fi Beats to Transport Cargo to\nExactly what is sounds like. The phenomenon of Lo-Fi music focused on consistent rhythms and interesting patterns never went away."};

    private void Start()
    {
        AudioManager.instance.PlayRadio(0);
    }
    private void Update()
    {

        currentStationName.text = stations[currentStation];
        currentStationDesc.text = stationsDescs[currentStation];
    }

    public void RadioStationSlider(float station)
    {
        currentStation = (int)station;
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
