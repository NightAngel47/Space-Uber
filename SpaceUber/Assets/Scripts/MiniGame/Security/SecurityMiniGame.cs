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
    [SerializeField] CodeBlock[] codeSegments;
    [SerializeField] TMP_Text codePreview;
    [SerializeField] TMP_Text requiredSuccessesText;
    [SerializeField] float displayTime = 1;
    [SerializeField] GameObject tryAgainText;
    [SerializeField] int requiredSuccesses = 3;
    [SerializeField] int minCodeLength = 3;
    [SerializeField] int maxCodeLength = 5;
    int successes = 0;
    string validChars = "abcdefghijklmnopqrstuvwsyz1234567890";
    string requiredCode = "";
    string availableCode = "";
    string inputCode = "";

    void Start() { GenerateCode(); }

    void Update()
    {
        requiredSuccessesText.text = "Required Successes: " + (requiredSuccesses - successes);
		if (inputCode.Length == requiredCode.Length) 
        {
            if (inputCode == requiredCode)
            {
                successes++;
                if (successes == requiredSuccesses) { EndMiniGameSuccess(); }
                else 
                {
                    inputCode = "";
                    GenerateCode();
                }
            }
            else 
            {
                inputCode = ""; 
                StartCoroutine(PromptTryAgain());
            }
        }
    }

    void ScrambleCodeBlocks()
    {
        for (int i = 0; i < codeSegments.Length - 1; i++)
        {
            CodeBlock block = codeSegments[i];
            int randomIndex = Random.Range(i + 1, codeSegments.Length);
            codeSegments[i] = codeSegments[randomIndex];
            codeSegments[randomIndex] = block;
        }
    }

    void GenerateCode()
    {
        ScrambleCodeBlocks();
        requiredCode = "";
        availableCode = "";
        foreach (CodeBlock codeSegment in codeSegments)
        {
            string code = validChars[Random.Range(0, validChars.Length)].ToString();
            while (availableCode.Contains(code)) { code = validChars[Random.Range(0, validChars.Length)].ToString(); }
            availableCode += code;
            codeSegment.codeText.text = code;
        }
        int codeLenght = Random.Range(minCodeLength, maxCodeLength+1);
        for(int i = 0; i < codeLenght; i++)
		{
            string code = availableCode[Random.Range(0, availableCode.Length)].ToString();
            while (requiredCode.Contains(code)) { code = availableCode[Random.Range(0, availableCode.Length)].ToString(); }
            requiredCode += code;
        }
        StartCoroutine(DisplayCode());
    }

    public void InputCode(string newCode) { inputCode += newCode; }

    IEnumerator PromptTryAgain()
	{
        tryAgainText.SetActive(true);
        yield return new WaitForSeconds(3);
        GenerateCode();
        tryAgainText.SetActive(false);
	}

    IEnumerator DisplayCode()
	{
        codePreview.text = "";
        yield return new WaitForSeconds(1);
        foreach(char codeSegment in requiredCode)
		{
            codePreview.text = codeSegment.ToString();
            yield return new WaitForSeconds(displayTime);
            codePreview.text = "";
        }
	}
}