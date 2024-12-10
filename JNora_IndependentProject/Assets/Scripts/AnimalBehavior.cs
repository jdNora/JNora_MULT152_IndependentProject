using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] animals;
    [SerializeField] Transform playerCamTransform;
    [SerializeField] TabletBehavior tabletBehavior;
    [SerializeField] GameObject spriteObject;
    SpriteRenderer animalSpriteRenderer;
    public int currentAnimalIndex = 8; // Runs once on startup to begin with index 0

    void Start()
    {
        animalSpriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        animalSpriteRenderer.sprite = animals[currentAnimalIndex];
    }

    void Update()
    {
        if (tabletBehavior.cameraModeActive)
        {
            spriteObject.SetActive(true);
            transform.LookAt(playerCamTransform.position);

            float distance = (playerCamTransform.position - transform.position).magnitude;
            print("Before clamp: " + distance);
            distance = Mathf.Clamp(distance, 0, 20);
            print("After clamp: " + distance);

            Color tempColor = animalSpriteRenderer.color;
            print("Temp color: " + tempColor);
            tempColor.a = (255 - (distance / 20) * 255);
            animalSpriteRenderer.color = tempColor;
        }
        else
        {
            spriteObject.SetActive(false);
        }
    }

    public void NextAnimal()
    {
        if (currentAnimalIndex < animals.Length)
        {
            currentAnimalIndex++;
        }
        else
        {
            currentAnimalIndex = 0;
        }

        animalSpriteRenderer.sprite = animals[currentAnimalIndex];
    }
}
