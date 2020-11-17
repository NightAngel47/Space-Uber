/*
 * HullPiece.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: 
 */

using UnityEngine;

public class HullPiece : MonoBehaviour
{
    public string[] IncrementSFX;
    Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (this == HullRepairMiniGame.selectedHullPiece)
        {
            collider.isTrigger = true;
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;
            transform.position = mousePosition;
        }
		else { collider.isTrigger = false; }
    }
    private void OnMouseDown()
    {
        Debug.Log("Click");
        Collider2D[] colliders = new Collider2D[1];
        if (this != HullRepairMiniGame.selectedHullPiece) { HullRepairMiniGame.selectedHullPiece = this; }
		else if(collider.OverlapCollider(new ContactFilter2D(), colliders ) == 0){ HullRepairMiniGame.selectedHullPiece = null; }
    }
}