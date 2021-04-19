/* Frank Calabrese
 * 4/13/21
 * MainMenu.cs
 * things in this script happen when the main menu is loaded
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => AudioManager.instance != null);
        AudioManager.instance.StopRadio();
        AudioManager.instance.PlayMusicWithTransition("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
