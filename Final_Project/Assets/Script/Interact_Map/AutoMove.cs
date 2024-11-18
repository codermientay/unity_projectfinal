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
    public bool isStop = false;

    // Tham chiếu đến stopPoint
    public stopPoint stopPointScript;
    private Vector3 old;
    private List<Vector3> path; // Danh sách vị trí từ A*
    private int currentTargetIndex = 0; // Chỉ số vị trí mục tiêu hiện tại

    public Grid_HomeMade grid_HomeMade;

    private A_Star aStar; // Tham chiếu tới lớp A_Star

    void Start()
    {
        path = new List<Vector3>();
        aStar = new A_Star(grid_HomeMade.grid, grid_HomeMade.tilemap, grid_HomeMade.x_left, grid_HomeMade.x_right, grid_HomeMade.y_up, grid_HomeMade.y_down);
        Vector3 start = transform.position; // Vị trí hiện tại của NPC
        Vector3 goal = new Vector3(10, -4, 0); // Mục tiêu

        path = aStar.FindPath(start, goal);
    }
    void Update()
    {

    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Đã chạm vào player");
    //         stopPointScript.setIsStop(false);
    //         animator.SetFloat("speed", 0);
    //         this.isStop = true;
    //     }
    // }
    void FixedUpdate()
    {
        if (path != null && currentTargetIndex < path.Count && !isStop)
        {
            Vector3 targetPosition = path[currentTargetIndex];
            Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;

            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Cập nhật animation
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);

            // Kiểm tra nếu đã đến gần mục tiêu hiện tại
            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                currentTargetIndex++;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }
}
