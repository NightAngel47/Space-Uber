/*
 * PanelRotateCorrection.cs
 * Author(s): Sydney Foe []
 * Created on: 10/6/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class PanelRotateCorrection : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
