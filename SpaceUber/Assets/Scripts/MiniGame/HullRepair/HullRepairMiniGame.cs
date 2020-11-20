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
	Collider2D collider;
	[SerializeField] int overLapCount = 0;

	private void Start()
	{
		collider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		Collider2D[] collider2Ds = new Collider2D[hullPieces.Length+1];
		if (!gameOver)
		{
			collider.OverlapCollider(new ContactFilter2D(), collider2Ds);
			foreach(Collider2D hullPiece in collider2Ds)
			{
				if(hullPiece)
				{
					if (hullPiece.GetComponent<HullPiece>() != null) { overLapCount++;  Debug.Log(""); }
					else { Debug.Log("not hull piece"); }
				}
				else { Debug.Log("not collider"); }
			}
			if(collider2Ds.Length == 0) { Debug.Log("No colliders"); }
			if ((overLapCount) == hullPieces.Length) { EndMiniGameSuccess(); }
		}
		overLapCount = 0;
	}


}