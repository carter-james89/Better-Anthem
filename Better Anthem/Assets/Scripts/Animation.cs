using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Transform target;

    public Vector3 startPos;
    public Vector3 endPos;

    public bool running = false;

    public float animationDist;

    private void Awake()
    {
        
    }
}
