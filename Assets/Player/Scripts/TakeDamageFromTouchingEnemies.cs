using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFromTouchingEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D col) 
    {
        Debug.Log("Collition");
    } 
    
}
