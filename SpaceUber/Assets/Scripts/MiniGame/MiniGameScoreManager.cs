/*
 * MiniGameScoreManager.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using TMPro;

public class MiniGameScoreManager : MonoBehaviour
{
    public static MiniGameTool selectedTool = null;
    [SerializeField] MiniGameCrop[] crops = null;
    [SerializeField] TMP_Text scoreText = null;
    int score = 0;

    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void IncrementScore() { score++; }
}