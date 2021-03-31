/*
 * ClickSound.cs
 * Author(s): Jake Hyland []
 * Created on: 11/8/2020 (en-US)
 * Description: Basic script for assigning SFX to objects that are clicked-on by the player throughout the game.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    public string[] Clicks;
    bool playingSound = false; 


    public void SlowClickPlay()
    {
        if (playingSound == false)
        {
            AudioManager.instance.PlaySFX(Clicks[Random.Range(0, Clicks.Length - 1)]);
            ClickPause();
        }
    }

    IEnumerator ClickPause()
    {
        playingSound = true;
        yield return new WaitForSeconds(0.25f);
        playingSound = false;
    }

    public void ClickPlay()
    {
            AudioManager.instance.PlaySFX(Clicks[Random.Range(0, Clicks.Length - 1)]);
    }
}
