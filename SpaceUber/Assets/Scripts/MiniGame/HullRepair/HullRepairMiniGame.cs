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

	private void Update()
	{
		int gridCoveredCount = 0;
		foreach(HullGridSquare gridSquare in gridSquares) { if (gridSquare.IsCovered()){ gridCoveredCount++; }}
		if (!gameOver) { if (gridCoveredCount == gridSquares.Length ) { EndMiniGameSuccess(); } }
	}
}