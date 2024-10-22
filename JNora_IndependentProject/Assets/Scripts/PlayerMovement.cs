using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement Values
    public float baseSpeed = 8.0f;
    public float groundDrag = 5.0f;
    public float jumpForce = 4.0f;
    public float jumpCooldown = 1.0f;
    public float airMultiplier = 0.3f;
    public float runMultiplier = 1.5f;

    // Player Status
    public float playerHeight = 2.0f;
    public LayerMask whatIsGround;
    bool grounded;
    bool readyToJump = true;
    public float currentSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Keybinds
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0.0f;
        }

        if (Input.GetKey(sprintKey))
        {
            currentSpeed = baseSpeed * runMultiplier;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * 5.0f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * airMultiplier * 5.0f, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        // Limit velocity
        if(flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
