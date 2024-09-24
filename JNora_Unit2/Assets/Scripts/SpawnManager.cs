using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    private float xPosRange = 18;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float randXPos = Random.Range(-xPosRange, xPosRange);
        int animalPrefabIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 randPos = new Vector3(randXPos, 0, 20);
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(animalPrefabs[animalPrefabIndex], randPos, animalPrefabs[animalPrefabIndex].transform.rotation);
        }
    }
}
