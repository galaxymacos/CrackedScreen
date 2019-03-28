using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumEnemy : Enemy
{

    [SerializeField] private HingeJoint _hingeJoint;

    private bool fullCharge;

    [SerializeField] private EnemyDetector enemyDetector;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    protected override void Die()
    {
//        Destroy(transform.parent.parent.gameObject);
    }

    private bool hasKnockedOffPlayer;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (fullCharge && enemyDetector.playerInRange() && !hasKnockedOffPlayer)
        {
            hasKnockedOffPlayer = true;
            PlayerProperty.playerClass.GetKnockOff(transform.position,new Vector3(300,200,0));
        }
    }

    public override void Move()
    {
        return;
    }

    public override bool AnimationPlaying()
    {
        return false;
    }

    public override void InteractWithPlayer()
    {
        
    }

    public override void ForceMove(Vector3 force) // 
    {
        return;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        print("pendulum took damage");
        ChangeAngleBasedOnCurrentHp();
    }

    private void ChangeAngleBasedOnCurrentHp()
    {
        var hingeJointMotor = _hingeJoint.motor;

        if (HP <= 0)
        {
            hingeJointMotor.targetVelocity = 1000;
            fullCharge = true;

        }
        else if ((double) HP / maxHp < 0.25)
        {
            hingeJointMotor.targetVelocity = 600;

        }
        else if ((double) HP / maxHp < 0.5)
        {
            hingeJointMotor.targetVelocity = 300;

        }
        else if ((double) HP / maxHp < 0.75)
        {
            print("change hinge joint target velocity");
            hingeJointMotor.targetVelocity = 100;
        }

        _hingeJoint.motor = hingeJointMotor;
    }
}
