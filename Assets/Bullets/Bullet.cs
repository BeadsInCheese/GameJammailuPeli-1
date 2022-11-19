using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 direction;
    public float speed = 5;
    public int bulletDamage = 20;
    public bool damagesPlayer = true;

    void Start()
    {
        transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterControl.instance.stats.ChangeHP(-bulletDamage);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyStats>().stats.ChangeHP(-bulletDamage);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
