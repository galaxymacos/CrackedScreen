using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenLockPlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;


    private void OnEnable()
    {
        playerController.canControlDialogueBox = false;
    }

    private void OnDisable()
    {
        playerController.canControlDialogueBox = true;
    }
}
