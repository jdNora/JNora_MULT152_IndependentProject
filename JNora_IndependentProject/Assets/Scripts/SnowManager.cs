using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 offset = new Vector3(0, 15, 0);

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
