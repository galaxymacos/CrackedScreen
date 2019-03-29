using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRun : MonoBehaviour {

    [SerializeField] private Vector3 knockOffForce;

    public void HomeRunHitPlayer() {
        print("Home run hits player");
        PlayerProperty.playerClass.TakeDamage(100);
        PlayerProperty.playerClass.GetKnockOff(knockOffForce);
    }
}
