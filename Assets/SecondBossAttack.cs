using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossAttack : MonoBehaviour
{
    [SerializeField] private EnemyDetector[] strikeHitBoxs;
    // Start is called before the first frame update

    public void BasicAttackHitPlayer(int attackIndex)
    {
        if (strikeHitBoxs[attackIndex].playerInRange())
        {
            if (attackIndex == 5)     // If it is the last strike of the basic attack, deal extra damage and knock up player
            {
                PlayerProperty.playerClass.TakeDamage(30);
                PlayerProperty.playerClass.GetKnockOff(transform.position);
            }
            else
            {
                PlayerProperty.playerClass.TakeDamage(10);
            }
        }
    }
}
