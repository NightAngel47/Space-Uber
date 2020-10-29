/*
 * WormholeDriver.cs
 * Author(s): Scott Acker
 * Created on: 10/21/2020 (en-US)
 * Description: Specific driver for the Wormhole event. Allows for the outcome of this event to be random.
 */

using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class WormholeDriver : InkDriverBase
{

    protected override void RandomizeEnding()
    {
        int rng = Random.Range(1, 5);
        switch (rng)
        {
            case 1:
                story.variablesState["randomEnd"] = story.variablesState["minorDamageEnd"];
                break;
            case 2:
                story.variablesState["randomEnd"] = story.variablesState["prankedEnd"];
                break;
            case 3:
                story.variablesState["randomEnd"] = story.variablesState["moneyEnd"];
                break;
            case 4:
                story.variablesState["randomEnd"] = story.variablesState["shuffleEnd"];
                break;
        }
    }
}
