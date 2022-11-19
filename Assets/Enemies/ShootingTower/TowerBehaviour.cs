using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    BTTree Brain;
    public GameObject bulletPrefab;
    float sight = 10;
    float attackRange = 5;
    GameObject targetObject;
    public Vector2 target;
    public float maxCooldown = 0.5f;
    // Start is called before the first frame update
    public Node.Status IsInRange()
    {
        if ((target - (Vector2)(transform.position)).magnitude < attackRange)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }
    
    private float cooldown;
    public Node.Status Shoot()
    {
        if (cooldown <= 0)
        {
            cooldown = maxCooldown;
            Vector2 dir = (target - (Vector2)(transform.position)).normalized;
            var bf = Instantiate(bulletPrefab);
            bf.transform.position = transform.position + new Vector3(dir.x, dir.y, transform.position.z);
            var bullet = bf.GetComponent<Bullet>();
            bullet.direction = dir;
        }
        transform.right = (Vector2)transform.position-target;
        return Node.Status.SUCCESS;
    }
    void Start()
    {
        targetObject = CharacterControl.instance.gameObject;
        cooldown = 0;
        Brain = new BTTree();
        SelectorNode actionSelector = new SelectorNode("actionSelector");
        LeafNode isInRange = new LeafNode("IsInRange", IsInRange);
        LeafNode shoot = new LeafNode("IsInRange", Shoot);


        Brain.AddChild(actionSelector);
        actionSelector.AddChild(isInRange);
        actionSelector.AddChild(shoot);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        target = targetObject.transform.position;
        Brain.Process();
    }
}
