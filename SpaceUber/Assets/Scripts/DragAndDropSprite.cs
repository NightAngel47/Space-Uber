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
    CropHarvestMiniGame miniGameManager;
    bool isBeingDraged = false;
    public string[] IncrementSFX;

    void Start()
    {
        miniGameManager = GameObject.FindGameObjectWithTag("MiniGameScoreManager").GetComponent<CropHarvestMiniGame>();
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
	private void OnMouseDown() 
    { 
        isBeingDraged = !isBeingDraged;
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(targetTag)) 
        {
            Destroy(gameObject);
            miniGameManager.IncrementScore();

            AudioManager.instance.PlaySFX(IncrementSFX[Random.Range(0, IncrementSFX.Length - 1)]);
        } 
    }
}