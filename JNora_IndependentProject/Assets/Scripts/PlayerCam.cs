using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // NOTICE: THE CAMERA MOVEMENT IS HANDLED SEPARATE FROM THE PLAYER AS A FIX FOR JITTERY VISUALS

    public float sensX = 1000.0f;
    public float sensY = 1000.0f;
    public float smoothing = 0.5f;

    [SerializeField] Transform player;

    // Camera orientation
    float yaw;
    float pitch;

    // Camera rotational velocity
    float xVel = 0.0f;
    float yVel = 0.0f;

    void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            // Get mouse direction
            xVel = Mathf.Lerp(xVel, Input.GetAxis("Mouse X"), 0.1f / smoothing);
            yVel = Mathf.Lerp(yVel, Input.GetAxis("Mouse Y"), 0.1f / smoothing);

            // Apply look force
            float mouseX = xVel * sensX * Time.deltaTime;
            float mouseY = yVel * sensY * Time.deltaTime;

            // Change recorded values
            pitch += mouseX;
            yaw -= mouseY;
            yaw = Mathf.Clamp(yaw, -80.0f, 80.0f);

            // Adjust actual orientation
            transform.rotation = Quaternion.Euler(yaw, pitch, 0);
            player.rotation = Quaternion.Euler(0, pitch, 0);
        }
    }
}
