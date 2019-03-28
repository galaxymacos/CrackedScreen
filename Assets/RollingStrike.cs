using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStrike : BossAbility
{
    private Animator animator;

    [SerializeField] private EnemyDetector RollingStrikeHitBoxPlayerDetector;

    [SerializeField] private float jumpForce = 1000f;
    private bool startJumping;
    private float gravity = 4000f;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (startJumping)
        {
            _rigidbody.AddForce(0,-gravity*Time.fixedDeltaTime,0);
            transform.position = Vector3.Lerp(transform.position,PlayerProperty.playerPosition,0.02f);
        }

    }

    public override void Play()
    {
        animator.SetTrigger("RollingStrike");
    }

    public void HitPlayerIfItIsInHitBox()
    {
        startJumping = false;
        if (RollingStrikeHitBoxPlayerDetector.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(20);
            PlayerProperty.playerClass.GetKnockOff(transform.position);
        }
    }

    public void RollingStrikeInitialized()
    {
        JumpOnPlayer();
    }
    public void JumpOnPlayer()
    {
        _rigidbody.AddForce(0,jumpForce,0);
        startJumping = true;
    }
}
