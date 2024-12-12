using UnityEngine;

public class DiaAct : MonoBehaviour, IInteractable
{
    [SerializeField] private DialougeObject dialougeObject;

    public void Interact(PlayerControl player)
    {
        player.DialougeUI.ShowDialogue(dialougeObject);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
            player.Interactable = this;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
            if (player.Interactable is DiaAct diaAct && diaAct == this)
            {
                player.Interactable = null;
            }
        }
    }
}