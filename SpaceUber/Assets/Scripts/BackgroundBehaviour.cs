/*
 * BackgroundBehaviour.cs
 * Author(s): Steven Drovie []
 * Created on: 2/9/2021 (en-US)
 * Description: 
 */

using System;
using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    [SerializeField] private Material[] scrollingBackgroundMats = new Material[3];

    private const string ScrollActive = "SCROLLACTIVE";

    private void Start()
    {
        ToggleScroll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleScroll();
        }
    }

    void ToggleScroll()
    {
        foreach (Material bgMat in scrollingBackgroundMats)
        {
            if (bgMat.IsKeywordEnabled(ScrollActive))
            {
                bgMat.DisableKeyword(ScrollActive);
            }
            else
            {
                bgMat.EnableKeyword(ScrollActive);
            }
        }
    }
}
