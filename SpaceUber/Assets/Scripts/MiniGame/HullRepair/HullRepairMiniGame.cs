/*
 * HullRepairMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: Manages hull repair mini game
 */

using UnityEngine;
using System.Collections.Generic;

public class HullRepairMiniGame : MiniGame
{
	public static HullPiece selectedHullPiece;
	[SerializeField] HullGridSquare[] gridSquares;
	[SerializeField] Color coveredColor = Color.green;
	[SerializeField] Color uncoveredColor = Color.red;

	[SerializeField]private GameObject smallPieces, largePieces, smallSlots, largeSlots;

	[SerializeField] private List<GameObject> smallPiecesList, largePiecesList;
	[SerializeField] private List<HullPieceSlot> smallSlotsList, largeSlotsList;

	protected override void Start()
    {
		base.Start();
        FillLists();
        RandomizePositions();
        Tutorial.Instance.SetCurrentTutorial(7, true);
	}

	private void FillLists()
    {
		smallPiecesList = new List<GameObject>();
		largePiecesList = new List<GameObject>();
		smallSlotsList = new List<HullPieceSlot>();
		largeSlotsList = new List<HullPieceSlot>();

		//generate array sizes
		int smallPieceCount = smallPieces.transform.childCount;
		int largePieceCount = largePieces.transform.childCount;
		int smallSlotCount = smallSlots.transform.childCount;
		int largeSlotCount = largeSlots.transform.childCount;

		//fill arrays
		for (int i = 0; i < smallPieceCount; i++)
        {
			smallPiecesList.Add(smallPieces.transform.GetChild(i).gameObject);
        }
		for (int i = 0; i < largePieceCount; i++)
		{
			largePiecesList.Add(largePieces.transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < smallSlotCount; i++)
		{
			smallSlotsList.Add(smallSlots.transform.GetChild(i).gameObject.GetComponent<HullPieceSlot>());
		}
		for (int i = 0; i < largeSlotCount; i++)
		{
			largeSlotsList.Add(largeSlots.transform.GetChild(i).gameObject.GetComponent<HullPieceSlot>());
		}
	}
	private void RandomizePositions()
    {
		//ensure they are "taken" at initialization
		foreach(HullPieceSlot slot in smallSlotsList)
		{
			slot.taken = false;
        }
		foreach (HullPieceSlot slot in largeSlotsList)
		{
			slot.taken = false;
		}

		foreach(GameObject piece in smallPiecesList)
        {
			int randNum = Random.Range(0, smallSlotsList.Count);
			
			while(smallSlotsList[randNum].taken)
            {
				randNum = Random.Range(0, smallSlotsList.Count);
			}

			smallSlotsList[randNum].taken = true;
			piece.transform.position = smallSlotsList[randNum].gameObject.transform.position;
			piece.GetComponent<HullPiece>().mySlot = smallSlotsList[randNum];

		}

		foreach (GameObject piece in largePiecesList)
		{
			int randNum = Random.Range(0, largeSlotsList.Count);

			while (largeSlotsList[randNum].taken)
			{
				randNum = Random.Range(0, largeSlotsList.Count);
			}

			largeSlotsList[randNum].taken = true;
			piece.transform.position = largeSlotsList[randNum].gameObject.transform.position;
			piece.GetComponent<HullPiece>().mySlot = largeSlotsList[randNum];
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