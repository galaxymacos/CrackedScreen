using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRun : MonoBehaviour {

    [SerializeField] private Vector3 knockOffForce;
    [SerializeField] private EnemyDetector homeRunHitBox;

    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HomeRunHitPlayer() {
        print("Home run hits player");
        animator.SetBool("DragonFistHitPlayer",false);
        if (homeRunHitBox.playerInRange())
        {
            PlayerProperty.playerClass.GetKnockOff(knockOffForce);
            PlayerProperty.playerClass.TakeDamage(100);
        }
    }
}
