using System.Collections;
using UnityEngine;
using Cinemachine;

public class ChangeLocationWithFade : MonoBehaviour
{
    [SerializeField] private GameObject locationUp; // Vị trí chuyển đến
    [SerializeField] private GameObject player;     // Người chơi
    [SerializeField] private CinemachineVirtualCamera camera; // Camera chính
    [SerializeField] private GameObject map;        // Bản đồ
    [SerializeField] private CanvasGroup fadeCanvas; // Canvas dùng để fade
    [SerializeField] private float fadeDuration = 1f; // Thời gian fade
    [SerializeField] private GameObject camera_transition;        // Bản đồ


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            StartCoroutine(FadeAndChangeLocation());
        }
    }

    private IEnumerator FadeAndChangeLocation()
    {

        // Hiển thị hiệu ứng Fade Out (đen màn hình)
        yield return StartCoroutine(Fade(0, 1, fadeDuration));
        camera_transition.SetActive(false);
        // Chuyển vị trí của player tới locationUp
        player.transform.position = locationUp.transform.position;
        camera.ForceCameraPosition(player.transform.position, Quaternion.identity);

        // Cập nhật confiner cho Cinemachine
        CinemachineConfiner2D confiner = camera.GetComponent<CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.m_BoundingShape2D = map.GetComponent<PolygonCollider2D>();
        }
        else
        {
            Debug.LogWarning("CinemachineConfiner2D không được gắn vào camera.");
        }
        // Hiển thị hiệu ứng Fade In (trở lại bình thường)
        yield return StartCoroutine(Fade(1, 0, fadeDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            
            yield return null;
            camera_transition.SetActive(true);
        }
        
        fadeCanvas.alpha = endAlpha; // Đảm bảo alpha được đặt chính xác ở cuối.

    }
}
