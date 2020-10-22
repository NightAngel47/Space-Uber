/*
 * EnergyLevelStabalizeMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: 
 */

using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLevelStabalizeMiniGame : MonoBehaviour
{
	[SerializeField] Slider slider1;
	[SerializeField] Slider slider2;
	[SerializeField] Slider slider3;
	[SerializeField] MiniGameButton button1;
	[SerializeField] MiniGameButton button2;
	[SerializeField] MiniGameButton button3;
	[SerializeField] MiniGameButton switch1;
	[SerializeField] MiniGameButton switch2;
	[SerializeField] MiniGameButton switch3;
	[SerializeField] TMP_Text optimizationText;
	float slider1Target;
    float slider2Target;
    float slider3Target;
	int button1Target;
	int button2Target;
	int button3Target;
	int switch1Target;
	int switch2Target;
	int switch3Target;

	float optimization;

	private void Start()
	{
		slider1Target = Random.Range(0f, 1f);
		slider2Target = Random.Range(0f, 1f);
		slider3Target = Random.Range(0f, 1f);
		button1Target = Random.Range(0, 2);
		button2Target = Random.Range(0, 2); 
		button3Target = Random.Range(0, 2); 
		switch1Target = Random.Range(0, 2); 
		switch2Target = Random.Range(0, 2); 
		switch3Target = Random.Range(0, 2); 
	}

	private void Update()
	{
		float sliderOptimizationLevel1 = 1- (Mathf.Abs(slider1Target - slider1.value));
		float sliderOptimizationLevel2 = 1 - (Mathf.Abs(slider2Target - slider2.value));
		float sliderOptimizationLevel3 = 1 - (Mathf.Abs(slider3Target - slider3.value));
		float buttonOptimizationLevel1 = 1 - (Mathf.Abs(button1Target - button1.value));
		float buttonOptimizationLevel2 = 1 - (Mathf.Abs(button2Target - button2.value));
		float buttonOptimizationLevel3 = 1 - (Mathf.Abs(button3Target - button3.value));
		float switchOptimizationLevel1 = 1 - (Mathf.Abs(switch1Target - switch1.value));
		float switchOptimizationLevel2 = 1 - (Mathf.Abs(switch2Target - switch2.value));
		float switchOptimizationLevel3 = 1 - (Mathf.Abs(switch3Target - switch3.value));
		float total = sliderOptimizationLevel1 + sliderOptimizationLevel2 + sliderOptimizationLevel3 +
			buttonOptimizationLevel1 + buttonOptimizationLevel2 + buttonOptimizationLevel3 + 
			switchOptimizationLevel1 + switchOptimizationLevel2 + switchOptimizationLevel3;
		optimization = Mathf.RoundToInt(100f * (total / 9f));
		optimizationText.text = (optimization + "%");
	}
}