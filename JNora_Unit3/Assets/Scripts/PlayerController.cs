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


    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }


    void Update()
    {
        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
        if (spaceDown && onGround)
        {
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
        }
    }
}
