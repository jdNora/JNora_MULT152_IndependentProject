using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Controller Speeds
    private float speed = 4.0f;
    private float mouseSensitivity = 500.0f;
    // Input
    private float verticalInput;
    private float horizontalInput;
    private float mouseX;
    private float mouseY;
    // Fields
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        Vector3 moveVector = Vector3.Normalize(new Vector3(horizontalInput, 0 , verticalInput));
        transform.Translate(moveVector * speed * Time.deltaTime);
        transform.Rotate(0, mouseX * mouseSensitivity * Time.deltaTime, 0);
        cam.transform.Rotate(-mouseY * mouseSensitivity * Time.deltaTime, 0, 0);
    }
}
