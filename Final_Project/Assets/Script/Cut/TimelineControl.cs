using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private DialougeUI dialogueUI;

    private void Update()
    {
        // Kiểm tra nếu hội thoại đã hoàn tất và Timeline đang bị tạm dừng
        if (dialogueUI.IsDialogueFinished() && playableDirector.state == PlayState.Paused)
        {
            playableDirector.Play(); // Tiếp tục Timeline
        }
    }

    public void PauseTimeline()
    {
        playableDirector.Pause(); // Tạm dừng Timeline
    }
}
