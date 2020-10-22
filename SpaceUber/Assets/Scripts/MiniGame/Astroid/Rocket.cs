/*
 * Rocket.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float upwardSpeed;
    [SerializeField] float scaleSpeed;
    void Start()
    {
        StartCoroutine(DecreaseScale());
    }

    void Update()
    {
        float scale = transform.localScale.x - scaleSpeed * Time.deltaTime;
        if (scale <= 0) { Destroy(gameObject); }
        transform.localScale = new Vector3(scale, scale, scale);

        transform.position += (transform.up).normalized * upwardSpeed * Time.deltaTime; ;
    }

    void OnTriggerEnter2D(Collider2D other)
	{
        Astroid astroid = other.GetComponent<Astroid>();
		if (astroid) 
        {
            astroid.StartExploding();
            Destroy(gameObject);
        }
	}

    IEnumerator DecreaseScale()
    {
        float originalScaleSpeed = scaleSpeed;
        float originalUpwardSpeed = upwardSpeed;
        while (transform.localScale.x >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            scaleSpeed += originalScaleSpeed;
            upwardSpeed -= upwardSpeed / 2;
        }
    }
}