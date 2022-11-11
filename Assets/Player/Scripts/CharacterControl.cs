using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    public float agility = 1.5f;
    public float strength = 1;
    public float endurance = 1;
    public float ability = 1;
    public float luck = 1;

    public float fistRange = 1;

    private PlayerInput playerInput;

    private Weapon weapon;
    private Collider2D collidedWeapon;

    private Rigidbody2D rigidBody;



    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5 + agility * 0.2f;
        var mouse = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 movement = playerInput.actions["walk"].ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(speed * movement.x, speed * movement.y);

        if (playerInput.actions["Attack"].triggered)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, mouse - (Vector2)transform.position, weapon != null ? weapon.range : fistRange);

            Debug.Log("attacked: " + mouse);
        }
        Debug.DrawLine((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, mouse);

        if (weapon != null && playerInput.actions["PickItem"].triggered)
        {
            Debug.Log("Dropped an item");
            var weaponTransform = transform.Find("WeaponItem");
            weaponTransform.SetParent(null);
            var weaponCollider = weaponTransform.gameObject.GetComponent<BoxCollider2D>();
            weaponCollider.enabled = true;
            weapon = null;
        }

        if (weapon == null && playerInput.actions["PickItem"].triggered && collidedWeapon != null)
        {
            Debug.Log("Picked up an item");
            collidedWeapon.transform.SetParent(transform);
            weapon = collidedWeapon.gameObject.GetComponent<Weapon>();
            collidedWeapon.enabled = false;
            collidedWeapon = null;
        }

        transform.right = (mouse - (Vector2)transform.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (weapon == null && collider.gameObject.name == "WeaponItem") {
            collidedWeapon = collider;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name == "WeaponItem") {
            collidedWeapon = null;
        }
    }
}
