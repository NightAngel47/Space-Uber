/*
 * Astroid.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Astroid : MonoBehaviour
{
    [SerializeField] float downwardSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float scaleSpeed;
    [SerializeField] float scaleExponent = 2;
    [SerializeField] float scaleLimit;
    [SerializeField] float meteoriteForce = 10;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject[] meteoritePrefabs;
    [SerializeField] GameObject explosion;
    List<GameObject> meteorites = new List<GameObject>();
    Image damageIndicator;
    Color originalColor;
    int rotationDirection;
    bool exploded = false;
    bool flashingIndicator = false;
    AstroidMiniGame miniGameManager;

    void Start()
    {
        miniGameManager = FindObjectOfType<AstroidMiniGame>();
        damageIndicator = GameObject.FindGameObjectWithTag("DamageIndicator").GetComponent<Image>();
        originalColor = damageIndicator.color;
        transform.localScale = Vector3.zero;
        int randomInt = Random.Range(0, 2);
        if(randomInt == 0) { rotationDirection = 1; }
        else { rotationDirection = -1; }
        GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
        StartCoroutine(IncreaseScale());
    }

    void Update()
    {
        float scale = transform.localScale.x + scaleSpeed * Time.deltaTime;
        if(scale > scaleLimit) {StartCoroutine(Explode(false)); }
        transform.localScale = new Vector3(scale, scale, scale);

        Vector3 position = transform.position;
        position.y -= downwardSpeed * Time.deltaTime;
        transform.position = position;

        Vector3 rotation = transform.eulerAngles;
        rotation.z += (rotationSpeed * rotationDirection * Time.deltaTime);
        transform.eulerAngles = rotation;

        if(miniGameManager.requiredAstroids == 0 && !exploded) { StartCoroutine(Explode(true)); }
    }

    IEnumerator IncreaseScale()
	{
        float originalScaleSpeed = scaleSpeed;
        float originalDownwardSpeed = downwardSpeed;
        while(transform.localScale.x < scaleLimit && !exploded)
		{
            yield return new WaitForSeconds(0.5f);
            scaleSpeed += originalScaleSpeed;
            downwardSpeed += downwardSpeed/2;
		}
	}

    public void StartExploding() { StartCoroutine(Explode(true)); }

    public IEnumerator Explode(bool fromRocket)
	{
        if (!exploded)
        {
            exploded = true;
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
            int meteoriteNumber = Random.Range(1, meteoritePrefabs.Length);
            explosion = Instantiate(explosion, transform.position, new Quaternion(), transform.parent);
            explosion.transform.localScale = transform.localScale;
            for (int i = 0; i <= meteoriteNumber; i++)
            {
                GameObject meteorite = Instantiate(meteoritePrefabs[i], transform.position, new Quaternion(), transform.parent);
                meteorites.Add(meteorite);
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                Vector2 direction = new Vector2(x, y) * meteoriteForce;
                meteorite.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            }
            if (!fromRocket)
            {
                StartCoroutine(FlashDamageIndicator());
				while (flashingIndicator) { yield return null; }
            }
			else 
            {
                if(miniGameManager.requiredAstroids > 0)miniGameManager.requiredAstroids--;
                yield return new WaitForSeconds(1f); 
            }
            foreach (GameObject meteorite in meteorites) { Destroy(meteorite); }
            Destroy(this);
        }
	}

    IEnumerator FlashDamageIndicator()
	{
        flashingIndicator = true;
        miniGameManager.damageText.SetActive(true);
        for (int i = 0; i <= 5; i++)
		{
            yield return new WaitForSeconds(0.08f);
            originalColor.a = i/10f;
            damageIndicator.color = originalColor;
		}
        for (int i = 4; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.08f);
            originalColor.a = i / 10f;
            damageIndicator.color = originalColor;
        }
        flashingIndicator = false;
        miniGameManager.damageText.SetActive(false);
    }
}