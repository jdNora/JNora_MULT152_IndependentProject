using CodiceApp;
using PlasticGui.WorkspaceWindow.BrowseRepository;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject tabletObject;

    [SerializeField] Material[] skyboxes;
    [SerializeField] Light sunlight;
    [SerializeField] ParticleSystem snowfall;

    public GameObject[] wildlifeSpawns;
    public GameObject researcherComputer;
    public GameObject targetAnimal;
    public AnimalBehavior animalBehavior;

    public int currentHour = 6;
    public int currentMinute = 0;
    public float tempTrend = 37.0f; // Temperature (Celcius) the player's body trends towards.

    public int objectivesCompleted = 0;
    public bool electricityWorking = true;
    public float failChance = 20.0f;

    void Start()
    {
        // Prepare gameplay scene

        SpawnWildlife();
        Debug.Log("Current wildlife index: " + animalBehavior.currentAnimalIndex);

        InvokeRepeating("UpdateTime", 0, 1);
    }

    void Update()
    {
        if(!playerObject.GetComponent<PlayerStatus>().conscious)
        {
            // Trigger faint animation and time pass events
        }
    }

    public void CompleteObjective()
    {
        objectivesCompleted++;
        if (objectivesCompleted >= 3)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadSceneAsync("Win");
        }
        SpawnWildlife();
    }

    private void SpawnWildlife()
    {
        GameObject selectedSpawnLocation = wildlifeSpawns[Random.Range(0, wildlifeSpawns.Length)];
        targetAnimal.transform.position = selectedSpawnLocation.transform.position;
        tabletObject.GetComponent<TabletBehavior>().objectiveTransform = selectedSpawnLocation.transform;
        animalBehavior.NextAnimal();

    }

    private void UpdateTime() // Fires every 1 IRL seconds = 1 Ingame minute (gameplay tick)
    {
        // Time flow
        if(currentMinute < 59)
        {
            currentMinute += 1;
        }
        else // Fires every hour
        {
            currentHour += 1;
            currentMinute = 0;
            ElectricalUpdate();
        }

        // Environment Effects
        var main = snowfall.main;
        var shape = snowfall.shape;
        float hoursPassedPlusOne = ((currentHour + (currentMinute / 60.0f)) - 5.0f);

        main.simulationSpeed = 1 + (hoursPassedPlusOne * 0.3f);
        shape.radius = 100 - (hoursPassedPlusOne * 5);
        RenderSettings.fogDensity = 0.03f + 0.005f * hoursPassedPlusOne;
        if (currentHour >= 12 && currentHour <= 20)
        {
            RenderSettings.skybox = skyboxes[Mathf.RoundToInt(currentHour - 12)];
        }
        sunlight.intensity = 0.5f - (0.5f * (hoursPassedPlusOne / 6));

        // Gameplay Effects
        tempTrend = 37.0f - (hoursPassedPlusOne - 1.0f);

        // [ EVENT TIMELINE ]
        // 0600 Game starts
        // 2400 Game ends
    }

    private void ElectricalUpdate()
    {
        // Electricity
        if(electricityWorking)
        {
            float roll = Random.value * 100;
            if (roll > failChance) // Succeed
            {
                failChance += 20;
            }
            else // Fail
            {
                failChance = 20;
                electricityWorking = false;
            }
        }
    }
}
