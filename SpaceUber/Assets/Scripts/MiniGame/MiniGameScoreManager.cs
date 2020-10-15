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
    public static MiniGameTool selectedTool;
    [SerializeField] MiniGameCrop[] crops;
    [SerializeField] TMP_Text scoreText;
    int score = 0;

    void Update()
    {
        scoreText.text = "Crops Collected: " + score;
    }

    public void IncrementScore() { score++; }
}