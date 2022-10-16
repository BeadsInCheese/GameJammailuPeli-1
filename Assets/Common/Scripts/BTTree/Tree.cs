using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Node
{
    public Tree()
    {
        name="Tree";
    }
    public override Status Process()
    {
        return this.children[currentChild].Process();
    }
}
