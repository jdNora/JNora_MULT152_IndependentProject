using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerController : MonoBehaviour
{
    Rigidbody rbPlayer;
    GameObject focalPoint;
    Renderer rendererPlayer;

    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = gameObject.GetComponent<Rigidbody>();
        rendererPlayer = GetComponent<Renderer>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float magnitude = forwardInput * speed * Time.deltaTime;
        rbPlayer.AddForce(focalPoint.transform.forward * magnitude, ForceMode.Impulse);

        rendererPlayer.material.color = new Color(1.0f, 1.0f - Mathf.Abs(forwardInput), 0.0f);
    }
}
