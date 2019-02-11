using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 newPos;

    public float HorizontalSpeed = 15.0f;
    public float driftSpeed = 2.0f;
    public float stunDuration= 1.25f;
    private float moveDirection = 0.0f;
    private float InitialYPos;
    private bool bIsTripped;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitialYPos = rb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            moveDirection = 0.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1.0f;
        }

        
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }

        if (Input.GetKey(KeyCode.S))
        {
            newPos = new Vector2(rb.position.x + (moveDirection * HorizontalSpeed * Time.deltaTime), InitialYPos);
        }
        else if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            newPos = new Vector2(rb.position.x + (moveDirection * driftSpeed * Time.deltaTime), InitialYPos);
        }

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        edgeVector.x -= GetComponent<BoxCollider2D>().size.x/2;
        newPos.x = Mathf.Clamp(newPos.x, -edgeVector.x, edgeVector.x);
        
        if(!bIsTripped)
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
        // change anim state to tripped
        bIsTripped = true;
        Invoke("PlayerGetUp", stunDuration);
    }

    void PlayerGetUp()
    {
        // change anim back to skiing
        bIsTripped = false;
    }
}
