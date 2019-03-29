using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class SecondStageBoss : Enemy
{
    public bool moveTowardsPlayer;
    private float moveTimeRemainsThisRound;
    private Animator animator;

    public BossAbility[] BossAbilities;

    [SerializeField] private float ignoreKnockUpTime = 3f;    // Enemy can't be knocked up for seconds after boss just stand up from lying 
    private float ignoreKnockUpTimeLeft;
    [SerializeField] private int MaxknockUpTimes = 3;    // Enemy can't be knock up more than this number in a row
    private int currentKnockUpTimes;
    public delegate void OnBossDie();

    public OnBossDie OnBossDieCallback;
    [SerializeField] private EnemyDetector autoAttackRange;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        OnChangeEnemyStateCallback += AnimateEnemy;
        specialAttackTimeRemains = specialAttackInterval;
        base.Start();
    }

    public override void KnockUp(Vector3 force)
    {
        // Boss can't be knocked up for more than several times
        if (currentKnockUpTimes >= MaxknockUpTimes )
        {
            return;
        }

        if (ignoreKnockUpTimeLeft > 0f)
        {
            return;
        }
        currentKnockUpTimes++;
        base.KnockUp(force);
    }

    public override void StandUp()
    {
        base.StandUp();
        ignoreKnockUpTimeLeft = ignoreKnockUpTime;
        currentKnockUpTimes = 0;
    }

    protected override void Die()
    {
        OnBossDieCallback?.Invoke();
        Destroy(gameObject);

    }

    public override void Update()
    {
        base.Update();
        animator.SetBool("AnimationPlaying", AnimationPlaying());
        

    }

// Start is called before the first frame update

    public override bool AnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("RollingStrike") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("DashUppercut") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("HomeRun") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("LayOnGround") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("AirFruitNinja") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("BaseballAttack");
    }

    public float specialAttackInterval = 10f;
    public float specialAttackTimeRemains;
    
    public override void InteractWithPlayer()
    {
        if (ignoreKnockUpTimeLeft > 0)
        {
            ignoreKnockUpTimeLeft -= Time.deltaTime;
        }
        if (StiffTimeRemain <= 0 & _enemyCurrentState == EnemyState.Standing)
        {
            if (autoAttackRange.playerInRange())
            {
                if (attackCooldownUp() && !AnimationPlaying())
                {
                    rb.velocity = new Vector3(0,rb.velocity.y,0);
                    animator.SetTrigger("Attack");

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
            }

            SpecialAttack();
        }

        animator.SetFloat("HorizontalVelocity",rb.velocity.x);
    }

    public bool CanMove()
    {
        return !isStiffed && !AnimationPlaying() && _enemyCurrentState == EnemyState.Standing;
    }

    private void SpecialAttack()
    {
        FaceBasedOnPlayerPosition();
        specialAttackTimeRemains -= Time.deltaTime;
        if (specialAttackTimeRemains <= 0)
        {
            
            if (!AnimationPlaying())
            {
                specialAttackTimeRemains = specialAttackInterval;
                int randomAbilityIndex = Random.Range(0, BossAbilities.Length);
                    BossAbilities[randomAbilityIndex].Play();
            }
        }
    }

    private bool attackCooldownUp()
    {
        return Time.time >= nextAttackTime;
    }

    [SerializeField] private EnemyDetector playerInAttackRangeDetector;
    private bool playerInAttackRange => playerInAttackRangeDetector.playerInRange();
    
    private void FixedUpdate()
    {
        if (CanMove())
        {
            Move();
            FaceBasedOnMoveDirection();
        }
        else
        {
            FaceBasedOnPlayerPosition();
        }
 
    }

    

    /// <summary>
    /// Was called in FixedUpdate()
    /// </summary>
    public override void Move()
    {
            
            if (!playerInAttackRange)
            {
                rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed,rb.velocity.y,PlayerDirectionInPlane().z*moveSpeed);
            }
    }

    /// <summary>
    /// This method is to calculate the distance between player and enemy (ignore height difference)
    /// </summary>
    /// <returns>The distance of player and enemy </returns>
    private Vector3 PlayerDirectionInPlane()
    {
        Vector3 playerDirection = (GameManager.Instance.player.transform.position - transform.position).normalized;
       return new Vector3(playerDirection.x,0,playerDirection.z);
    }

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            moveTowardsPlayer = !moveTowardsPlayer;
        } 
    }

    public void AnimateEnemy(EnemyState enemyState)
    {
        switch (_enemyCurrentState)
        {
            case EnemyState.Standing:
                animator.SetBool("Stand",true);
                break;
            case EnemyState.GotHitToAir:
                animator.SetTrigger("HitToAir");
                break;
            case EnemyState.LayOnGround:
                animator.SetTrigger("LayOnGround");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
