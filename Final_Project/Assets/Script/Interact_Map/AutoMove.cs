using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Diagnostics;

public class AutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    public bool isStop = false; // Trạng thái dừng di chuyển

    private List<Vector3> path; // Danh sách vị trí từ A*
    private int currentTargetIndex = 0; // Chỉ số vị trí mục tiêu hiện tại

    public Grid_HomeMade grid_HomeMade; // Lưới được sử dụng trong A*
    private A_Star aStar; // Tham chiếu tới lớp A_Star

    public Vector3[] goals; // Danh sách các mục tiêu

    void Start()
    {
        aStar = new A_Star(grid_HomeMade.grid, grid_HomeMade.tilemap, grid_HomeMade.x_left, grid_HomeMade.x_right, grid_HomeMade.y_up, grid_HomeMade.y_down);
        StartCoroutine(CallFunction());
    }

    IEnumerator CallFunction()
    {
        foreach (var goal in goals)
        {
            // Làm tròn vị trí hiện tại
            Vector3 currentPosition = RoundVector3(transform.position);
            transform.position = currentPosition;

            // Tìm đường đi từ vị trí hiện tại đến mục tiêu
            path = aStar.FindPath(currentPosition, goal);
            if (path == null || path.Count == 0)
            {
                UnityEngine.Debug.LogWarning("Không tìm thấy đường đi từ " + currentPosition + " đến " + goal);
                yield return new WaitForSeconds(1f);
                continue;
            }
            // UnityEngine.Debug.Log("Đường đi được tìm thấy: ");
            // foreach (var pos in path)
            // {
            //     UnityEngine.Debug.Log(pos);
            // }


            currentTargetIndex = 0; // Reset chỉ số mục tiêu
            isStop = false;

            // Di chuyển qua từng vị trí trong đường đi
            while (currentTargetIndex < path.Count)
            {
                MoveToTarget(path[currentTargetIndex]);
                yield return null; // Chờ cho đến khung hình tiếp theo
            }

            // Đợi 1 giây trước khi di chuyển đến mục tiêu tiếp theo
            yield return new WaitForSeconds(0.5f);
        }

        UnityEngine.Debug.Log("Hoàn thành tất cả các mục tiêu.");
    }

    private Vector3 RoundVector3(Vector3 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position.z = Mathf.Round(position.z);
        return position;
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        // Tính toán hướng di chuyển
        Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;

        // Di chuyển đối tượng
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Cập nhật animation
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetFloat("speed", direction.sqrMagnitude);

        // Kiểm tra nếu đã đạt được mục tiêu hiện tại
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            currentTargetIndex++;
        }

        // Dừng animation nếu đã đạt mục tiêu cuối
        if (currentTargetIndex >= path.Count)
        {
            animator.SetFloat("speed", 0);
            isStop = true;
        }
    }
}
