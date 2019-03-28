using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class PatrolEnemy : Enemy
    {
        private Animator animator;
        private bool patrolRight = true;
        private float currentDistanceFromCenter;
        public float leftLimit = -5f;
        public float rightLimit = 5f;

        [SerializeField] private EnemyDetector AttackHitBox;
        internal bool floorExistsInFront;



        private bool needTurnAround()
        {
            if (!isGrounded())
            {
                return false;
            }

            if (!floorExistsInFront)
            {
                return true;
            }

            return false;
        }


        [SerializeField] private float patrolSpeed = 5f;

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            OnChangeEnemyStateCallback += AnimateEnemy;
            base.Start();
            currentDistanceFromCenter = Random.Range(leftLimit, rightLimit);
            if (Random.Range(0, 2) == 0)
            {
                patrolRight = true;
            }
            else
            {
                patrolRight = false;
            }
        }

        private bool PlayerInRange()
        {
            foreach (Collider col in AttackHitBox._enemiesInRange)
            {
                if (col.gameObject == PlayerProperty.player)
                {
                    return true;
                }

            }
            return false;

        }

        public bool isGrounded()
        {
            LayerMask groundLayer = 1 << 11;
            bool isConnectingToGround = Physics.Raycast(transform.position, Vector3.down, GetComponent<BoxCollider>().size.y/2+GetComponent<BoxCollider>().center.y,
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
            animator.SetBool("Idle",_enemyCurrentState == EnemyState.Standing);
        }

        public override void InteractWithPlayer()
        {
            if (StiffTimeRemain<=0 && _enemyCurrentState == EnemyState.Standing)
            {
                if (PlayerInRange())
                {
                    if (Time.time >= nextAttackTime)
                    {
                        animator.SetTrigger("Attack");
                        nextAttackTime = Time.time + 1 / attackSpeed;
                    }
                }
                else
                {
                    Move();
                    animator.SetFloat("Velocity",rb.velocity.x);
                }
            }
        }

   
        public override void Move()
        {
            var position = transform.position;
            if (patrolRight)
            {
                Flip(true);
                rb.velocity = new Vector3(patrolSpeed,rb.velocity.y,rb.velocity.z);
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
                rb.velocity = new Vector3(-patrolSpeed,rb.velocity.y,rb.velocity.z);
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

        public override bool AnimationPlaying()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("MinionAttack");
            
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
}
