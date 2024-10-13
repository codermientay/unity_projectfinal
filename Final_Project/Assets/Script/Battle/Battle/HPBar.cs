using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    //private void Start()
    //{
    //    health.transform.localScale = new Vector3(0.5f, 0.6f);
    //}
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}
