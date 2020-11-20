/*
 * Slot.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] int value;

    public int GetValue() { return value; }
}