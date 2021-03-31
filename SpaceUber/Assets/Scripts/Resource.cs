/*
 * Resouce.cs
 * Author(s): Grant Frey []
 * Created on: 9/16/2020 (en-US)
 * Description:
 */

using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceDataType resourceType;

    public int[] amount;
    public int activeAmount = 0;
    public int minAmount;

    void Awake()
    {
        GetComponent<RoomStats>().AddToResourceList(this);
    }
}
