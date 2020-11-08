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
    bool winSound = false;

    private void Start()
	{
		gameWinScreen.SetActive(false);
        winSound = false;
	}

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false);
        AudioManager.instance.PlaySFX("De-Overclock");
    }

    public void EndMiniGameSuccess()
	{
		gameWinScreen.SetActive(true);

        if (winSound == false)
        {
            AudioManager.instance.PlaySFX(Successes[Random.Range(0, Successes.Length - 1)]);
            winSound = true;
        }
    }

	public void ConfirmMiniGameSuccess()
	{
		gameWinScreen.SetActive(false);
		OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);
	}
}