using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseItem : ScriptableObject
{
    [Header("Thông tin")]
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] float pricesell;
    [SerializeField] float pricebuy;

    public string Name => name;
    public float Pricebuy => pricebuy;
    public float Pricesell => pricesell;
    public string Description => description;
    public Sprite Icon => icon;
}
