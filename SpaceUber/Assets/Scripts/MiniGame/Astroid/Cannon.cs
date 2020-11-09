/*
 * Cannon.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: 
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
	[SerializeField] bool left = false;
	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] Transform barrel;
	[SerializeField] float coolDown;
	[SerializeField] GameObject[] coolDownIndicators;
	[SerializeField] GameObject projectileParent;
	[SerializeField] Image coolDownBarIndicator;
	float coolDownBarWidth;
	bool canFire = true;
	Camera cam;
    public string[] fireCannon;
    private void Start()
	{
		coolDownBarWidth = coolDownBarIndicator.rectTransform.rect.width;
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
				Instantiate(projectilePrefab, barrel.position, transform.rotation, projectileParent.transform);
                AudioManager.instance.PlaySFX(fireCannon[Random.Range(0, fireCannon.Length - 1)]);
                StartCoroutine(CoolDown());
			}
		}
	}
	
	IEnumerator CoolDown()
	{
		float timeElapsed = 0;
		canFire = false;
		float x = coolDownBarIndicator.rectTransform.rect.x;
		float y = coolDownBarIndicator.rectTransform.rect.y;
		float height = coolDownBarIndicator.rectTransform.rect.height;
		while (timeElapsed < coolDown)
		{
			int indicatorNumber = Mathf.RoundToInt(timeElapsed / coolDown * coolDownIndicators.Length)+1;
			coolDownBarIndicator.rectTransform.sizeDelta = new Vector2(coolDownBarWidth * (timeElapsed / coolDown), height);
			Debug.Log(coolDownBarIndicator.rectTransform.rect.width);
			yield return new WaitForSeconds(0.01f);
			timeElapsed += 0.01f;
		}
		coolDownBarIndicator.rectTransform.sizeDelta = new Vector2(coolDownBarWidth, height);
        AudioManager.instance.PlaySFX("Ready to Fire");
        canFire = true;
	}
}