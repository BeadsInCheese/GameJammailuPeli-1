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
    private Collider2D touchingWeaponCollider;

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

    private float cooldown;

    // Update is called once per frame
    void Update()
    {
        float speed = stats.GetSpeed();
        var mouse = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 movement = playerInput.actions["walk"].ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(speed * movement.x, speed * movement.y);

        HandleAttack(mouse);

        Debug.DrawLine((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, (Vector2)transform.position + (mouse - (Vector2)transform.position).normalized + (mouse - (Vector2)transform.position).normalized * (weapon != null ? weapon.range : stats.GetFistRange()));

        HandlePickup(mouse);

        transform.right = (mouse - (Vector2)transform.position).normalized;

        rigidBody.velocity = new Vector2(speed * movement.x, speed * movement.y);

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    void HandleAttack(Vector2 mouse)
    {
        if (!playerInput.actions["Attack"].triggered || cooldown > 0) return;

        // Using ranged (particle) weapon
        if (weapon != null && weapon.bulletPrefab != null)
        {
            cooldown = weapon.attackSpeed;
            var bf = Instantiate(weapon.bulletPrefab);
            bf.transform.position = weapon.muzzle.position;
            var bullet = bf.GetComponent<Bullet>();
            bullet.damagesPlayer = false;
            bullet.bulletDamage = weapon.damage;
            bullet.direction = (mouse - (Vector2)(transform.position)).normalized;
        }
        // No weapon or melee weapon
        else
        {
            var hits = Physics2D.RaycastAll((Vector2)transform.position + (mouse - (Vector2)transform.position).normalized, mouse - (Vector2)transform.position, weapon != null ? weapon.range : stats.GetFistRange());
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.tag.Equals("Enemy"))
                {
                    var enemy = hit.collider.gameObject.GetComponent<EnemyStats>();
                    Debug.Log("EnemyHP" + enemy.stats.GetCurrentHP());
                    float damage = weapon != null ? weapon.damage * stats.GetMeleeDamage() : stats.GetMeleeDamage();
                    enemy.stats.ChangeHP((int)damage);
                }
            }
        }
    }

    void HandlePickup(Vector2 mouse)
    {
        if (playerInput.actions["PickItem"].triggered && weapon != null)
        {
            Debug.Log("Dropped an item");
            weapon.transform.SetParent(null);
            weapon.transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            weapon = null;
        }

        if (playerInput.actions["PickItem"].triggered && touchingWeaponCollider != null && weapon == null)
        {
            Debug.Log("Picked up an item");
            touchingWeaponCollider.transform.SetParent(transform);

            touchingWeaponCollider.transform.up = -((Vector2)transform.position - mouse);
            touchingWeaponCollider.transform.position = (Vector2)transform.position + (mouse - (Vector2)transform.position).normalized;

            weapon = touchingWeaponCollider.gameObject.GetComponent<Weapon>();
            touchingWeaponCollider.enabled = false;
            touchingWeaponCollider = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (weapon == null && collider.gameObject.tag == "Weapon")
        {
            touchingWeaponCollider = collider;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Weapon")
        {
            touchingWeaponCollider = null;
        }
    }
}
