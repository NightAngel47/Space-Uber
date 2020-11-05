/*
 * AstroidMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/21/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using TMPro;

public class AstroidMiniGame : MiniGame
{
    public GameObject damageText;
    public int requiredAstroids = 10;
    [SerializeField]TMP_Text scoreText = null;

	private void Update()
	{
		scoreText.text = "Astroids Remaining: " + requiredAstroids;
		if(requiredAstroids == 0) { EndMiniGameSuccess(1); }
	}
}