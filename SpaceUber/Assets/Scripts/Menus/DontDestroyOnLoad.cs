/* DontDestroyOnLoad.cs
 * Frank Calabrese 
 * 2/1/21
 * A gameObject containing this script will be brought into any new scene
 * instead of being destroyed
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
