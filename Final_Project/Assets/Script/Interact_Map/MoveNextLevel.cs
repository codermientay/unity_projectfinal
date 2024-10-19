using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveNextLevel : MonoBehaviour
{
    public GameObject loadingScene;
    public Slider slider;
    public int sceneBuildindex;

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Đã chuyển map");
        if(other.tag == "Player"){
            loadLevel(sceneBuildindex);
        }
    }

    public void loadLevel(int index){
        StartCoroutine(Loading_Asyn(index));
    }
    IEnumerator Loading_Asyn(int index){
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        loadingScene.SetActive(true);
        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress/.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
