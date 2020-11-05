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
	[SerializeField] protected float statModification = 1;
	[SerializeField] GameObject gameWinScreen;

	private void Start()
	{
		gameWinScreen.SetActive(false);
	}

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false);
	}

    public void EndMiniGameSuccess()
	{
		gameWinScreen.SetActive(true);
    }

	public void ConfirmMiniGameSuccess()
	{
		gameWinScreen.SetActive(false);
		OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);
	}
}