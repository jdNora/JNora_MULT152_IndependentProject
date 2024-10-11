using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10.0f;
    public float xBounds = -10.0f;
    private PlayerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerCtrl.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if(transform.position.x < xBounds && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
