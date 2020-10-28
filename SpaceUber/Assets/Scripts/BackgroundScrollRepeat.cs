// Author: Keziah Yarnoff
// Date of creation: 10/15/2020
// Credits: Kee Gamedev on YouTube - https://www.youtube.com/watch?v=P3hcopOkpa8
// Follow video for setup. The only change I made was to make speed public for the sake of adjustment.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollRepeat : MonoBehaviour
{
    private float width;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().size.x;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 vector = new Vector2(width * -2f, 0);
        transform.position = (Vector2)transform.position + vector;
    }
}
