using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherEnemy : Enemy
{
    private Animator animator;
    [SerializeField] private GameObject arrow;

    [SerializeField] private Transform arrowSpawnPoint;

    [SerializeField] private EnemyDetector AttackHitBox;


    private bool chargeNextAttack;
    private float currentDistanceFromCenter;
    private bool dodging;
    [SerializeField] private float dodgingProbability = 0.8f;


    [SerializeField] private float dodgingSpeed = 20f;
    internal bool floorExistsInFront;
    public float leftLimit = -5f;
    private bool patrolRight = true;


    [SerializeField] private float patrolSpeed = 5f;
    public float rightLimit = 5f;


    private bool needTurnAround()
    {
        if (!isGrounded()) return false;

        if (!floorExistsInFront) return true;

        return false;
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        OnChangeEnemyStateCallback += AnimateEnemy;
        base.Start();
        currentDistanceFromCenter = Random.Range(leftLimit, rightLimit);
        if (Random.Range(0, 2) == 0)
            patrolRight = true;
        else
            patrolRight = false;
    }
    
    


    public override void TakeDamage(float damage)
    {
        if (dodging) return;
        if (DodgingSucceed()) return;
        base.TakeDamage(damage);
    }

    public override void KnockUp(Vector3 force)
    {
        if (dodging) return;
        if (DodgingSucceed()) return;
        base.KnockUp(force);
    }

    private bool DodgingSucceed()
    {
        if (isProbabilityEventHappens(dodgingProbability) && _enemyCurrentState == EnemyState.Standing)
        {
            FaceBasedOnPlayerPosition();
            dodging = true;
            animator.SetTrigger("RollAttack");
            chargeNextAttack = true;
            return true;
        }

        return false;
    }

    private bool isProbabilityEventHappens(float odd)
    {
        var randomNum = Random.Range(0, 100);
        if (randomNum > odd * 100)
            return false;
        return true;
    }

    private bool PlayerInRange()
    {
        foreach (var col in AttackHitBox._enemiesInRange)
            if (col.gameObject == PlayerProperty.player)
                return true;
        return false;
    }

    public bool isGrounded()
    {
        LayerMask groundLayer = 1 << 11;
        var isConnectingToGround = Physics.Raycast(transform.position, Vector3.down,
            GetComponent<BoxCollider>().size.y / 2 + GetComponent<BoxCollider>().center.y,
            groundLayer);
        return isConnectingToGround;
    }

    protected override void Die()
    {
        spriteRenderer.enabled = false;
        AudioManager.instance.PlaySfx("MinionDie");
        Destroy(gameObject);
    }
    
    public bool CanMove()
    {
        return !isStiffed && !AnimationPlaying() && _enemyCurrentState == EnemyState.Standing;
    }

//    private float dodgingTimeRemains;
    public override void Update()
    {
        base.Update();
        animator.SetBool("Idle", _enemyCurrentState == EnemyState.Standing);

        if (dodging)
        {
            FaceBasedOnPlayerPosition();
            if (PlayerIsAtRight())
                transform.Translate(-dodgingSpeed * Time.deltaTime, 0, 0);
            else
                transform.Translate(dodgingSpeed * Time.deltaTime, 0, 0);
        }
    }


    public void UnDodge()
    {
        dodging = false;
    }
    
    private void FixedUpdate()
    {
        animator.SetFloat("Velocity",rb.velocity.x);
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
    
    public override void InteractWithPlayer()
    {
        if (StiffTimeRemain <= 0 && _enemyCurrentState == EnemyState.Standing && !AnimationPlaying())
        {
            if (PlayerInRange())
            {
                if (Time.time >= nextAttackTime && !dodging)
                {
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
            }
        }
    }

    public void SpawnTheFuckingArrow()
    {
        var arrowInstantiate = Instantiate(arrow, arrowSpawnPoint.position, Quaternion.identity);
        arrowInstantiate.GetComponent<Arrow>().flyDirection =
            (PlayerProperty.playerPosition - transform.position).normalized;
        if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
            arrowInstantiate.GetComponent<SpriteRenderer>().flipX = true;

        if (chargeNextAttack)
        {
            arrowInstantiate.GetComponent<Arrow>().flySpeed *= 2;
            chargeNextAttack = false;
            var SecondArrow = Instantiate(arrow, arrowSpawnPoint.position + new Vector3(0, -2, 0), Quaternion.identity);
            SecondArrow.GetComponent<Arrow>().flyDirection =
                (PlayerProperty.playerPosition - transform.position).normalized;
            SecondArrow.GetComponent<Arrow>().flySpeed *= 2;

            if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
                SecondArrow.GetComponent<SpriteRenderer>().flipX = true;

            var thirdArrow = Instantiate(arrow, arrowSpawnPoint.position + new Vector3(0, 2, 0), Quaternion.identity);
            thirdArrow.GetComponent<Arrow>().flyDirection =
                (PlayerProperty.playerPosition - transform.position).normalized;
            thirdArrow.GetComponent<Arrow>().flySpeed *= 2;
            if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
                thirdArrow.GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    public override void Move()
    {
        
        if (patrolRight)
        {
            rb.velocity = new Vector3(patrolSpeed, rb.velocity.y, rb.velocity.z);
            currentDistanceFromCenter += patrolSpeed * Time.deltaTime;
            if (needTurnAround())
            {
                currentDistanceFromCenter = rightLimit;
                floorExistsInFront = true;
            }

            if (currentDistanceFromCenter >= rightLimit) patrolRight = false;
        }
        else
        {
            rb.velocity = new Vector3(-patrolSpeed, rb.velocity.y, rb.velocity.z);
            if (needTurnAround())
            {
                currentDistanceFromCenter = leftLimit;
                floorExistsInFront = true;
            }

            currentDistanceFromCenter -= patrolSpeed * Time.deltaTime;
            if (currentDistanceFromCenter <= leftLimit) patrolRight = true;
        }
    }

    public override bool AnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
               animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge");
    }

    public void AnimateEnemy(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Standing:
                animator.SetTrigger("Idle");
                break;
            case EnemyState.GotHitToAir:
                animator.SetTrigger("HitToAir");
                break;
            case EnemyState.LayOnGround:
                animator.SetTrigger("LayDown");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(enemyState), enemyState, null);
        }
    }
}