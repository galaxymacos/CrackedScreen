using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private static readonly int Die = Animator.StringToHash("DIE");

    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
    }

    public void LockPlayerControl()
    {
        print("Lock player control");
        playerController.canControl = false;
    }

    public void ReleasePlayerControl()
    {
        print("release player control");
        playerController.canControl = true;
    }

    public void PlayerStartDying()
    {
        playerController.canControl = false;
        if (!GameManager.Instance.PlayerDying)
        {
            GameManager.Instance.PlayerDying = true;
            PlayerProperty.animator.SetTrigger(Die);
        }
        AudioManager.instance.ForceStopAllSfx();
        AudioManager.instance.PlaySfx("Die");
    }

    public void PlayerDie()
    {
        GameManager.Instance.onPlayerDieCallback?.Invoke();
        GameManager.Instance.PlayerDying = false;

        playerController.canControl = true;
    }

}