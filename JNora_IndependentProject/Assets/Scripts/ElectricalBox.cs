using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBox : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private bool canRepair = false;

    private void Update()
    {
        if((canRepair) && Input.GetKeyDown(KeyCode.E))
        {
            attemptElectricalRepair();
        }
    }

    private void OnTriggerEnter(Collider other) // Optimize with Enter & Exit functions & bool
    {
        if(other.gameObject.CompareTag("Player"))
        {
            print("Player entered electrical.");
            canRepair = true;
        }
    }
    private void OnTriggerExit(Collider other) // Optimize with Enter & Exit functions & bool
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Player exited electrical.");
            canRepair = false;
        }
    }

    private void attemptElectricalRepair()
    {
        if(!gameManager.electricityWorking)
        {
            gameManager.GetComponent<GameManager>().electricityWorking = true;
            print("Power restored!");
        }
        else
        {
            print("It doesn't need to be repaired.");
        }
    }
}
