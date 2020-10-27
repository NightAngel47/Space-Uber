/*
 * CodeBlock.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/22/2020 (en-US)
 * Description: 
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
	public TMP_Text codeText;
	SecurityMiniGame miniGameManager;

	private void Start()
	{
		miniGameManager = FindObjectOfType<SecurityMiniGame>();
	}

	public void InputCode()
	{
		miniGameManager.InputCode(codeText.text);
		GetComponent<Image>().color = miniGameManager.GetHighlightColor();
	}
}