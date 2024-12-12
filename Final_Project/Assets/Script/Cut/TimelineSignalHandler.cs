using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSignalHandler : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private DialougeUI dialogueUI;

    private bool isPaused;
    public bool isStop;

    public void StopTimelineAndShowDialogue(DialougeObject dialogueObject)
    {
        // Dừng Timeline
        playableDirector.Pause();
        isPaused = true;

        // Hiển thị hội thoại
        dialogueUI.ShowDialogue(dialogueObject);
    }

    public void HandleUpdateCut()
    {
        // Khi hội thoại kết thúc, tiếp tục Timeline
        if (isPaused && dialogueUI.IsDialogueFinished())
        {
            playableDirector.Play();
            isPaused = false;

        }
    }
    public void IsCutsceneFinished()
    {
        isStop = true;
    }
}
