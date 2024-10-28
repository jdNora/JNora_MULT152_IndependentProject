using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // NOTICE: THE PLAYER MOVEMENT USES THE RIGIDBODY FOR SMOOTH AND ACCURATE MOTION

    // Movement Values
    public float baseSpeed = 6.0f;
    public float groundDrag = 0.0f; // Use for strong winds and sludge
    public float runMultiplier = 1.75f;

    // Player Status
    public float playerHeight = 2.0f;
    public float movementSpeed;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Keybinds
    public KeyCode sprintKey = KeyCode.LeftShift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = groundDrag;
        movementSpeed = baseSpeed;
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void CheckInput() // Checks for input
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(sprintKey))
        {
            movementSpeed = baseSpeed * runMultiplier;
        }
        else
        {
            movementSpeed = baseSpeed;
        }
    }
    
    private void MovePlayer() // Handles the player's movement
    {
        // Get movement direction and apply force

        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * movementSpeed * 5.0f, ForceMode.Force);

        // Limits the player's forward and horizontal velocity to the intended speed

        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
