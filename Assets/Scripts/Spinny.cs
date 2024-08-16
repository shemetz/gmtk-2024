using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinny : MonoBehaviour
{
    // This script is attached to the ball which has a rigidbody 2d
    Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (Random.Range(0, Time.fixedDeltaTime) < 0.1f)
        {
            rb.AddTorque(Random.Range(-1f, 1f));
        }
    }
}
