/*
 * HullPiece.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: Controls the individual pieces for the hull repair puzzle. Includes functionality for picking up the pieces
 */

using UnityEngine;

public class HullPiece : MonoBehaviour
{
    public string[] PickupSFX;
    public string[] PutdownSFX;
    public string[] BumpSFX;

    Collider2D collide;
    public HullPieceSlot mySlot;

    void Start()
    {
        collide = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (this == HullRepairMiniGame.selectedHullPiece)
        {
            collide.isTrigger = true;

            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = -0.1f;
            transform.position = mousePosition;
        }
		else { collide.isTrigger = false; }
    }

	private void OnMouseDown()
    {
        Collider2D[] colliders = new Collider2D[1];

        //if there is no hull piece currently selected, see if you can pick one up
        if (this != HullRepairMiniGame.selectedHullPiece) 
        { 
            HullRepairMiniGame.selectedHullPiece = this; 
            AudioManager.instance.PlaySFX(PickupSFX[Random.Range(0, PickupSFX.Length)]);
        }
        //Dropping a hull piece
        else
        {
            //Figure out everything that is overlapping this piece
            collide.OverlapCollider(new ContactFilter2D(), colliders);
            bool collidedWithHullPiece = false;
            bool outOfBounds = false;
            
            foreach(Collider2D collider in colliders)
            {
				if (collider) 
                { 
                    if (collider.CompareTag("Hull Piece")) 
                    { 
                        collidedWithHullPiece = true; 
                        AudioManager.instance.PlaySFX(BumpSFX[Random.Range(0, BumpSFX.Length)]); 
                    } 
                    if(collider.CompareTag("OutOfBounds"))
                    {
                        AudioManager.instance.PlaySFX(BumpSFX[Random.Range(0, BumpSFX.Length)]);
                        outOfBounds = true;
                    }
                }
            }
			if (!collidedWithHullPiece && !outOfBounds) 
            { 
                HullRepairMiniGame.selectedHullPiece = null; 
                AudioManager.instance.PlaySFX(PutdownSFX[Random.Range(0, PutdownSFX.Length)]);
            }
        }
    }
}