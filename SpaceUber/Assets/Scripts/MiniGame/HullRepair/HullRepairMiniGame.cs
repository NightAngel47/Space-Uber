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

	public GameObject[] smallHullPieces, largeHullPieces;
	public HullPieceSlot[] smallSlots, largeSlots;

	protected override void Start()
    {
		base.Start();
		//RandomizePositions();
    }

	private void RandomizePositions()
    {
		foreach(HullPieceSlot slot in smallSlots)
		{
			slot.taken = false;
        }
		foreach (HullPieceSlot slot in largeSlots)
		{
			slot.taken = false;
		}

		foreach(GameObject piece in smallHullPieces)
        {
			int randNum = Random.Range(0, smallSlots.Length);
			while(smallSlots[randNum].taken)
            {
				randNum = Random.Range(0, smallSlots.Length);
			}

			smallSlots[randNum].taken = true;
			piece.transform.position = smallSlots[randNum].myPosition;
        }

		foreach (GameObject piece in largeHullPieces)
		{
			int randNum = Random.Range(0, largeSlots.Length);
			while (largeSlots[randNum].taken)
			{
				randNum = Random.Range(0, largeSlots.Length);
			}

			largeSlots[randNum].taken = true;
			piece.transform.position = largeSlots[randNum].myPosition;
		}
	}



    private void Update()
	{
		int gridCoveredCount = 0;
		foreach (HullGridSquare gridSquare in gridSquares) 
		{ 
			if (gridSquare.IsCovered())
			{ 
				gridCoveredCount++; 
			} 
		}

		if (!gameOver)
        {
            if (gridCoveredCount == gridSquares.Length)
            {
                EndMiniGameSuccess();
                AudioManager.instance.PlaySFX("Fixed");
            }
        }
	}

	public Color CoveredColor() { return coveredColor; }
	public Color UncoveredColor() { return uncoveredColor; }
}