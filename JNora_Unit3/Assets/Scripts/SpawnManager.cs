using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerCtrl;
    public GameObject obsPrefab;
    Vector3 spawnPos = new Vector3(30, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObs", 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl.gameOver)
        {
            CancelInvoke("SpawnObs");
        }
    }

    void SpawnObs()
    {
        Instantiate(obsPrefab, spawnPos, obsPrefab.transform.rotation);
    }
}