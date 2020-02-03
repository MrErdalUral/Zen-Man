using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public bool DestroyOnStart = false;
    public float OnStartDestroyTime = 0.25f;

    void Start()
    {
        if(DestroyOnStart) DestroyThis(OnStartDestroyTime);
    }
    public void DestroyThis(float time)
    {
        Destroy(gameObject,time);
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void DestroyOther(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
