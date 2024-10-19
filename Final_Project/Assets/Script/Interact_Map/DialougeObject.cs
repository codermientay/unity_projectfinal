using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialouge/DialougeObject")] 
public class DialougeObject : ScriptableObject
{
    [SerializeField] [TextArea] private String[] dialouge;
    public string[] Dialogue => dialouge;
}
