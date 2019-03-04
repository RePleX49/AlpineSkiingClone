using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector2 newPos;
    Animator animator;
    ParticleSystem SnowTrail;

    public KeyCode RightKey;
    public KeyCode LeftKey;
    public KeyCode DownKey;
    public KeyCode FTrickKey;
    public KeyCode BTrickKey;
    public KeyCode RTrickKey;
    public KeyCode LTrickKey;


    public float HorizontalSpeed = 15.0f;
    public float driftSpeed = 2.0f;
    public float stunDuration = 1.25f;
    public float DownScreenDistance;
    public float TrickDuration = 1.35f;

    private float moveDirection = 0.0f;
    private float InitialYPos;
    private float DownScreenPosition;

    private bool bIsTripped;
    private bool bDoingTrick;
    private bool moveDown;
    private bool GameStarted;
    private bool SnowTrailActive;

    public enum TrickState {Neutral, ForwardTrick, BackTrick, RightTrick, LeftTrick};

    public TrickState trickState;

    void Awake()
    {
        moveDown = false;
        bDoingTrick = false;
        GameStarted = false;
        SnowTrailActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        SnowTrail = GetComponent<ParticleSystem>();
        SnowTrail.Stop();
        InitialYPos = rb.position.y;
        DownScreenPosition = InitialYPos - DownScreenDistance;
    }

    // Update is called once per frame
    void Update()
    {
        GetTrickInput();

        if(!bDoingTrick)
        {
            GetMovementInput();
        }
        
        
        TriggerAnimations();

        //TODO refactor this
        if(moveDown)
        {
            InitialYPos = Mathf.Lerp(InitialYPos, DownScreenPosition, 0.01f);
            if(InitialYPos < DownScreenPosition)
            {
                ToggleParticle();
                moveDown = false;
            }
        }       

        if (Input.GetKey(RightKey) || Input.GetKey(LeftKey))
        {            
            //Move player position respectively when left or right input is true
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }

        if (Input.GetKey(DownKey) || bDoingTrick) 
        {
            //Give player zero horizontal movement when down input is true
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }
        else if(!Input.GetKey(RightKey) && !Input.GetKey(LeftKey)) 
        {
            if(GameStarted)
            {
                //Move character at driftSpeed if there is no input
                if(!bIsTripped)
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
            // tell GameManager that game is finished and initiate movement downscreen
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

    void GetMovementInput()
    {
        if (Input.GetKey(DownKey))
        {
            moveDirection = 0.0f;
        }
        else if (!Input.GetKey(DownKey))
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

        if (Input.GetKey(RightKey))
        {
            moveDirection = 1.0f;
        }

        if (Input.GetKey(LeftKey))
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

    void GetTrickInput()
    {
        if (Input.GetKeyDown(FTrickKey))
        {
            moveDirection = 0.0f;
            trickState = TrickState.ForwardTrick;
            animator.SetTrigger("ForwardTrick");
            bDoingTrick = true;
            Invoke("ResetTrickState", TrickDuration);
        }
        else if (Input.GetKeyDown(BTrickKey))
        {
            moveDirection = 0.0f;
            trickState = TrickState.BackTrick;
            animator.SetTrigger("BackTrick");
            bDoingTrick = true;
            Invoke("ResetTrickState", TrickDuration);
        }
        else if (Input.GetKeyDown(RTrickKey))
        {
            moveDirection = 0.0f;
            sr.flipX = false;
            trickState = TrickState.RightTrick;
            animator.SetTrigger("RightTrick");
            bDoingTrick = true;
            Invoke("ResetTrickState", TrickDuration);
        }
        else if (Input.GetKeyDown(LTrickKey))
        {
            moveDirection = 0.0f;
            sr.flipX = true;
            trickState = TrickState.LeftTrick;
            animator.SetTrigger("LeftTrick");
            bDoingTrick = true;
            Invoke("ResetTrickState", TrickDuration);
        }
    }

    void ResetTrickState()
    {
        trickState = TrickState.Neutral;
        bDoingTrick = false;
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
                if(!bDoingTrick)
                {
                    sr.flipX = false;
                    animator.SetTrigger("SkiDown");
                }               
            }
        }        
    }

    void GameStart()
    {
        GameStarted = true;
        ToggleParticle();
        animator.SetBool("GameStart", true);
    }

    void ToggleParticle()
    {
        if(SnowTrailActive)
        {
            SnowTrail.Stop();
            SnowTrailActive = false;
        }
        else
        {
            SnowTrail.Play();
            SnowTrailActive = true;
        }        
    }
}
