/*
 * HullGridSquare.cs
 * Author(s): #Greg Brandt#
 * Created on: 12/1/2020 (en-US)
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class HullGridSquare : MonoBehaviour
{
    Collider2D collider;
    Image image;
    bool isCovered = false;

	private void Start()
	{
        collider = GetComponent<Collider2D>();
        image = GetComponent<Image>();
	}
	private void Update()
	{
        Collider2D[] colliders = new Collider2D[1];
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        bool collidedWithHullPiece = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider) { if (collider.CompareTag("Hull Piece")) { collidedWithHullPiece = true; } }
        }
        if (collidedWithHullPiece) { image.color = Color.green; isCovered = true; }
        else { image.color = Color.red; isCovered = false; }
    }

    public bool IsCovered() { return isCovered; }
}