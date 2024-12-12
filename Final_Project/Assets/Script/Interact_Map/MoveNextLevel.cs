using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveNextLevel : MonoBehaviour
{
    public GameObject loadingScene; // UI hiển thị khi đang tải cảnh
    public Slider slider; // Thanh tải
    public int sceneBuildindex; // Index cảnh tiếp theo

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Chuyển cảnh
            loadLevel();
        }
    }

    public void loadLevel()
    {
        StartCoroutine(Loading_Asyn(0));
    }

    IEnumerator Loading_Asyn(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        loadingScene.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
    // public void Loading()
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(0);
    // }
}
