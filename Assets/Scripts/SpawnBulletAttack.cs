using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletAttack : SpawnAttack
{

    public override void SpawnObjInteraction(GameObject obj)
    {
        foreach (var bulletScript in obj.GetComponentsInChildren<Bullet>())
        {
            bulletScript.flyingDirection = GameManager.Instance.player.transform.position;
        }
    }
}
