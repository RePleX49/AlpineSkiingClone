using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector2 newPos;
    Animator animator;

    public float HorizontalSpeed = 15.0f;
    public float driftSpeed = 2.0f;
    public float stunDuration= 1.25f;
    private float moveDirection = 0.0f;
    private float InitialYPos;
    private bool bIsTripped;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitialYPos = rb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        TriggerAnimations();       

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {            
            //Move player position respectively when left or right input is true
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }

        if (Input.GetKey(KeyCode.S)) 
        {
            //Give player zero horizontal movement when down input is true
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }
        else if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) 
        {
            //Move character at driftSpeed if there is no input
            if(!bIsTripped)
            {
                animator.SetTrigger("SkiDrift");
            }           
            newPos = new Vector2(rb.position.x + (moveDirection * driftSpeed * Time.deltaTime), InitialYPos);
        }

        MakeHorizontalLimits();          
    }

    void FixedUpdate()
    {
        if (!bIsTripped)
        {
            rb.MovePosition(newPos);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Flag")
        {
            PlayerTrip();
            // Debug.Log("Player Tripped");
        }
    }

    void PlayerTrip()
    {
        bIsTripped = true;
        animator.SetTrigger("SkiTripped");       
        Invoke("PlayerGetUp", stunDuration);
    }

    void PlayerGetUp()
    {
        bIsTripped = false;
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection = 0.0f;
        }
        else if (!Input.GetKey(KeyCode.S))
        {
            if (moveDirection == 0.0f) // If no input then randomly assign a drift direction
            {
                if (Random.Range(0.0f, 10.0f) > 5.0f)
                {
                    moveDirection = 1.0f;
                }
                else
                {
                    moveDirection = -1.0f;
                }
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1.0f;
        }
    }

    void MakeHorizontalLimits()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        edgeVector.x -= GetComponent<BoxCollider2D>().size.x / 2;
        newPos.x = Mathf.Clamp(newPos.x, -edgeVector.x, edgeVector.x);
    }

    void TriggerAnimations()
    {
        if(!bIsTripped)
        {
            // Check moveDirection sign and flip sprite accordingly
            if (moveDirection > 0.0f)
            {
                sr.flipX = false;
                animator.SetTrigger("SkiActive");
            }
            else if (moveDirection < 0.0f)
            {
                sr.flipX = true;
                animator.SetTrigger("SkiActive");
            }
            else
            {
                sr.flipX = false;
                animator.SetTrigger("SkiDown");
            }
        }        
    }
}
