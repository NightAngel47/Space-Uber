/*
 * ArrayLayout.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Sets up the 2d array grid
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct rowData
    {
        //public bool[] row;
        public int[] row;
    }

    public rowData[] rows = new rowData[9];
}
