using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class BossEnemy : Enemy
{
    public float movingTowardsPlayerPercentage = 0.7f;

    public bool moveTowardsPlayer;

    private float moveTimeRemainsThisRound;

    public float moveTimeInARow = 3f;

    private Animator animator;

    public BossAbility[] BossAbilities;
    public string[] specialAttackAnimationNames;

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
    
    
// Start is called before the first frame update
    private bool canMove;
    private bool isplayingAnimation;

    public void LockEnemyMove()
    {
        isplayingAnimation = true;
    }

    public void ReleaseEnemyMove()
    {
        isplayingAnimation = false;
    }


    private bool animationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("RollingAttack") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousStrike");
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
                if (attackCooldownUp() && !animationPlaying())
                {
                    rb.velocity = new Vector3(0,rb.velocity.y,0);
                    animator.SetTrigger("Attack");

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
            }

            SpecialAttack();
        }
        
        
        

        if (!animationPlaying())
        {
            ReleaseEnemyMove();
            ChangeFacing(rb.velocity.x);
        }
        else
        {
            LockEnemyMove();

        }
        canMove = !isStiffed && !isplayingAnimation;
        
        animator.SetFloat("HorizontalVelocity",rb.velocity.x);
    }

    private void SpecialAttack()
    {
        specialAttackTimeRemains -= Time.deltaTime;
        if (specialAttackTimeRemains <= 0)
        {
            
            if (!animationPlaying())
            {
                specialAttackTimeRemains = specialAttackInterval;
                int randomAbilityIndex = Random.Range(0, specialAttackAnimationNames.Length);
                animator.SetTrigger(specialAttackAnimationNames[randomAbilityIndex]);
                if (randomAbilityIndex < BossAbilities.Length)
                {
                    BossAbilities[randomAbilityIndex].Play();
                }
            }
        }
    }

    private void ChangeFacing(float horizontalSpeed)
    {
        if (horizontalSpeed>0)
        {
            Flip(true);
        }

        if (horizontalSpeed<0)
        {
            Flip(false);
        }
    }

    
    

    private bool attackCooldownUp()
    {
        return Time.time >= nextAttackTime;
    }

    private void FixedUpdate()
    {
        if (canMove && _enemyCurrentState == EnemyState.Standing)
        {
            Move();
        }
 
    }

//    public ContinuousStrikeHitbox continuousStrikeHitBoxScript;
    public EnemyDetector EnemyDetector;

    public float teleportLimit = 4f;
    public void ContinuousStrike()
    {
//        if (continuousStrikeHitBoxScript.playerInRange)
//        {
//            Attack();
//        }
        foreach (Collider col in EnemyDetector._enemiesInRange)
        {
            if (col.gameObject == PlayerProperty.player)
            {
                Attack();
                break;
            }
        }
    }

    public void Teleport()
    {
        Vector3 playerPos = PlayerProperty.playerPosition;
        if (Random.Range(0, 2) < 1)
        {
            transform.position = playerPos + new Vector3(-teleportLimit, 0, 0);
            Flip(true);
            
        }
        else
        {
            transform.position = playerPos + new Vector3(teleportLimit, 0, 0);
            Flip(false);

        }
    }


    public override void Move()
    {
        if (moveTimeRemainsThisRound > 0)
        {
            if (moveTowardsPlayer)
            {
//                rb.MovePosition(transform.position + PlayerDirectionInPlane()*moveSpeed*Time.fixedDeltaTime);
                rb.velocity = new Vector3(PlayerDirectionInPlane().x * moveSpeed,rb.velocity.y);
//                transform.Translate(PlayerDirectionInPlane()*moveSpeed*Time.deltaTime);
                moveTimeRemainsThisRound -= Time.fixedDeltaTime;    
            }
            else
            {
//                rb.MovePosition(transform.position-PlayerDirectionInPlane()*moveSpeed*Time.fixedDeltaTime);
                rb.velocity = new Vector3(-PlayerDirectionInPlane().x * moveSpeed,rb.velocity.y);


                moveTimeRemainsThisRound -= Time.fixedDeltaTime;
            }
                
        }
        else
        {
            ChangeBossMovementDirectionInRandom();
        }
    }

    public override bool AnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RollingAttack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("ContinuousStrike") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    private Vector3 PlayerDirectionInPlane()
    {
        Vector3 playerDirection = (GameManager.Instance.player.transform.position - transform.position).normalized;
       return new Vector3(playerDirection.x,0,playerDirection.z);
    }

    private void ChangeBossMovementDirectionInRandom()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= movingTowardsPlayerPercentage*100)
        {
            moveTowardsPlayer = true;
        }

        moveTimeRemainsThisRound = moveTimeInARow;
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
                animator.SetTrigger("LayDown");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
