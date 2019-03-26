using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Skill
{
    [SerializeField] private float blockingDuration = 0.3f;

    private void FixedUpdate()
    {
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }

        if (_isPlaying)
        {
            if (TimePlayed + blockingDuration <= Time.time)
            {
                playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Stand);
                print("Change player state in blocking function");
                
//                playerController.canControl = true;
                _isPlaying = false;
            }
        }
    }

    public override void Play()
    {
        if (_skillNotOnCooldown)
        {
            GameManager.Instance.animator.SetTrigger("Defend");
//            playerController.canControl = false;
            base.Play();
            _skillNotOnCooldown = false; // Skill is on cooldown
            playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Block);
            GameManager.Instance.player.GetComponent<PlayerCombat>().EnterCounterAttackMode();
        }
        else
        {
            print("Blocking is on cooldown");
        }
    }
}