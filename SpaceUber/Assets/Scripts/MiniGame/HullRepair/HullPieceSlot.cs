/*
 * HullPieceSlot.cs
 * Author(s): 
 * Created on: 3/28/2021 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullPieceSlot : MonoBehaviour
{
    [HideInInspector] public HullPiece myPiece;
    [HideInInspector] public bool taken = false;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
