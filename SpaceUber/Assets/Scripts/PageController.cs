/*
 * NextPage.cs
 * Author(s): Sam Ferstein
 * Created on: 12/2/2020 (en-US)
 * Description: This controls the page movement of the text boxes for events.
 */

using UnityEngine;
using TMPro;

public class PageController : MonoBehaviour
{
    public TextMeshProUGUI name = new TextMeshProUGUI();

    public void NextPage()
    {
        name.pageToDisplay += 1;
    }

    public void PreviousPage()
    {
        if(name.pageToDisplay != 1)
        {
            name.pageToDisplay -= 1;
        }
    }
}
