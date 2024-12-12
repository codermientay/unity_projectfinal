using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] public List<ShopSlot> slots;
}

[Serializable] 
public class ShopSlot
{
    [SerializeField] baseItem item;
    [SerializeField] int count;
    public baseItem Item => item;
    public int Count => count;

    public void setCount(int count){
        this.count = count;
    }
}