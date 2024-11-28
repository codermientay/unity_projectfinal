using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class DialougeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isOpen{get; private set;}
    private TypewriterEffect typewriterEffect;
    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseBox();
    }

    private IEnumerator StepThroughDialogue(DialougeObject dialougeObject)
    {
        // yield return new WaitForSeconds(1);
        foreach (string dialouge in dialougeObject.Dialogue)
        {
            Debug.Log(dialouge);
            yield return typewriterEffect.Run(dialouge, textLabel);
            yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space));
        }
        CloseBox();
    }
    public void showDialogue(DialougeObject dialougeObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialougeObject));

    }
    private void CloseBox(){
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = String.Empty;
    }
}
