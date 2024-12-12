using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerCutscene : MonoBehaviour
{
    [SerializeField] private CutsceneManager cutsceneManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cutsceneManager.StartCutscene();
        }
    }
}

