using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 direction;
    public float speed=5;
    void Start()
    {
        transform.GetComponent<Rigidbody2D>().velocity=direction*speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
