/*
 * MiniGameTool.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class MiniGameTool : MonoBehaviour
{
    [SerializeField] string targetTag;
    MiniGameScoreManager scoreManager;
    public MiniGameToolType toolType;
    Vector3 originalPosition;
    bool isBeingDraged = false;
    bool mousedOver = false;
    bool isOverTarget = false;
    int originalLayer;

    void Start()
    {
        originalLayer = gameObject.layer;
        originalPosition = transform.position;
        scoreManager = GameObject.FindGameObjectWithTag("MiniGameScoreManager").GetComponent<MiniGameScoreManager>();
    }

    void Update()
    {
        if (isBeingDraged)
        {
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            transform.position = mousePosition;
        }
        if(MiniGameScoreManager.selectedTool != this) 
        {
            isBeingDraged = false;
            transform.position = originalPosition;
        }
        if (Input.GetMouseButtonDown(1))
        {
            MiniGameScoreManager.selectedTool = null;
            gameObject.layer = originalLayer;
        }
    }

    private void OnMouseDown() 
    {
            isBeingDraged = true;
            MiniGameScoreManager.selectedTool = this;
            gameObject.layer = 2;
    }
    

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = true; } }

    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag(targetTag)) { isOverTarget = false; } }
}

public enum MiniGameToolType { WateringCan, Fertilizer, Clippers, Seed }