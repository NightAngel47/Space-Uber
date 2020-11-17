/*
 * HullRepairMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class HullRepairMiniGame : MiniGame
{
    public static HullPiece selectedHullPiece;
    [SerializeField] HullPiece[] hullPieces;
	[SerializeField] Collider2D collider;
	[SerializeField] int overLapCount;

	private void Update()
	{
		Collider2D[] collider2Ds = new Collider2D[hullPieces.Length+1];
		if (!gameOver)
		{
			if ((overLapCount = collider.OverlapCollider(new ContactFilter2D(), collider2Ds)) == hullPieces.Length+1) { EndMiniGameSuccess(); }
		}
	}


}