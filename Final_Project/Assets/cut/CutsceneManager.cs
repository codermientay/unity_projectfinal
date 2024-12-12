using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private DialougeUI dialogueUI; // Tham chiếu đến `DialougeUI`
    [SerializeField] private DialougeObject dialogueObject; // Nội dung hội thoại
    [SerializeField] private Transform npc; // NPC trong cutscene
    [SerializeField] private Transform npcTargetPosition; // Điểm NPC cần di chuyển đến

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // 1. Dừng điều khiển nhân vật
        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.enabled = false;
        }

        // 2. Di chuyển NPC đến vị trí chỉ định
        yield return MoveToPosition(npc, npcTargetPosition.position, 2f);

        // 3. Hiển thị hội thoại
        dialogueUI.ShowDialogue(dialogueObject);

        // 4. Chờ đến khi hội thoại kết thúc
        yield return new WaitUntil(() => dialogueUI.IsDialogueFinished());

        // 5. Kết thúc cutscene, bật lại điều khiển nhân vật
        if (player != null)
        {
            player.enabled = true;
        }
    }

    private IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(obj.position, targetPosition) > 0.1f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
