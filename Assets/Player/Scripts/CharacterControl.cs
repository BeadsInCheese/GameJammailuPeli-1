using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    PlayerInput playerInput;
    public StatsSystem stats = new StatsSystem();
    public static CharacterControl instance;

    private Weapon weapon;
    private Collider2D collidedWeapon;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
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
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;

        }
    }

    // Update is called once per frame
    void Update()
    {
        float speed = stats.GetSpeed();
        var mouse = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 movement = playerInput.actions["walk"].ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(speed * movement.x, speed * movement.y);

        if (playerInput.actions["Attack"].triggered)
        {
            var hits = Physics2D.RaycastAll((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, mouse - (Vector2)transform.position, weapon != null ? weapon.range : stats.GetFistRange());
            foreach (RaycastHit2D hit in hits) {
                if (hit.collider != null && hit.collider.gameObject.tag.Equals("Enemy"))
                {

                    var enemy = hit.collider.gameObject.GetComponent<EnemyStats>();
                    Debug.Log("EnemyHP" + enemy.stats.GetCurrentHP());
                    float damage = weapon != null ? weapon.damage * stats.GetMeleeDamage() : stats.GetMeleeDamage();
                    enemy.stats.ChangeHP((int)damage);
                } 
            }
        }
        Debug.DrawLine((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, (Vector2)transform.position + (mouse - (Vector2)transform.position).normalized + (mouse - (Vector2)transform.position).normalized* (weapon != null ? weapon.range : stats.GetFistRange()));

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

        rigidBody.velocity = new Vector2(speed * movement.x, speed * movement.y);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (weapon == null && collider.gameObject.name == "WeaponItem")
        {
            collidedWeapon = collider;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "WeaponItem")
        {
            collidedWeapon = null;
        }
    }
}
