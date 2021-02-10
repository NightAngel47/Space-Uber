/*
 * BackgroundBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 2/9/2021 (en-US)
 * Description: Controls the space background made with shader.
 */

using System;
using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    private Material spaceMat;
    private const string SCROLL_ACTIVE = "SCROLLACTIVE";

    private void Awake()
    {
        spaceMat = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (spaceMat.IsKeywordEnabled(SCROLL_ACTIVE) && GameManager.instance.currentGameState != InGameStates.Events)
        {
            spaceMat.DisableKeyword(SCROLL_ACTIVE);
        }
        else if (!spaceMat.IsKeywordEnabled(SCROLL_ACTIVE) && GameManager.instance.currentGameState == InGameStates.Events)
        {
            spaceMat.EnableKeyword(SCROLL_ACTIVE);
        }
    }
}
