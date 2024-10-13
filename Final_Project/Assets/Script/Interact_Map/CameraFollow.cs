using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Offset position of the camera relative to the player

    void LateUpdate()
    {
        // Only follow the player's X and Y position, and keep the camera's Z position constant
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
    }
}
