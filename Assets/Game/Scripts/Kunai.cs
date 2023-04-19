using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
