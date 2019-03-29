using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherEnemy : Enemy {
    private Animator animator;
<<<<<<< HEAD
    private bool patrolRight = true;
    private float currentDistanceFromCenter;
=======
    [SerializeField] private GameObject arrow;

    [SerializeField] private Transform arrowSpawnPoint;

    [SerializeField] private EnemyDetector AttackHitBox;
    [SerializeField] private float dodgingProbability = 0.8f;
    private bool dodging;


    private bool chargeNextAttack;
    private float currentDistanceFromCenter;
    internal bool floorExistsInFront;
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
    public float leftLimit = -5f;
    public float rightLimit = 5f;

    [SerializeField] private EnemyDetector AttackHitBox;
    internal bool floorExistsInFront;
    [SerializeField] private GameObject arrow;



<<<<<<< HEAD
    private bool needTurnAround()
    {
        if (!isGrounded())
        {
            return false;
        }
=======
    private bool needTurnAround() {
        if (!isGrounded()) return false;
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen

        if (!floorExistsInFront)
        {
            return true;
        }

        return false;
    }

<<<<<<< HEAD

    [SerializeField] private float patrolSpeed = 5f;

    protected override void Start()
    {
=======
    protected override void Start() {
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
        animator = GetComponent<Animator>();
        OnChangeEnemyStateCallback += AnimateEnemy;
        base.Start();
        currentDistanceFromCenter = Random.Range(leftLimit, rightLimit);
        if (Random.Range(0, 2) == 0)
        {
            patrolRight = true;
        }
        else
<<<<<<< HEAD
        {
            patrolRight = false;
        }
    }

    private bool PlayerInRange()
    {
        foreach (Collider col in AttackHitBox._enemiesInRange)
        {
=======
            patrolRight = false;
    }
    
    

    public override void TakeDamage(float damage) {
        if (dodging) {
            return;
        }
        if (isProbabilityEventHappens(dodgingProbability) && _enemyCurrentState == EnemyState.Standing) {
            FlipAccordingToPosition();
            animator.SetTrigger("RollAttack");
            dodging = true;
            print("Dodging");

            chargeNextAttack = true;
            return;
        }

        base.TakeDamage(damage);
    }

    public override void KnockUp(Vector3 force) {
        if (dodging) {
            return;
        }
        if (isProbabilityEventHappens(dodgingProbability)) {
            animator.SetTrigger("RollAttack");
            chargeNextAttack = true;
            return;
        }
        base.KnockUp(force);
    }

    private bool isProbabilityEventHappens(float odd) {
        var randomNum = Random.Range(0, 100);
        if (randomNum > odd*100)
            return false;
        return true;
    }

    private bool PlayerInRange() {
        foreach (var col in AttackHitBox._enemiesInRange)
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
            if (col.gameObject == PlayerProperty.player)
            {
                return true;
            }

        }
        return false;

    }

    public bool isGrounded() {
        LayerMask groundLayer = 1 << 11;
        bool isConnectingToGround = Physics.Raycast(transform.position, Vector3.down, GetComponent<BoxCollider>().size.y/2+GetComponent<BoxCollider>().center.y,
            groundLayer);
        return isConnectingToGround;
    }

    protected override void Die() {
        spriteRenderer.enabled = false;
        AudioManager.instance.PlaySfx("MinionDie");
        Destroy(gameObject);
<<<<<<< HEAD

    }

    private bool dodging;
    public override void Update()
    {
=======
    }
    
    

    [SerializeField] private float dodgingSpeed = 20f;
//    private float dodgingTimeRemains;
    public override void Update() {
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
        base.Update();
        animator.SetBool("Idle",_enemyCurrentState == EnemyState.Standing);

        if (dodging)
        {
<<<<<<< HEAD
            if (isFacingRight)
            {
                transform.Translate(-5*Time.deltaTime,0,0);
            }
=======
            print("Player is dodging");
//            FlipAccordingToPosition();
            if (PlayerIsAtRight())
                transform.Translate(-dodgingSpeed * Time.deltaTime, 0, 0);
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
            else
            {
                transform.Translate(5*Time.deltaTime,0,0);
            }
        }
    }

    private bool chargeNextAttack;

<<<<<<< HEAD
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
        if (StiffTimeRemain<=0 && _enemyCurrentState == EnemyState.Standing)
        {
            if (PlayerInRange())
            {
                if (Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !chargeNextAttack)
                    {
                        animator.SetTrigger("RollAttack");
                        chargeNextAttack = true;
                    }
                }
            }
            else
            {
                Move();
                animator.SetFloat("Velocity",rb.velocity.x);
=======
    

    


    public void UnDodge() {
        dodging = false;
    }

    public override void InteractWithPlayer() {
        if (StiffTimeRemain <= 0 && _enemyCurrentState == EnemyState.Standing) {
            if (PlayerInRange()) {
                if (Time.time >= nextAttackTime && !dodging) {
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1 / attackSpeed;
                }
                else {
                    Move();
                    
                    animator.SetFloat("Velocity", rb.velocity.x);
                }
            }
            else {
                Move();
                animator.SetFloat("Velocity", rb.velocity.x);
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
            }
        }
    }

<<<<<<< HEAD
    [SerializeField] private Transform arrowSpawnPoint;
    
    public void SpawnTheFuckingArrow()
    {
        print("Spawn arrow");
        var arrowInstantiate = Instantiate(arrow,arrowSpawnPoint.position,Quaternion.identity);
=======
    public void SpawnTheFuckingArrow() {
        var arrowInstantiate = Instantiate(arrow, arrowSpawnPoint.position, Quaternion.identity);
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
        arrowInstantiate.GetComponent<Arrow>().flyDirection =
            (PlayerProperty.playerPosition - transform.position).normalized;
        if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
        {
            arrowInstantiate.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (chargeNextAttack) {
            arrowInstantiate.GetComponent<Arrow>().flySpeed *= 2;
            chargeNextAttack = false;
            var SecondArrow = Instantiate(arrow,arrowSpawnPoint.position + new Vector3(0,-2,0),Quaternion.identity);
            SecondArrow.GetComponent<Arrow>().flyDirection =
                (PlayerProperty.playerPosition - transform.position).normalized;
            SecondArrow.GetComponent<Arrow>().flySpeed *= 2;

            if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
            {
                SecondArrow.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            var thirdArrow = Instantiate(arrow,arrowSpawnPoint.position + new Vector3(0,2,0),Quaternion.identity);
            thirdArrow.GetComponent<Arrow>().flyDirection =
                (PlayerProperty.playerPosition - transform.position).normalized;
            thirdArrow.GetComponent<Arrow>().flySpeed *= 2;
            if ((PlayerProperty.playerPosition - transform.position).normalized.x < 0)
            {
                thirdArrow.GetComponent<SpriteRenderer>().flipX = true;
            }
            

        }
        
    }

<<<<<<< HEAD
   
    public override void Move()
    {
        var position = transform.position;
        if (patrolRight)
        {
            Flip(true);
            rb.velocity = new Vector3(patrolSpeed,rb.velocity.y,rb.velocity.z);
=======

    public override void Move() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge")) {
            return;
        }
        if (patrolRight) {
                Flip(true);

            rb.velocity = new Vector3(patrolSpeed, rb.velocity.y, rb.velocity.z);
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
            currentDistanceFromCenter += patrolSpeed * Time.deltaTime;
            if (needTurnAround()) {
                currentDistanceFromCenter = rightLimit;
                floorExistsInFront = true;
            }
            if (currentDistanceFromCenter >= rightLimit) patrolRight = false;
//                            print("Walking right");
        }
<<<<<<< HEAD
        else
        {
            Flip(false);
//                rb.MovePosition(position + new Vector3(-patrolSpeed * Time.deltaTime, 0, 0));
            rb.velocity = new Vector3(-patrolSpeed,rb.velocity.y,rb.velocity.z);
            if (needTurnAround())
            {
=======
        else {
                Flip(false);
            rb.velocity = new Vector3(-patrolSpeed, rb.velocity.y, rb.velocity.z);
            if (needTurnAround()) {
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
                currentDistanceFromCenter = leftLimit;
                floorExistsInFront = true;
            }

            currentDistanceFromCenter -= patrolSpeed * Time.deltaTime;
            if (currentDistanceFromCenter <= leftLimit) patrolRight = true;
//                            print("Walking left");
        }
    }

<<<<<<< HEAD
    public void AnimateEnemy(EnemyState enemyState)
    {
        switch (enemyState)
        {
=======
    public void AnimateEnemy(EnemyState enemyState) {
        switch (enemyState) {
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
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