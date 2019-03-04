using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGate : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerScript ps;
    Animator animator;
    SpriteRenderer sr;    
    AudioSource audioSource;

    public ParticleSystem ConfettiParticle1;
    public ParticleSystem ConfettiParticle2;

    public float moveSpeed = 2.0f;
    public bool playerPassed = false;

    int TrickInt;

    PlayerScript.TrickState GateTrick;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        TrickInt = Random.Range(1, 5);

        animator.SetInteger("TrickSpriteInt", TrickInt);
        GateTrick = (PlayerScript.TrickState)TrickInt;
        Debug.Log(GateTrick);
    }

    void FixedUpdate()
    {
        if(TrickInt == 4)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        Vector2 newPos = rb.position + Vector2.up * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {            
            if(ps.trickState == GateTrick)
            {
                playerPassed = true;
                audioSource.Play();
                ConfettiParticle1.Play();
                ConfettiParticle2.Play();
                // Debug.Log("Passed");
            }            
        }
        else if (collision.gameObject.tag == "GameManager")
        {
            if(playerPassed)
            {
                collision.gameObject.GetComponent<GameManager>().AddGate();
            }
            
            Destroy(this.gameObject);
        }
        // Debug.Log("Collided");
    }
}
