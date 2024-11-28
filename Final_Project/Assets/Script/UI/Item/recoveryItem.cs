using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new recovery item")]
public class recoveryItem : baseItem
{
    [Header("Hồi máu")]
    [SerializeField] int hpAmonut;
    [SerializeField] bool restoreMaxHP;

    [Header("Hồi chiêu")]
    [SerializeField] int ppAmonut;
    [SerializeField] bool restoreMaxPP;


    [Header("Hồi sinh")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevie;
}
