using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletBehavior : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController playerController;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.tabletUp)
        {
            animator.SetBool("TabletUp", true);
        }
        else
        {
            animator.SetBool("TabletUp", false);
        }
    }
}
