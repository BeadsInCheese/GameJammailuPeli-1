using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInput playerInput;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = playerInput.actions["walk"].ReadValue<Vector2>();
        transform.position = new Vector2(movement.x*Time.deltaTime+transform.position.x,movement.y*Time.deltaTime+transform.position.y);
    }
}
