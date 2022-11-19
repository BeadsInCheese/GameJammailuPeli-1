using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    BTTree Brain;
    public GameObject bulletPrefab;
    float sight = 10;
    float attackRange = 5;
    public GameObject targetObject;
    public Vector2 target;
    // Start is called before the first frame update
    public Node.Status IsInRange() {
        if ((target - (Vector2)(transform.position)).magnitude < attackRange) {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCCESS;
        }
    }
    public Node.Status Shoot()
    {
        Vector2 dir = (target - (Vector2)(transform.position)).normalized;
        var bf=Instantiate(bulletPrefab);
        bf.transform.position = transform.position+new Vector3(dir.x , dir.y,transform.position.z);
        var bullet=bf.GetComponent<Bullet>();
        bullet.direction = dir;

        return Node.Status.SUCCESS;
    }
    void Start()
    {
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
        target = targetObject.transform.position;
        Brain.Process();
    }
}
