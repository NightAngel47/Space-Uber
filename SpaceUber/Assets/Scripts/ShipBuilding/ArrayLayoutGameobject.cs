using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrayLayoutGameobject
{ 
    [System.Serializable]
    public struct rowData
    {
        public GameObject[] row;
    }

    public rowData[] rows = new rowData[10];
}
