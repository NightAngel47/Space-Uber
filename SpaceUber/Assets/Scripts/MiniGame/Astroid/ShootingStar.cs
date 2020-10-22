/*
 * ShootingStar.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/21/2020 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    [SerializeField] float upwardSpeed = 1;
    [SerializeField] float scaleHeightSpeed = 0.1f;
    [SerializeField] float scaleWidthSpeed = 0.1f;
    [SerializeField] float lifeTime = 3;
    Vector3 originalPosition;
    float delay;
    float originalZ;
    bool delayed = false;
    void Start()
    {
        originalZ = transform.eulerAngles.z;
        delay = Random.Range(0f, 3f);
        originalPosition = transform.position;
        StartCoroutine(DecreaseScale());
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (delayed)
        {
            float scaleHeight = transform.localScale.x + scaleHeightSpeed * Time.deltaTime;
            float scaleWidth = transform.localScale.x + scaleWidthSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scaleWidth, scaleHeight, 1);

            transform.position += (transform.up).normalized * upwardSpeed * Time.deltaTime; ;
        }
    }

    IEnumerator DecreaseScale()
    {
        float deltaZ = Random.Range(0, 360);
        float z = Random.Range(originalZ - deltaZ, originalZ + deltaZ);
        Vector3 rotation = transform.eulerAngles;
        rotation.z = z;
        transform.eulerAngles = rotation;
        if(!delayed )yield return new WaitForSeconds(delay);
        delayed = true;
        float life = 0;
        float originalScaleHeightSpeed = scaleHeightSpeed;
        float originalScaleWidthSpeed = scaleWidthSpeed;
        float originalUpwardSpeed = upwardSpeed;
        while (life < lifeTime)
        {
            yield return new WaitForSeconds(0.5f);
            life += 0.5f;
            scaleHeightSpeed += originalScaleHeightSpeed;
            scaleWidthSpeed += originalScaleWidthSpeed;
            upwardSpeed += upwardSpeed / 2;
        }
        scaleHeightSpeed = originalScaleHeightSpeed;
        scaleWidthSpeed = originalScaleWidthSpeed;
        upwardSpeed = originalUpwardSpeed;
        transform.localScale = Vector3.zero;
        transform.position = originalPosition;
        StartCoroutine(DecreaseScale());
    }
}