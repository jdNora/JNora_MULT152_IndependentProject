using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureZone : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerStatus playerStatus;

    private void OnTriggerEnter(Collider other) // Optimize with Enter & Exit functions & bool
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("WarmZone"))
            {
                playerStatus.inWarmZone = true;
                Debug.Log("Entered Warm Zone");
            }
            else if (gameObject.CompareTag("ColdZone"))
            {
                playerStatus.inColdZone = true;
                Debug.Log("Entered Cold Zone");
            }
        }
    }
    private void OnTriggerExit(Collider other) // Optimize with Enter & Exit functions & bool
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("WarmZone"))
            {
                playerStatus.inWarmZone = false;
                Debug.Log("Left Warm Zone");
            }
            else if (gameObject.CompareTag("ColdZone"))
            {
                playerStatus.inColdZone = false;
                Debug.Log("Left Cold Zone");
            }
        }
    }
}
