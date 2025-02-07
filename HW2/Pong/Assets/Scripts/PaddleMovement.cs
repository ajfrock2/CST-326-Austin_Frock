using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    
    private Vector2 movementDirection;
    public InputActionReference move;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementDirection.x * speed,  rb.linearVelocity.y,  rb.linearVelocity.z);
    }
}
