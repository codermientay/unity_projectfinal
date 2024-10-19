using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class stopPoint : MonoBehaviour
{
    public bool isStop = false;
    public float face;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng là player
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Đã nhìn thấy player");
            // Lấy thành phần PlayerMovement
            PlayerControl playerMovement = other.GetComponent<PlayerControl>();
            if (playerMovement != null) // Kiểm tra xem thành phần có tồn tại không
            {
                Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                playerMovement.enabled = false; // Tắt script PlayerMovement
                playerMovement.animator.SetFloat("speed", 0);
                face = transform.rotation.eulerAngles.z;
                Debug.Log("Face: " + face);
                this.isStop = true;
            }
            else
            {
                Debug.LogWarning("PlayerMovement component not found on player.");
            }
        }
    }

    public void setIsStop(bool check)
    {
        this.isStop = check;
    }
}
