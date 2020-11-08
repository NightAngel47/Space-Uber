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
    public string[] Successes;

    private void Start()
	{
		gameWinScreen.SetActive(false);
	}

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false);
        AudioManager.instance.PlaySFX("De-Overclock");
    }

    public void EndMiniGameSuccess()
	{
		gameWinScreen.SetActive(true);
        AudioManager.instance.PlaySFX(Successes[Random.Range(0, Successes.Length - 1)]);
    }

	public void ConfirmMiniGameSuccess()
	{
		gameWinScreen.SetActive(false);
		OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);
	}
}