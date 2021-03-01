/* Frank Calabrese
 * WhyAmIDisabled.cs
 * for use in debugging to find out why an object is
 * being disabled. Quite situational.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhyAmIDisabled : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.LogError("Someone disabled me. Who could it be?");
    }
}
