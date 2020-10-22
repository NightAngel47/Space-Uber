/*
 * Cannon.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	[SerializeField] bool left = false;
	[SerializeField] GameObject rocketPrefab = null;
	[SerializeField] Transform barrel;
	[SerializeField] float coolDown;
	bool canFire = true;
	Camera cam;
	private void Start()
	{
		cam = Camera.main;
	}
	private void Update()
	{
		// convert mouse position into world coordinates
		Vector2 mouseScreenPosition = cam.ScreenToWorldPoint(Input.mousePosition);

		// get direction you want to point at
		Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

		// set vector of transform directly
		transform.up = direction;
		if (canFire)
		{
			if ((left && Input.GetMouseButtonDown(1)) || (!left && Input.GetMouseButtonDown(0)))
			{
				Instantiate(rocketPrefab, barrel.position, transform.rotation, transform.parent); 
				StartCoroutine(CoolDown());
			}
			if (!left && Input.GetMouseButtonDown(0)) { Instantiate(rocketPrefab, barrel.position, transform.rotation, transform.parent); StartCoroutine(CoolDown()); }
		}
	}
	
	IEnumerator CoolDown()
	{
		canFire = false;
		yield return new WaitForSeconds(coolDown);
		canFire = true;
	}
}