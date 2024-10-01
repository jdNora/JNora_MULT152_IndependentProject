using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject spawnpointObjects;
    public Transform[] spawnpoints;

    public int currentHour = 6;
    public int currentMinute = 0;
    public float outdoorTemp = 37.0f; // Temperature the player's body trends towards.

    public bool electricityWorking = true;
    public float failChance = 20.0f;

    public GameObject player;

    void Start()
    {
        // Prepare gameplay scene
        //spawnpointObjects.GetComponentInChildren   FIX THIS

        // Initiate Cutscene

        // Begin normal gameplay functions
        InvokeRepeating("UpdateTime", 0, 4);

    }

    private void SpawnWildlife()
    {

    }

    private void UpdateTime() // 4 IRL seconds = 1 Ingame minute (gameplay tick)
    {
        if(currentMinute < 59)
        {
            currentMinute += 1;
        }
        else
        {
            currentHour += 1;
            currentMinute = 0;
            HourlyUpdate();
        }
    }

    private void HourlyUpdate()
    {
        // Electricity
        if(electricityWorking)
        {
            float roll = Random.value * 100;
            if (roll > failChance) // Succeed
            {
                failChance += 20;
                print("The outpost's electrical power is deteriorating...");
            }
            else // Fail
            {
                failChance = 20;
                electricityWorking = false;
                print("The outpost's electrical power went out.");
            }
        }
    }
}
