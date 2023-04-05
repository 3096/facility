using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20.0f;
    public float jumpSpeed = 24.0f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
    }
}
