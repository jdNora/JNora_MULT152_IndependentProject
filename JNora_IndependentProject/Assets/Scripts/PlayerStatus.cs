using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float energy = 100.0f;
    public float bodyTemp = 37.0f;

    void Start()
    {
        InvokeRepeating("UpdateBodyTemp", 0, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if(energy <= 0)
        {
            print("You succumbed to the cold...");
            Application.Quit();
        }
    }

    private void UpdateBodyTemp()
    {
        // Adjust Temp
        bodyTemp = Mathf.Lerp(bodyTemp, gameManager.GetComponent<GameManager>().outdoorTemp, 0.02f);
        if (Mathf.Abs(bodyTemp - Mathf.Round(bodyTemp)) < 0.01f)
        {
            bodyTemp = Mathf.Round(bodyTemp);
        }


        // Energy Loss
        float energyDrain = (35 - bodyTemp) * 0.05f;
        if (bodyTemp <= 35)
        {
            energy -= energyDrain;
        }
    }
}
