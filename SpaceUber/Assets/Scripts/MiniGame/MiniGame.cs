/*
 * MiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Parent class of mini games
 */

using UnityEngine;

public class MiniGame : MonoBehaviour
{
	[SerializeField] MiniGameType miniGameSceneName;

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false);
	}

    public void EndMiniGameSuccess(float statModification)
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);
    }
}