/*
 * SecurityMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/21/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using TMPro;
using System.Collections;

public class SecurityMiniGame : MiniGame
{
    [SerializeField] TMP_Text[] codeSegments;
    [SerializeField] TMP_InputField input;
    [SerializeField] float displayTime = 1;
    [SerializeField] GameObject tryAgainText;
    string validChars = "abcdefghijklmnopqrstuvwsyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    string requiredCode;

    void Start()
    {
        GenerateCode();
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if(input.text == requiredCode) { EndMiniGameSuccess(); }
            else 
            {
                input.text = ""; GenerateCode();
                StartCoroutine(PromptTryAgain());
            }
        }
    }

    void GenerateCode()
	{
        requiredCode = "";
        foreach (TMP_Text codeSegment in codeSegments)
        {
            string code = validChars[Random.Range(0, validChars.Length)].ToString();
            codeSegment.text = code;
            requiredCode += code;
            StartCoroutine(DisplayCode());
        }
    }

    IEnumerator PromptTryAgain()
	{
        tryAgainText.SetActive(true);
        yield return new WaitForSeconds(3);
        tryAgainText.SetActive(false);
	}

    IEnumerator DisplayCode()
	{
        foreach(TMP_Text codeSegment in codeSegments)
		{
            codeSegment.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTime);
            codeSegment.gameObject.SetActive(false);
		}
	}
}