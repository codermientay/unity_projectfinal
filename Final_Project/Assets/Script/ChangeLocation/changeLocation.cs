using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeLocation : MonoBehaviour
{
    [SerializeField] GameObject locationUp;
    [SerializeField] GameObject player;
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] GameObject map;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Chuyển vị trí của player tới location_up
            player.transform.position = locationUp.transform.position;
            camera.ForceCameraPosition(player.transform.position, Quaternion.identity);
            CinemachineConfiner2D confiner = camera.GetComponent<CinemachineConfiner2D>();
            if (confiner != null)
            {
                confiner.m_BoundingShape2D = map.GetComponent<PolygonCollider2D>();
                camera.gameObject.SetActive(false);
                camera.gameObject.SetActive(true);

            }
            else
            {
                Debug.LogWarning("CinemachineConfiner2D không được gắn vào camera.");
            }

        }
    }
}
