/*
 * MiniGameButton.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/6/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class MiniGameButton : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;
	[HideInInspector] public int value = 0;
	[SerializeField] Sprite onSprite = null;
	[SerializeField] Sprite offSprite = null;

	public void OnMouseDown() { ChangeValue(); }

	public void ChangeValue()
	{
		isOn = !isOn;
		if (isOn) { GetComponent<Image>().sprite = onSprite; value = 1; }
		else { GetComponent<Image>().sprite = offSprite; value = 0; }
	}
}