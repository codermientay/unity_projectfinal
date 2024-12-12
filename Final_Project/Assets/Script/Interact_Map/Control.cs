using System;
using System.Collections;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;
    public BoxCollider2D box;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounter();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0, solidObjectsLayer))
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounter()
    {
        // Kiểm tra va chạm tại vị trí hiện tại với lớp grassLayer
        var bounds = box.bounds;
        Collider2D hit = Physics2D.OverlapBox(bounds.center, bounds.size, 0, grassLayer);

        if (hit != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.SetBool("isMoving", false);
                OnEncountered?.Invoke();
            }
        }
    }
    private void Update()
    {
        // Kiểm tra va chạm mỗi frame
        if (CheckCollision())
        {
            Debug.Log("Có va chạm!");
        }
    }

    private bool CheckCollision()
    {
        // Lấy thông tin của BoxCollider2D
        Vector2 boxPosition = box.bounds.center; // Vị trí tâm của BoxCollider2D
        Vector2 boxSize = box.bounds.size;       // Kích thước của BoxCollider2D

        // Kiểm tra va chạm
        Collider2D hit = Physics2D.OverlapBox(boxPosition, boxSize, 0f, solidObjectsLayer);

        return hit != null; // Trả về true nếu có va chạm
    }

    private void OnDrawGizmos()
    {
        // Vẽ gizmo để dễ debug trong Unity Editor
        if (box != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        }
    }
}
