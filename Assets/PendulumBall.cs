using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumBall : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            PlayerProperty.playerClass.TakeDamage(damage);
            PlayerProperty.playerClass.GetKnockOff(transform.TransformDirection(transform.position));
        }
    }
}
