using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAroundMap : MonoBehaviour
{
    void Awake()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var difVector = (collision.transform.position - transform.position);
        collision.transform.position -= difVector * 2;
    }

}
