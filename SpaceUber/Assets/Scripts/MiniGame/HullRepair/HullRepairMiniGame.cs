/*
 * HullRepairMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: Manages hull repair mini game
 */

using UnityEngine;

public class HullRepairMiniGame : MiniGame
{
	public static HullPiece selectedHullPiece;
	[SerializeField] HullGridSquare[] gridSquares;
	[SerializeField] Color coveredColor = Color.green;
	[SerializeField] Color uncoveredColor = Color.red;
    [Tooltip("Percent to increase the frequency of an event showing up after finishing a minigame.")]
    public float percentIncrease = 5;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
	{
		int gridCoveredCount = 0;
		foreach (HullGridSquare gridSquare in gridSquares) { if (gridSquare.IsCovered()) { gridCoveredCount++; } }
		if (!gameOver)
        {
            if (gridCoveredCount == gridSquares.Length)
            {
                eventSystem.chanceOfEvent += percentIncrease;
                EndMiniGameSuccess();
                AudioManager.instance.PlaySFX("Fixed");
            }
        }
	}

	public Color CoveredColor() { return coveredColor; }
	public Color UncoveredColor() { return uncoveredColor; }
}