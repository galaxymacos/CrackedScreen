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

    public override void Update()
    {
        base.Update();
        animator.SetBool("Idle", _enemyCurrentState == EnemyState.Standing);

        if (dodging)
        {
            if (isFacingRight)
                transform.Translate(-5 * Time.deltaTime, 0, 0);
            else
                transform.Translate(5 * Time.deltaTime, 0, 0);
        }
    }

    public void Dodge()
    {
        dodging = true;
    }

    public void UnDodge()
    {
        dodging = false;
    }

    public override void InteractWithPlayer()
    {
        if (StiffTimeRemain <= 0 && _enemyCurrentState == EnemyState.Standing)
        {
            if (PlayerInRange())
            {
                if (Time.time >= nextAttackTime)
                {
                    float random = Random.Range(0, 100);
                    if (random > 50)
                    {
                        animator.SetTrigger("Attack");
                    }
                    else
                    {
                        animator.SetTrigger("RollAttack");
                        chargeNextAttack = true;
                    }

                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
                else
                {
                    Move();
                    animator.SetFloat("Velocity", rb.velocity.x);
                }
            }
            else
            {
                Move();
                animator.SetFloat("Velocity", rb.velocity.x);
            }
        }
    }

    public void SpawnTheFuckingArrow()
    {
        print("Spawn arrow");
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }
        var position = transform.position;
        if (patrolRight)
        {
            Flip(true);
            rb.velocity = new Vector3(patrolSpeed, rb.velocity.y, rb.velocity.z);
            currentDistanceFromCenter += patrolSpeed * Time.deltaTime;
            if (needTurnAround())
            {
                currentDistanceFromCenter = rightLimit;
                floorExistsInFront = true;
            }

            if (currentDistanceFromCenter >= rightLimit) patrolRight = false;
//                            print("Walking right");
        }
        else
        {
            Flip(false);
//                rb.MovePosition(position + new Vector3(-patrolSpeed * Time.deltaTime, 0, 0));
            rb.velocity = new Vector3(-patrolSpeed, rb.velocity.y, rb.velocity.z);
            if (needTurnAround())
            {
                currentDistanceFromCenter = leftLimit;
                floorExistsInFront = true;
            }

            currentDistanceFromCenter -= patrolSpeed * Time.deltaTime;
            if (currentDistanceFromCenter <= leftLimit) patrolRight = true;
//                            print("Walking left");
        }
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