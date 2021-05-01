/*
 * QuotesBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 5/1/2021 (en-US)
 * Description: 
 */

using System;
using TMPro;
using UnityEngine;

public class QuotesBehaviour : MonoBehaviour
{
    [SerializeField, Tooltip("List of quotes that can be displayed.")]
    private string[] quotes = new string[10];

    private TMP_Text quoteText;
    private static int previousQuoteIndex = 0;
    
    private void Awake()
    {
        quoteText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        SetQuoteText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SetQuoteText();
        }
    }

    private int GetNewQuoteIndex()
    {
        int index = UnityEngine.Random.Range(0, quotes.Length);
        if (index == previousQuoteIndex && quotes.Length > 2)
        {
            return GetNewQuoteIndex();
        }
        previousQuoteIndex = index;
        return index;
    }

    private void SetQuoteText()
    {
        quoteText.text = $"\"{quotes[GetNewQuoteIndex()]}\"";
    }
}
