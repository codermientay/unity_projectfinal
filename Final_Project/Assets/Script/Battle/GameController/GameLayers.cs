using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask fovLayer;



    public static GameLayers i { get; set; }
    private void Awake()
    {
        i = this;
    }
    public LayerMask FovLayer
    {
        get => fovLayer;
    }
}
