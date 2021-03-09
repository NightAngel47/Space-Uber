/* Frank Calabrese
 * 3/7/21
 * TutorialButton.cs
 * script for help buttons throughout the game. 
 * set relevantTutorial to match the ID of the tutorial you want to show.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    //check Tutorial object inspector to see what ints correspond to which tutorials
    [SerializeField] int relevantTutorial;

    public void beginTutorial()
    {
        Tutorial.Instance.setCurrentTutorial(relevantTutorial, false);
     }
}