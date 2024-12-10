using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 offset = new Vector3(0, 15, 0);

    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
