using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _acc = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 vector)
    {
        GetComponent<Rigidbody2D>().AddForce(vector * _acc,ForceMode2D.Force);
    }

    public void SetVelocity(Vector2 vector)
    {
        GetComponent<Rigidbody2D>().AddForce(vector* _acc,ForceMode2D.Impulse);
    }
}
