/*
 * DragAndDropSprite.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Enables a 2D object to be dragged and dropped
 */

using UnityEngine;

public class DragAndDropSprite : MonoBehaviour
{
    [SerializeField] string targetTag;
    MiniGameScoreManager scoreManager;
    Vector3 originalPosition;
    bool isBeingDraged = false;
    bool mousedOver = false;
    bool isOverTarget = false;

    void Start()
    {
        originalPosition = transform.position;
        scoreManager = GameObject.FindGameObjectWithTag("MiniGameScoreManager").GetComponent<MiniGameScoreManager>();
    }

    void Update()
    {
        if(isBeingDraged)
		{
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            transform.position = mousePosition;
        }
    }

	private void OnMouseEnter() { mousedOver = true;  }
	private void OnMouseExit() { mousedOver = false; }
	private void OnMouseDown() { if (mousedOver) { isBeingDraged = true; } }
	private void OnMouseUp() 
    { 
        isBeingDraged = false;
        if (isOverTarget)
        {
            Destroy(gameObject);
            scoreManager.IncrementScore();
        }
        else { transform.position = originalPosition; }
    }

	private void OnTriggerEnter2D(Collider2D collision) { if(collision.CompareTag(targetTag)) { isOverTarget = true; } }

	private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = false; } }
}