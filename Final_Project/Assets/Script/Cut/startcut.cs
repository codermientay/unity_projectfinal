using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{

    [SerializeField] private GameObject playCutscene; // GameObject chứa PlayableDirector
    [SerializeField] public TimelineSignalHandler timeline;
    public bool isRunyet;
    [SerializeField] public PlayerControl playerControl;
    private PlayableDirector director;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRunyet)
        {
            director = this.playCutscene.GetComponent<PlayableDirector>();
            // Kiểm tra nếu nhân vật có tag "Player" va chạm với trigger
            if (collision.gameObject.CompareTag("Player"))
            {
                // Lấy PlayableDirector từ GameObject và phát cutscene
                if (director != null)
                {
                    director.Play(); // Bắt đầu chạy cutscene
                    playerControl.isPlay = true;
                    isRunyet = true;
                }
            }
        }

    }
    public void Update()
    {
        if (director != null)
        {   
            Debug.Log(director.name+", "+director.time +", "+director.duration);
            timeline.HandleUpdateCut();
            if (director.time >= director.duration)
            {
                playerControl.isPlay = false;
                director = null;
            }
        }

    }
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     // Kiểm tra nếu là "Player" rời khỏi trigger
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         playerControl.isPlay = false; // Đặt lại isPlay thành false khi player rời đi
    //     }
    // }
}
