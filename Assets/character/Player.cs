using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20.0f;
    public float jumpSpeed = 24.0f;
    public float sizeScale = 2.0f;
    public LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private BoxCollider2D col;
    private Animator anim;

    public int facing = -1;
    private bool isGrabbing = false;
    private GameObject grabbedObject;
    private string grabObjectTag = "movable";
    private Vector2 colSizeBak;
    private Vector2 colOffsetBak;
    public float grabOffset;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        colSizeBak = col.size;
        colOffsetBak = col.offset;

        transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
    }

    void Update()
    {
        // movement
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if (moveHorizontal > 0)
        {
            anim.SetBool("running", true);
            if (facing != 1)
            {
                facing = 1;
                onDirectionChange();
            }
        }
        else if (moveHorizontal < 0)
        {
            anim.SetBool("running", true);
            transform.localScale = new Vector3(2, 2, 2);
            if (facing != -1)
            {
                facing = -1;
                onDirectionChange();
            }
        }
        else
        {
            anim.SetBool("running", false);
        }

        // grab
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                // enter grab mode
                isGrabbing = true;
            }
            else
            {
                DropGrabbedObject();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            // exit grab mode
            isGrabbing = false;
        }

        if (isGrabbing)
        {
            // attempt to grab object
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facing, 1.0f, groundLayer);
            if (hit.collider != null && hit.collider.gameObject.tag == grabObjectTag)
            {
                ObtainGrabObject(hit.collider.gameObject);
                isGrabbing = false;
            }
        }

        // grab animation
        if (grabbedObject != null || isGrabbing)
        {
            anim.SetBool("grabbing", true);
        }
        else
        {
            anim.SetBool("grabbing", false);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }

    private void onDirectionChange()
    {
        // flip
        transform.localScale = new Vector3(-sizeScale * facing, sizeScale, sizeScale);

        if (grabbedObject)
        {
            // check if player collides with wall
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facing, col.size.x, groundLayer);
            if (hit.collider != null)
            {
                // move player away from wall
                transform.position = new Vector3(transform.position.x - grabOffset * sizeScale * facing, transform.position.y, transform.position.z);
            }
        }
    }

    private void ObtainGrabObject(GameObject obj)
    {
        grabbedObject = obj;
        grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        grabbedObject.transform.parent = transform;
        grabbedObject.GetComponent<BoxCollider2D>().enabled = false;

        // expand box collider to include grabbed object
        col.size = new Vector2(col.size.x + grabbedObject.GetComponent<BoxCollider2D>().size.x / sizeScale, col.size.y);
        grabOffset = grabbedObject.GetComponent<BoxCollider2D>().size.x / sizeScale / 2;
        col.offset = new Vector2(colOffsetBak.x - grabOffset, col.offset.y);
    }

    private void DropGrabbedObject()
    {
        grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<BoxCollider2D>().enabled = true;
        grabbedObject = null;

        // restore box collider
        col.size = colSizeBak;
        col.offset = colOffsetBak;
        grabOffset = 0;
    }
}
