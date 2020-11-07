/*
 * DestroyAfterTime.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/6/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float time;
    void Start()
    {
        StartCoroutine(DestroyThisAfterTime());
    }

    IEnumerator DestroyThisAfterTime()
	{
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
	}
}