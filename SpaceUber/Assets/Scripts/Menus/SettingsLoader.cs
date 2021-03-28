/* Frank Calabrese
 * SetDeactive.cs
 * load settings that start deactivated
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject audioSettings;
    private bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        optionsMenu.SetActive(true);
        audioSettings.SetActive(true);
        
    }
    private void Update()
    {
        if(!done)
        {
           // audioSettings.SetActive(false);
           // optionsMenu.SetActive(false);
            done = true;
        }
    }


}
