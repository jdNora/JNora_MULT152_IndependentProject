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

    public Boolean inputEnabled = true;
    public Boolean canRun = true;
    public Boolean tabletUp = false;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRigidbody;
    AudioSource playerAudioSource;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject tabletObject;
    TabletBehavior tabletBehavior;
    public GameObject transition;

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
        tabletBehavior = tabletObject.GetComponent<TabletBehavior>();
    }

    private void Update()
    {
        CheckInput();

        stepCooldown += Time.deltaTime;

        if(!tabletUp)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    int transitionContext = 0;
    IEnumerator PostCameraTransition() // For behavior AFTER the transition effect
    {
        yield return new WaitForSeconds(2);

        switch (transitionContext)
        {
            case 1:
                tabletBehavior.canTakePhoto = true;
                tabletBehavior.cameraOverlay.SetActive(true);
                playerCam.GetComponent<Camera>().fieldOfView = 50;
                break;

            case 2:
                tabletObject.GetComponent<TabletBehavior>().SwitchScreen(0);
                transition.SetActive(false);
                inputEnabled = true;
                break;

            case 3:

                break;

            default:

                break;
        }
    }

    public void FadeOutInTransition(int context) // 1 = Camera Mode In // 2 = Camera Mode Out // 3 = Faint
    {
        transitionContext = context;
        inputEnabled = false;

        transition.SetActive(true);

        if (transitionContext == 1 || transitionContext == 2)
        {
            transition.GetComponent<Animator>().SetTrigger("FadeOutIn");
            StartCoroutine(PostCameraTransition());
        }
        else if (transitionContext == 3)
        {
            transition.SetActive(true);
            transition.GetComponent<Animator>().SetTrigger("FadeOut");
        }
    }

    private void CheckInput() // Checks for input
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (inputEnabled)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                PlayFootsteps();
            }

            if (Input.GetKey(sprintKey) && canRun)
            {
                movementSpeed = baseSpeed * runMultiplier;
                stepRate = 0.45f;
            }
            else
            {
                movementSpeed = baseSpeed;
                stepRate = 0.6f;
            }

            if (Input.GetKeyDown(tabletKey))
            {
                if (!tabletObject.GetComponent<TabletBehavior>().cameraModeActive && !tabletObject.GetComponent<TabletBehavior>().compassModeActive)
                {
                    tabletUp = !tabletUp;
                }
            }
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
        if (inputEnabled && (verticalInput != 0 || horizontalInput != 0))
        {
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            playerRigidbody.AddForce(moveDirection.normalized * movementSpeed * 5.0f, ForceMode.Force);
            playerModel.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.1f;
            playerModel.GetComponent<CapsuleCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
        }
        // Stabilize when there's no input
        else
        {
            playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, new Vector3(0, playerRigidbody.velocity.y, 0), 0.2f);
            playerModel.GetComponent<CapsuleCollider>().material.dynamicFriction = 1.0f;
            playerModel.GetComponent<CapsuleCollider>().material.frictionCombine = PhysicMaterialCombine.Average;
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
