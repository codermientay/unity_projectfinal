using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    Healer healer;
    private void Awake()
    {
        healer = GetComponent<Healer>();
    }

    public void Interact()
    {
        //if (healer != null)
        //{
        //    yield return healer.Heal(initiator, dialog);
        //}
        //else
        //{

        //    yield return DialogManager.Instance.ShowDialog(dialog);
        //}
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
