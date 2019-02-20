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
    public float DownScreenDistance;
    private float moveDirection = 0.0f;
    private float InitialYPos;
    private float DownScreenPosition;
    private bool bIsTripped;
    private bool moveDown = false;
    private bool GameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InitialYPos = rb.position.y;
        DownScreenPosition = InitialYPos - DownScreenDistance;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        TriggerAnimations();

        //TODO refactor this
        if(moveDown)
        {
            InitialYPos = Mathf.Lerp(InitialYPos, DownScreenPosition, 0.01f);
            if(InitialYPos < DownScreenPosition)
            {
                moveDown = false;
            }
        }

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
            if(GameStarted)
            {
                //Move character at driftSpeed if there is no input
                if (!bIsTripped)
                {
                    animator.SetTrigger("SkiDrift");
                }
                newPos = new Vector2(rb.position.x + (moveDirection * driftSpeed * Time.deltaTime), InitialYPos);
            }
            else
            {
                animator.SetTrigger("PrepDefault");
            }
        }

        MakeHorizontalLimits();          
    }

    void FixedUpdate()
    {
        if (!bIsTripped)
        {
            rb.MovePosition(newPos);
           // Debug.Log("Updating Position");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Flag")
        {
            PlayerTrip();
            // Debug.Log("Player Tripped");
        }
        else if (collision.gameObject.tag == "FinishLine")
        {
            moveDown = true;
            GameObject gm = GameObject.Find("GameManager");
            gm.GetComponent<GameManager>().GameFinished = true;
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
                animator.SetTrigger("PrepMove");
            }
            else if (moveDirection < 0.0f)
            {
                sr.flipX = true;
                animator.SetTrigger("SkiActive");
                animator.SetTrigger("PrepMove");
            }
            else
            {
                sr.flipX = false;
                animator.SetTrigger("SkiDown");
            }
        }        
    }

    void GameStart()
    {
        GameStarted = true;
        animator.SetBool("GameStart", true);
    }
}
