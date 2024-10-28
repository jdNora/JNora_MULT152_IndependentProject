using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool gameOver = false;
    public float gravityModifier = 2.0f;
    public float jumpForce = 10.0f;
    bool onGround = true;
    Rigidbody rbPlayer;
    Animator animPlayer;

    // Effects
    public ParticleSystem expSystem;
    public ParticleSystem dirtSystem;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    AudioSource asPlayer;

    void Start()
    {
        Physics.gravity *= gravityModifier;
        rbPlayer = GetComponent<Rigidbody>();
        animPlayer = GetComponent<Animator>();
        asPlayer = GetComponent<AudioSource>();
    }


    void Update()
    {
        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
        if (spaceDown && onGround && !gameOver)
        {
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            animPlayer.SetTrigger("Jump_trig");
            dirtSystem.Stop();
            asPlayer.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            dirtSystem.Play();
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animPlayer.SetBool("Death_b", true);
            animPlayer.SetInteger("DeathType_int", 1);
            expSystem.Play();
            dirtSystem.Stop();
            asPlayer.PlayOneShot(crashSound, 1.0f);
        }
    }
}
