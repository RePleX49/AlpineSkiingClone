using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    public float moveSpeed = 2.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 newPos = rb.position + Vector2.up * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GameManager")
        {
            Destroy(this.gameObject);
        }
    }
}
