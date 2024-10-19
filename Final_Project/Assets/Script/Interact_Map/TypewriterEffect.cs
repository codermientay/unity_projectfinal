using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float TypewriterSpeed = 50f;
    public Coroutine Run(String texttoType, TMP_Text textLabel){
        
        return StartCoroutine(TypeText(texttoType, textLabel));

    }
    private IEnumerator TypeText(String texttoType, TMP_Text textLabel){
        textLabel.text = String.Empty;

        // yield return new WaitForSeconds(1);
        float t = 0;
        int charIndex = 0;
        while (charIndex < texttoType.Length){
            t += Time.deltaTime * TypewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, texttoType.Length);

            textLabel.text =  texttoType.Substring(0, charIndex);

            yield return null;
        }
        textLabel.text = texttoType; 
    }
}
