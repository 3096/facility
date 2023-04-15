using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20.0f;
    public float jumpSpeed = 24.0f;
    public LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private BoxCollider2D col;
    private Animator anim;

    private bool isGrabbing = false;
    private GameObject grabbedObject;
    private string grabObjectTag = "movable";

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // movement logic
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        // movement animations
        if (moveHorizontal > 0)
        {
            anim.SetBool("running", true);
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (moveHorizontal < 0)
        {
            anim.SetBool("running", true);
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            anim.SetBool("running", false);
        }

        // grab logic
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     if (grabbedObject == null)
        //     {
        //         Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0f);
        //         foreach (Collider2D collider in colliders)
        //         {
        //             if (collider.gameObject.tag == grabObjectTag)
        //             {
        //                 Debug.Log("Grabbed " + collider.gameObject.name);
        //                 grabbedObject = collider.gameObject;
        //                 grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //                 grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //                 grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //                 grabbedObject.transform.parent = transform;
        //                 break;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //         grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        //         grabbedObject.transform.parent = null;
        //         grabbedObject = null;
        //     }
        // }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }
}
