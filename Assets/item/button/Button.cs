using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // public GameObject doorObj;
    public Door door;

    private Animator anim;
    private HashSet<string> interactWithTags = new HashSet<string> { "Player", "movable" };
    private int triggerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (interactWithTags.Contains(other.gameObject.tag))
        {
            triggerCount++;
        }
        if (triggerCount > 0)
        {
            anim.SetBool("pressed", true);
            door.Open();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (triggerCount > 0 && interactWithTags.Contains(other.gameObject.tag))
        {
            triggerCount--;
        }
        if (triggerCount == 0)
        {
            anim.SetBool("pressed", false);
            door.Close();
        }
    }
}
