﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGate : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 2.0f;
    public bool playerPassed = false;

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

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerPassed = true;
        }
        // Debug.Log("Collided");
    }
}
