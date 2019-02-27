using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGate : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerScript ps;
    Animator animator;

    public float moveSpeed = 2.0f;

    public bool playerPassed = false;

    PlayerScript.TrickState RequiredTrick;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();

        int TrickInt = Random.Range(1, 4);

        animator.SetInteger("TrickSpriteInt", TrickInt);
        RequiredTrick = (PlayerScript.TrickState)TrickInt;
    }

    void FixedUpdate()
    {
        Vector2 newPos = rb.position + Vector2.up * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {            
            if(ps.trickState == RequiredTrick)
            {
                playerPassed = true;
                Debug.Log("Passed");
            }            
        }
        else if (collision.gameObject.tag == "GameManager")
        {
            if(!playerPassed)
            {
                collision.gameObject.GetComponent<GameManager>().AddMissedGate();
            }
            
            Destroy(this.gameObject);
        }
        // Debug.Log("Collided");
    }
}
