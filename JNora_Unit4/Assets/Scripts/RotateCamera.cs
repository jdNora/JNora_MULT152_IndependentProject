using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float speed = 5000.0f;

    // Update is called once per frame
    void Update()
    {
        float rotateInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, -rotateInput * speed * Time.deltaTime);
    }
}
