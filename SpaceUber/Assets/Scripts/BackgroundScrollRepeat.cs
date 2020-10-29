// Author: Keziah Yarnoff
// Date of creation: 10/15/2020
// Credits: Kee Gamedev on YouTube - https://www.youtube.com/watch?v=P3hcopOkpa8
// Follow video for setup. The only change I made was to make speed public for the sake of adjustment.

using UnityEngine;

public class BackgroundScrollRepeat : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    
    private float width;
    [SerializeField] private float speed;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        width = GetComponent<SpriteRenderer>().size.x;
        rigidbody2D.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > width)
        {
            Reposition();
        }

        // Toggle when background should be moving
        if (rigidbody2D.velocity.magnitude > 0 && GameManager.instance.currentGameState != InGameStates.Events)
        {
            rigidbody2D.velocity = Vector2.zero;
        }
        else if (rigidbody2D.velocity.magnitude <= 0 && GameManager.instance.currentGameState == InGameStates.Events)
        {
            rigidbody2D.velocity = new Vector2(speed, 0);
        }
    }

    private void Reposition()
    {
        transform.position += new Vector3(width * -2f, 0, 0);
    }
}
