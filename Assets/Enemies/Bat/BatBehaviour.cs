using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Navigation nav;
    BTTree Brain;
    float proximityTreshhold=0.3f;
    float sight=10;
    float attackRange = 5;
    public float attackWaitTime=1;
    private Node.Status Wonder()
    {
        nav.moving = true;
        if ((CharacterControl.instance.transform.position - this.transform.position).magnitude < sight) 
        {
            return Node.Status.FAILURE;
        }
        if (Mathf.Abs(((Vector2)(transform.position) - nav.Target).magnitude)<proximityTreshhold)
        {
            nav.Target = new Vector2(transform.position.x + Random.Range(-30, 30), transform.position.y + Random.Range(-30, 30));
            
        }
        return Node.Status.RUNNING;
    }

    private Node.Status Seek() {
        nav.moving = true;
        if ((CharacterControl.instance.transform.position - this.transform.position).magnitude < attackRange) 
        {
            return Node.Status.SUCCESS;
        }
        nav.Target = CharacterControl.instance.transform.position;
        return Node.Status.RUNNING;
    }
    private float waitTime=0;
    private Node.Status WaitToAttack() 
    {
        nav.moving = false;
        if (waitTime <= 0) {
            waitTime = attackWaitTime;
            return Node.Status.SUCCESS;
        }
        waitTime -= Time.deltaTime;
        return Node.Status.RUNNING;
    }

    public float dashForce = 300; 
    private Node.Status Dash() {
        nav.moving = false;
        Vector2 dir = (CharacterControl.instance.transform.position-transform.position).normalized;
        rigidBody.AddForce(dir * dashForce);
        return Node.Status.SUCCESS;
    }
    Rigidbody2D rigidBody;
    void Start()
    {
        rigidBody=this.GetComponent<Rigidbody2D>();
        waitTime = attackWaitTime;
        Brain = new BTTree();
        SelectorNode actionSelector = new SelectorNode("actionSelector");
        LeafNode wonder = new LeafNode("wwonder",Wonder);
        SequenceNode seekAndDestroySequence = new SequenceNode("Seek and destroy");
        LeafNode seek = new LeafNode("seek", Seek);
        LeafNode waitToAttack = new LeafNode("wait to attack", WaitToAttack);
        LeafNode dash = new LeafNode("dash", Dash);

        Brain.AddChild(actionSelector);
        actionSelector.AddChild(wonder);
        actionSelector.AddChild(seekAndDestroySequence);
        seekAndDestroySequence.AddChild(seek);
        seekAndDestroySequence.AddChild(waitToAttack);
        seekAndDestroySequence.AddChild(dash);






    }

    // Update is called once per frame
    void Update()
    {
        Brain.Process();   
    }
}
