/*
 * EnergyLevelStabalizeMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using Boo.Lang;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLevelStabalizeMiniGame : MonoBehaviour
{
	[SerializeField] Slider[] sliders = null;
	[SerializeField] MiniGameButton[] buttonSwitches = null;
	[SerializeField] TMP_Text optimizationText = null;
	[SerializeField] GameObject[] powerBarIndicators = null;
	[Tooltip("This is the percentage added to the total optimization")]
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
		for (int i = 0; i < sliders.Length; i++) { optimizationLevels.Add(1.2f - (Mathf.Abs(sliderTargets[i] - sliders[i].value))); }
		for (int i = 0; i < buttonSwitches.Length; i++) { optimizationLevels.Add(1 - (Mathf.Abs(buttonSwitchTargets[i] - buttonSwitches[i].value))); }
		int total = 0;
		//add the rounded 
		foreach(float optimizationLevel in optimizationLevels) { total += Mathf.RoundToInt(100f * (optimizationLevel / (buttonSwitches.Length + sliders.Length))); }

		//Make indicator number the percent of indicators active based on total
		int indicatorNumber = (total / (100/powerBarIndicators.Length));

		for(int i = 0; i < powerBarIndicators.Length; i++) { powerBarIndicators[i].SetActive(i < indicatorNumber); }

		if(total > 100) { total = 100; }
		optimizationText.text = (total + "%");
	}

	void RandomizeTargets()
	{
		for (int i = 0; i < sliders.Length; i++) { sliderTargets[i] = Random.Range(0f, 1f); }
		for (int i = 0; i < buttonSwitches.Length; i++) { buttonSwitchTargets[i] = Random.Range(0, 2); }
	}
}