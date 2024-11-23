using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rbEnemy;
    GameObject player;

    public float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 seekDirection = (player.transform.position - transform.position).normalized;
        rbEnemy.AddForce(seekDirection * speed * Time.deltaTime, ForceMode.Impulse);
    }
}
