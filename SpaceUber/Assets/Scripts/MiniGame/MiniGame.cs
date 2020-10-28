/*
 * MiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Parent class of mini games
 */

using UnityEngine;

public class MiniGame : MonoBehaviour
{
    [SerializeField] string miniGameSceneName;

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false, 1);
	}

    public void EndMiniGameSuccess(float statModification)
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);
    }
}