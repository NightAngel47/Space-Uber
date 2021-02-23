/*
 * Resouce.cs
 * Author(s): Grant Frey []
 * Created on: 9/16/2020 (en-US)
 * Description:
 */

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceDataType resourceType;

    public int amount;
    public int activeAmount = 0;
    public int minAmount;

    void Start()
    {
        GetComponent<RoomStats>().AddToResourceList(this);
    }
}
