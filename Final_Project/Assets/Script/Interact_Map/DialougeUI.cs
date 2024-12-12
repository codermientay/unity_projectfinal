using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class DialougeUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isOpen { get; private set; }
    private bool isDialogueFinished; // Biến theo dõi trạng thái hội thoại
    private TypewriterEffect typewriterEffect;

    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseBox();
    }

    private IEnumerator StepThroughDialogue(DialougeObject dialougeObject)
    {
        isDialogueFinished = false; // Bắt đầu hội thoại
        foreach (string dialouge in dialougeObject.Dialogue)
        {
            yield return typewriterEffect.Run(dialouge, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        CloseBox();
        isDialogueFinished = true; // Hội thoại đã kết thúc
    }

    public void ShowDialogue(DialougeObject dialougeObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialougeObject));
    }

    public bool IsDialogueFinished()
    {
        return isDialogueFinished;
    }

    private void CloseBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}

