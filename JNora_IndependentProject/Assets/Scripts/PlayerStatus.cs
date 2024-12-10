using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    /* Note on Body Temperature
     * Normal > 36C
     * Cold > 35C
     * Mild Hypothermia > 32C
     * Hypothermia > 30C
     * Severe Hypothermia < 30C
    */

    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject tabletObject;
    [SerializeField] GameObject playerModel;
    PlayerController playerController;
    Animator animator;

    public bool conscious = true;
    public bool inWarmZone = false;
    public bool inColdZone = false;
    public float vitals = 100.0f;
    public float bodyTemp = 37.0f;

    void Start()
    {
        InvokeRepeating("UpdateBodyTemp", 0, 0.25f);
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vitals <= 20)
        {
            playerController.canRun = false;
            if (vitals <= 0)
            {
                PlayerFaint();
            }
        }
        else
        {
            playerController.canRun = true;
        }
    }

    

    private void UpdateBodyTemp()
    {
        float targetTemp;
        if (inWarmZone)
        {
            targetTemp = 37.0f;
        }
        else if (inColdZone)
        {
            targetTemp = gameManager.GetComponent<GameManager>().tempTrend - 5.0f;
        }
        else
        {
            targetTemp = gameManager.GetComponent<GameManager>().tempTrend;
        }

        // Adjust Temp
        bodyTemp = Mathf.Lerp(bodyTemp, targetTemp, 0.02f);
        if (Mathf.Abs(bodyTemp - Mathf.Round(bodyTemp)) < 0.01f)
        {
            bodyTemp = Mathf.Round(bodyTemp);
        }

        // Vitals Loss
        float energyDrain = (35 - bodyTemp) * 0.05f;
        if (bodyTemp <= 35)
        {
            vitals -= energyDrain;
        }
    }

    void PlayerFaint()
    {
        Debug.Log("Player fainted...");
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync("Lose");

        playerController.inputEnabled = false;
        conscious = false; // Game manager detects this and triggers game-wide events
    }
}
