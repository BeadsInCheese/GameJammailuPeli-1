using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 direction;
    public float speed=5;
    public int dmg=20;
    void Start()
    {
        transform.GetComponent<Rigidbody2D>().velocity=direction*speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            CharacterControl.instance.stats.ChangeHP(-dmg);

        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
