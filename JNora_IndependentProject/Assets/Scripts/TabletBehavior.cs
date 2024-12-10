using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TabletBehavior : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject playerCameraObject;
    public GameObject transition;
    public GameObject cameraOverlay;

    // Components
    GameManager gameManager;
    PlayerStatus playerStatus;
    PlayerController playerController;
    public Animator animator;
    public AudioSource audioSource;

    // Display Content
    [SerializeField] Canvas displayCanvas;
    [SerializeField] GameObject[] screens;
    [SerializeField] GameObject videoPanel;
    GameObject currentScreen;
    int currentScreenIndex;

    // Screen Content
    [SerializeField] GameObject vitalsText;
    [SerializeField] GameObject vitalsWarningIcon;
    [SerializeField] GameObject bodyTemperatureText;
    [SerializeField] GameObject bodyTemperatureWarningIcon;
    [SerializeField] GameObject temperatureText;
    [SerializeField] GameObject galleryContent;
    [SerializeField] GameObject galleryPhotoPrefab;
    [SerializeField] GameObject outpostNeedle;
    [SerializeField] GameObject objectiveNeedle;

    public AudioClip cameraShutterSFX;
    public AudioClip clickSFX;
    public AudioClip maximizeSFX;
    public AudioClip minimizeSFX;

    // Flags and Data
    public GameObject targetAnimal;
    public Transform objectiveTransform;
    public Transform outpostTransform;
    public bool canTakePhoto = false;
    public bool cameraModeActive = false;
    public bool compassModeActive = false;
    Sprite photoSprite;

    void Start()
    {
        currentScreen = screens[0];
        gameManager = gameManagerObject.GetComponent<GameManager>();
        playerStatus = playerObject.GetComponent<PlayerStatus>();
        playerController = playerObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        TabletActions();
        UpdateScreens();
    }

    void TabletActions()
    {
        // Tablet behavior handled in PlayerController for easier detection
        if (playerController.tabletUp) // While Tablet Opened
        {
            if (animator.GetBool("TabletUp") == false) // On Tablet Open
            {
                videoPanel.SetActive(true); // Save performance
                audioSource.PlayOneShot(maximizeSFX);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                if (currentScreen != screens[0])
                {
                    SwitchScreen(0);
                }
            }
            animator.SetBool("TabletUp", true);

            // Return To Menu
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(cameraModeActive)
                {
                    cameraModeActive = false;
                    canTakePhoto = false;
                    cameraOverlay.SetActive(false);
                    animator.SetBool("CameraMode", false);
                    playerController.FadeOutInTransition(2);
                    playerCameraObject.GetComponent<Camera>().fieldOfView = 70;
                }
                else if(compassModeActive)
                {
                    compassModeActive = false;
                    animator.SetBool("CompassMode", false);
                }
                else if(currentScreenIndex == 0)
                {
                    playerController.tabletUp = false;
                }
                SwitchScreen(0);
            }
        }
        else // While Tablet Closed
        {
            if (animator.GetBool("TabletUp") == true) // On Tablet Close
            {
                CloseTablet();
            }
        }
    }

    void UpdateScreens()
    {
        if (playerController.tabletUp) // Only while the tablet is up
        {
            switch (currentScreenIndex) // Tablet functions for Update()
            {
                case 0: // Main
                    break;

                case 1: // Temperature
                    if (currentScreen)
                        temperatureText.GetComponent<TextMeshProUGUI>().text = gameManager.tempTrend.ToString("F1") + "C";
                    break;

                case 2: // Status

                    vitalsText.GetComponent<TextMeshProUGUI>().text = playerStatus.vitals.ToString("F1") + "%";
                    if (playerStatus.vitals < 20)
                    {
                        vitalsWarningIcon.SetActive(true);
                    }
                    else
                    {
                        vitalsWarningIcon.SetActive(false);
                    }

                    bodyTemperatureText.GetComponent<TextMeshProUGUI>().text = playerStatus.bodyTemp.ToString("F1") + "C";
                    if (playerStatus.bodyTemp < 35)
                    {
                        bodyTemperatureWarningIcon.SetActive(true);
                    }
                    else
                    {
                        bodyTemperatureWarningIcon.SetActive(false);
                    }

                    break;

                case 3: // Gallery
                    break;

                case 4: // Camera
                    if (Input.GetMouseButtonDown(0) && canTakePhoto && cameraModeActive)
                    {
                        canTakePhoto = false;
                        cameraOverlay.SetActive(false);

                        Debug.Log("Click!");
                        audioSource.PlayOneShot(cameraShutterSFX);

                        StartCoroutine(TakePhoto());

                        canTakePhoto = true;
                    }
                    break;

                case 5: // Compass

                    Vector3 directionToOutpost = outpostTransform.position - transform.position;
                    directionToOutpost.y = 0f; // Ignore height differences
                    float outpostAngle = Mathf.Atan2(directionToOutpost.x, directionToOutpost.z) * Mathf.Rad2Deg;
                    outpostNeedle.transform.rotation = Quaternion.Euler(90f, 0f, -outpostAngle);

                    Vector3 directionToObjective = objectiveTransform.position - transform.position;
                    directionToObjective.y = 0f; // Ignore height differences
                    float objectiveAngle = Mathf.Atan2(directionToObjective.x, directionToObjective.z) * Mathf.Rad2Deg;
                    objectiveNeedle.transform.rotation = Quaternion.Euler(90f, 0f, -objectiveAngle);

                    break;

                case 6: // Objectives
                    break;

                case 7: // Help
                    break;

                default: // Also Main
                    break;
            }
        }
    }

    private IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        // Fire Raycast For Objective (Must Be Within 20 Meters)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerObject.transform.forward, out hit, 20f))
        {
            if(hit.collider.gameObject == targetAnimal)
            {
                // Complete objective
                Debug.Log("Target photographed!");
            }
            Debug.Log("Target undetected...");
        }
        else
        {
            Debug.Log("Target undetected...");
        }

        // Initial Capture
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        // Correction
        Texture2D photoTexture = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        photoTexture.SetPixels(screenshot.GetPixels());
        photoTexture.Apply();
        
        // Free Memory
        Destroy(screenshot);
        
        // Add To Gallery
        Sprite photoSprite = Sprite.Create(photoTexture, new Rect(0f, 0f, photoTexture.width, photoTexture.height), new Vector2(0.5f, 0.5f));
        GameObject newGalleryPhoto = Instantiate(galleryPhotoPrefab, galleryContent.transform);
        newGalleryPhoto.GetComponent<UnityEngine.UI.Image>().sprite = photoSprite;

        // Change Camera Back
        cameraOverlay.SetActive(true);
    }

    public void SwitchScreen(int screen)
    {
        audioSource.PlayOneShot(clickSFX);
        foreach (var s in screens)
        {
            s.SetActive(false);
        }
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        currentScreenIndex = screen;
        switch (screen) // For screen initialization and custom transition behavior
        {
            case 0: // Main

                break;

            case 1: // Temperature

                break;

            case 2: // Status

                break;

            case 3: // Gallery

                break;

            case 4: // Camera
                cameraModeActive = true;
                animator.SetBool("CameraMode", true);
                playerController.FadeOutInTransition(1);
                playerController.inputEnabled = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;

            case 5: // Compass
                compassModeActive = true;
                animator.SetBool("CompassMode", true);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;

            case 6: // Objectives

                break;

            case 7: // Help

                break;

            default: // Also Main

                break;
        }
        currentScreen = screens[currentScreenIndex];
        Debug.Log("Current screen: " + currentScreen.name);
        currentScreen.SetActive(true);
    }

    void CloseTablet()
    {
        videoPanel.SetActive(false);
        audioSource.PlayOneShot(minimizeSFX);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        animator.SetBool("TabletUp", false);
    }
}
