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
	[SerializeField] TMP_Text optimizationText;
	float target1;
    float target2;
    float target3;
	float optimization;

	private void Start()
	{
		target1 = Random.Range(0f, 1f);
		target2 = Random.Range(0f, 1f);
		target3 = Random.Range(0f, 1f);
	}

	private void Update()
	{
		float optimizationLevel1 = 1- (Mathf.Abs(target1 - slider1.value));
		float optimizationLevel2 = 1 - (Mathf.Abs(target2 - slider2.value));
		float optimizationLevel3 = 1 - (Mathf.Abs(target3 - slider3.value));
		float total = optimizationLevel1 + optimizationLevel2 + optimizationLevel3;
		optimization = Mathf.RoundToInt(100 * (total / 3f));
		optimizationText.text = (optimization + "%");
	}
}