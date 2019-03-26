using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerMovement _playerMovement;
    [SerializeField] private Skill[] playerSkills;

    private float counterAttackTimeRemains;
    private float counterAttackActivationDuration = 0.5f;


    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_playerController.canControl)
        {
            if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Stand ||
                _playerMovement.playerCurrentState == PlayerMovement.PlayerState.Walk) // If player is on ground
            {
                if (Input.GetKeyDown(KeyCode.K))
                    playerSkills[0].Play();

                else if (Input.GetKeyDown(KeyCode.L))
                    playerSkills[1].Play();
                else if (Input.GetKeyDown(KeyCode.U)) playerSkills[3].Play();
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    playerSkills[4].Play();
                }
            }

            if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Jump ||
                _playerMovement.playerCurrentState == PlayerMovement.PlayerState.DoubleJump ||
                _playerMovement.playerCurrentState == PlayerMovement.PlayerState.FallDown) // If player is on air
                if (Input.GetKeyDown(KeyCode.J))
                {
                    playerSkills[2].Play();
                }

            
        }
        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block && counterAttackTimeRemains > 0)
        {
            counterAttackTimeRemains -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerSkills[5].Play();
                counterAttackTimeRemains = 0;
//                    GameManager.Instance.animator.SetTrigger("CounterAttack");
            }
        }
    }

    public void EnterCounterAttackMode()
    {
        counterAttackTimeRemains = counterAttackActivationDuration;
    }
}