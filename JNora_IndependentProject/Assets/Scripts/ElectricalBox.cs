using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBox : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private void OnTriggerStay(Collider other) // Optimize with Enter & Exit functions & bool
    {
        if (Input.GetKey(KeyCode.E) && other == gameManager.GetComponent<GameManager>().player)
        {
            print("Power restored!");
            gameManager.GetComponent<GameManager>().electricityWorking = true;
        }
    }
}
