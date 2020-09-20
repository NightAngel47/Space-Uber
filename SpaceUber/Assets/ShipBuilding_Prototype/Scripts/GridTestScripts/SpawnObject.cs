/*
 * SpawnObject.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Functions to spawn each of the rooms for buttons
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject _1x1;
    public GameObject _1x2;
    public GameObject _2x2;
    public GameObject _1x3;
    public GameObject _LeftL;

    public void Spawn1x1()
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(_1x1, new Vector3(1, -1, 0), Quaternion.identity);
            //g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Spawn1x2()
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(_1x2, new Vector3(1, -1, 0), Quaternion.identity);
            //g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Spawn2x2()
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(_2x2, new Vector3(1, -1, 0), Quaternion.identity);
           //g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SpawnLeftL()
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(_LeftL, new Vector3(1, -1, 0), Quaternion.identity);
            //g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Spawn1x3()
    {
        if (ObjectMover.hasPlaced == true)
        {
            ObjectMover.hasPlaced = false;
            GameObject g = Instantiate(_1x3, new Vector3(1, -1, 0), Quaternion.identity);
            //g.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
