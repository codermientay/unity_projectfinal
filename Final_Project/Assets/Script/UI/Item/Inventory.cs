using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<ItemSlot> slots;
}

[Serializable]
public class ItemSlot
{
    [SerializeField] baseItem item;
    [SerializeField] int count;
    public baseItem Item => item;
    public int Count => count;

    public void setCount(int count)
    {
        this.count = count;
    }
    public void setItem(baseItem item)
    {
        this.item = item;
    }
}