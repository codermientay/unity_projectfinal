using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;

    // Thêm biến để lưu trữ trạng thái dừng
    public bool isStop;

    // Tham chiếu đến stopPoint
    public stopPoint stopPointScript;
    private Vector2 old;
    void Start()
    {
        old = this.transform.position;
        if (stopPointScript != null)
        {
            isStop = stopPointScript.isStop;
            Debug.Log("Vị trí stopPoint: " + stopPointScript.transform.position);
        }

    }
    void Update()
    {
        if (stopPointScript.isStop && stopPointScript.face == 0)
        {
            movement.y = -1;

            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            movement.y = 0;
            movement.x = 0;
        }

        // Set animation parameters
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Đã chạm vào player");
            stopPointScript.setIsStop(false);
            animator.SetFloat("speed", 0);
            this.isStop = true;
        }
    }
    void FixedUpdate()
    {
        // Normalize movement to avoid faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Move NPC
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
