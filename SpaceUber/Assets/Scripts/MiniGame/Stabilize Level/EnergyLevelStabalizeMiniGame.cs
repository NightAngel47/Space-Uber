/*
 * EnergyLevelStabalizeMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */


using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLevelStabalizeMiniGame : MiniGame
{
	[SerializeField] Slider[] sliders = null;
	[SerializeField] MiniGameButton[] buttonSwitches = null;
	[SerializeField] TMP_Text optimizationText = null;
	[SerializeField] GameObject[] powerBarIndicators = null;
	float[] sliderTargets = null;
	int[] buttonSwitchTargets = null;


	private void Start()
	{
		sliderTargets = new float[sliders.Length];
		buttonSwitchTargets = new int[buttonSwitches.Length];
		RandomizeTargets();
	}

	private void Update()
	{
		List<float> optimizationLevels = new List<float>();
		for (int i = 0; i < sliders.Length; i++) 
		{
			//Adjust slider values to only have 1 of 5 values [0.2, 0.4, 0.6, 0.8, 1]
			float num = 1 - (Mathf.Abs(sliderTargets[i] - sliders[i].value));
			float roundedNum = num*100;
			roundedNum = Mathf.RoundToInt((roundedNum / 20f)+0.49f);
			optimizationLevels.Add(roundedNum/5f); 
		}
		for (int i = 0; i < buttonSwitches.Length; i++) { optimizationLevels.Add(1 - (Mathf.Abs(buttonSwitchTargets[i] - buttonSwitches[i].value))); }
		int total = 0;
		//add the rounded 
		foreach(float optimizationLevel in optimizationLevels) { total += Mathf.RoundToInt(100f * (optimizationLevel / (buttonSwitches.Length + sliders.Length))); }

		//Make indicator number the percent of indicators active based on total
		int indicatorNumber = (total / (100/powerBarIndicators.Length));

		for(int i = 0; i < powerBarIndicators.Length; i++) { powerBarIndicators[i].SetActive(i < indicatorNumber); }

		total += 50;
		optimizationText.text = (total + "%");
		if(total == 100) { EndMiniGameSuccess(); }
	}

	void RandomizeTargets()
	{
		for (int i = 0; i < sliders.Length; i++) { sliderTargets[i] = Random.Range(0f, 1f); }
		for (int i = 0; i < buttonSwitches.Length; i++) { buttonSwitchTargets[i] = Random.Range(0, 2); }
	}
}