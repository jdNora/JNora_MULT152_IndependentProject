using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TabletBehavior : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Canvas displayCanvas;
    [SerializeField] GameObject[] screens;
    GameObject currentScreen;

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
        if(playerController.tabletUp) // While Tablet Opened
        {
            if (animator.GetBool("TabletUp") == false) // On Tablet Open
            {
                audioSource.PlayOneShot(maximizeSFX);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                if (currentScreen != screens[0])
                {
                    SwitchScreen(0);
                }
            }
            animator.SetBool("TabletUp", true);
        }
        else // While Tablet Closed
        {
            if (animator.GetBool("TabletUp") == true) // On Tablet Close
            {
                audioSource.PlayOneShot(minimizeSFX);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
            animator.SetBool("TabletUp", false);
        }
    }

    public void SwitchScreen(int screen)
    {
        audioSource.PlayOneShot(clickSFX);
        currentScreen.SetActive(false);
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
                break;
            case 5:
                currentScreen = screens[5];
                break;
            case 6:
                currentScreen = screens[6];
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;
            default:
                currentScreen = screens[0];
                break;
        }
        currentScreen.SetActive(true);
    }
}
