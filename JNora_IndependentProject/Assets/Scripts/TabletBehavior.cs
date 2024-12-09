using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;
using TMPro;

public class TabletBehavior : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject player;

    // Display Content
    [SerializeField] Canvas displayCanvas;
    [SerializeField] GameObject[] screens;
    [SerializeField] GameObject videoPanel;
    GameObject currentScreen;

    // Screen Content
    [SerializeField] GameObject temperatureText;

    // Components
    PlayerController playerController;
    Animator animator;
    AudioSource audioSource;

    public AudioClip clickSFX;
    public AudioClip maximizeSFX;
    public AudioClip minimizeSFX;

    void Start()
    {
        currentScreen = screens[0];
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        TabletActions();

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

            // Return to menu
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchScreen(0);
            }
        }
        else // While Tablet Closed
        {
            if (animator.GetBool("TabletUp") == true) // On Tablet Close
            {
                videoPanel.SetActive(false);
                audioSource.PlayOneShot(minimizeSFX);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
            animator.SetBool("TabletUp", false);
        }
    }

    void UpdateScreens()
    {
        // Weather screen
    }

    public void SwitchScreen(int screen)
    {
        audioSource.PlayOneShot(clickSFX);
        foreach (var s in screens)
        {
            s.SetActive(false);
        }
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        switch (screen)
        {
            case 0:
                currentScreen = screens[0];
                break;
            case 1:
                currentScreen = screens[1];
                break;
            case 2:
                currentScreen = screens[2];
                break;
            case 3:
                currentScreen = screens[3];
                break;
            case 4:
                currentScreen = screens[4];
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;
            case 5:
                currentScreen = screens[5];
                break;
            case 6:
                currentScreen = screens[6];
                break;
            case 7:
                currentScreen = screens[7];
                break;
            default:
                currentScreen = screens[0];
                break;
        }
        Debug.Log("Current screen: " + currentScreen.name);
        currentScreen.SetActive(true);
    }
}
