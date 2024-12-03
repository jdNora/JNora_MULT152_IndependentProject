using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // NOTICE: THE PLAYER MOVEMENT USES THE RIGIDBODY FOR SMOOTH AND ACCURATE MOTION

    // Movement Values
    public float baseSpeed = 3.0f;
    public float runMultiplier = 1.75f;

    // Player Status
    public float playerHeight = 2.0f;
    float movementSpeed;

    public Boolean tabletUp = false;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRigidbody;
    AudioSource playerAudioSource;
    [SerializeField] GameObject playerObject;

    [SerializeField] AudioClip footstep;
    float stepRate = 0.6f;
    float stepCooldown = 0.0f;

    // Keybinds
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode tabletKey = KeyCode.Space;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        movementSpeed = baseSpeed;

        playerAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckInput();

        stepCooldown += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void CheckInput() // Checks for input
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            PlayFootsteps();
        }

        if (Input.GetKey(sprintKey))
        {
            movementSpeed = baseSpeed * runMultiplier;
            stepRate = 0.45f;
        }
        else
        {
            movementSpeed = baseSpeed;
            stepRate = 0.6f;
        }

        if (Input.GetKey(tabletKey))
        {
            Cursor.lockState = CursorLockMode.None;
            tabletUp = true;
        }
        else if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            tabletUp = false;
        }
    }

    private void PlayFootsteps()
    {
        if (stepCooldown > stepRate)
        {
            stepCooldown = 0;
            playerAudioSource.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
            playerAudioSource.PlayOneShot(footstep);
        }
    }
    
    private void MovePlayer() // Handles the player's movement
    {
        // Get movement direction and apply force
        if (verticalInput != 0 || horizontalInput != 0)
        {
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            playerRigidbody.AddForce(moveDirection.normalized * movementSpeed * 5.0f, ForceMode.Force);
            playerObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.1f;
            playerObject.GetComponent<CapsuleCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
        }
        // Stabilize when there's no input
        else
        {
            playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, new Vector3(0, playerRigidbody.velocity.y, 0), 0.2f);
            playerObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 1.0f;
            playerObject.GetComponent<CapsuleCollider>().material.frictionCombine = PhysicMaterialCombine.Average;
        }

        // Limits the player's forward and horizontal velocity to the intended speed
        Vector3 flatVel = new Vector3(playerRigidbody.velocity.x, 0.0f, playerRigidbody.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            playerRigidbody.velocity = new Vector3(limitedVel.x, playerRigidbody.velocity.y, limitedVel.z);
        }
    }
}
