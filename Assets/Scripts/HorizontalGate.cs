using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGate : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = rb.position + Vector2.up * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("GameManager"))
        {
            collision.gameObject.BroadcastMessage("AddMissedGate");
            Debug.Log("MissedGate");
            Destroy(this.gameObject);
        }
    }
}
