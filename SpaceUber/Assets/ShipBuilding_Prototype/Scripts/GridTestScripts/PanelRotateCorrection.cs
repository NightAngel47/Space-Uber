/*
 * PanelRotateCorrection.cs
 * Author(s): Sydney Foe []
 * Created on: 10/6/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class PanelRotateCorrection : MonoBehaviour
{
    Vector3 pos;
    void Awake()
    {
        pos = transform.position;
    }

    void LateUpdate()
    {
        transform.position = pos;
        transform.rotation = Quaternion.identity;
    }
}
