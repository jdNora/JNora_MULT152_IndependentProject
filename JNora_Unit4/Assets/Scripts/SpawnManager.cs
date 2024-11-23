using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    float spawnRange = 8.5f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPos = GenerateSpawnPos();
        Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
        // InvokeRepeating("SpawnEnemy", 1, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 GenerateSpawnPos()
    {
        float xPos = Random.Range(-spawnRange, spawnRange);
        float zPos = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(xPos, enemyPrefab.transform.position.y, zPos);
        // Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
        return spawnPos;
    }
}
