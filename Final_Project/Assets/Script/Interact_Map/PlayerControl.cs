using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private DialougeUI dialougeUI;
    public DialougeUI DialougeUI => dialougeUI;

    public IInteractable Interactable { get; set; }
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    void Update()
    {
        if (dialougeUI.isOpen) return;

        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Set animation parameters
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("npc"))
        {
            Interactable.Interact(this);
        }
    }
    void FixedUpdate()
    {
        // Normalize movement to avoid faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Move player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
