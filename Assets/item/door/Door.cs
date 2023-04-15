using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    public void Open()
    {
        GetComponent<Animator>().SetBool("open", true);
        col.enabled = false;
    }

    public void Close()
    {
        GetComponent<Animator>().SetBool("open", false);
        col.enabled = true;
    }
}
