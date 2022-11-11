using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInput playerInput;
    StatsSystem stats = new StatsSystem();
    public static CharacterControl instance;

    Weapon weapon;
    Rigidbody2D rigidBody;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else {
            Destroy(instance.gameObject);
            instance = this;
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speed =stats.GetSpeed();
        var mouse = (Vector2)Camera.main.ScreenToWorldPoint( Mouse.current.position.ReadValue()); 
        Vector2 movement = playerInput.actions["walk"].ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(speed*movement.x, speed*movement.y);
        
        if (playerInput.actions["Attack"].triggered) {
            RaycastHit2D hit=Physics2D.Raycast((Vector2)transform.position+(mouse-(Vector2)transform.position).normalized, mouse-(Vector2)transform.position,weapon!=null? weapon.range:stats.GetFistRange());
 
            Debug.Log("attacke: "+mouse);
        }
        Debug.DrawLine((Vector2)transform.position + (mouse-(Vector2)transform.position  ).normalized, mouse);
    }
}
