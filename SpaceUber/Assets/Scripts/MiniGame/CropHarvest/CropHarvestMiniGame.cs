/*
 * CropHarvestMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using TMPro;

public class CropHarvestMiniGame : MiniGame
{
    public static MiniGameTool selectedTool = null;
    [SerializeField] MiniGameCrop[] crops = null;
    int requiredScore;
    int score = 0;
    bool gameOver = false;

	void Update()
    {
        if(score == requiredScore&&!gameOver) { gameOver = true; EndMiniGameSuccess();  }
    }

    public void IncrementScore() { score++; }
    public void IncrementRequiredScore() { requiredScore++; }
}