using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageOnPlayerTouch : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 10;
    private void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            CharacterControl.instance.stats.ChangeHP(-damage);
            Debug.Log(CharacterControl.instance.stats.GetCurrentHP());
        }
    } 
    
}
