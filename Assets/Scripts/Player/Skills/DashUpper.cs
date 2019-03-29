using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpper : Skill
{
    [SerializeField] private float phaseOneDuration = 0.3f;
    [SerializeField] private float phaseTwoDuration = 0.5f;
    [SerializeField] private Transform dashPosition;
    [SerializeField] private float dashRadius;
    [SerializeField] private Transform uppercutPosition;
    [SerializeField] private float uppercutRadius;
    [SerializeField] private Vector3 enemyKnockDownForce;
    [SerializeField] private int damage = 30;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private EnemyDetector dashEnemyDetector;
    [SerializeField] private EnemyDetector uppercutEnemyDetector;

    [Tooltip("How long can the enemy move again if it is hit by dash")] [SerializeField]
    private float stiffTimePhaseOne = 0.3f;


    private bool _tookDamagePhaseOne;
    private bool _tookDamagePhaseTwo;
    private Vector3 _originalEnemyKnockDownForce;


    private void Awake()
    {
        _originalEnemyKnockDownForce = enemyKnockDownForce;
    }


    private void FixedUpdate()
    {
        LayerMask enemyLayer = 1 << 9;
        enemyKnockDownForce = new Vector3(enemyKnockDownForce.x * playerController.facingOffset, enemyKnockDownForce.y,
            enemyKnockDownForce.z);
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }

        if (_isPlaying)
        {
            if (TimePlayed + phaseOneDuration >= Time.time)
            {
                rb.MovePosition(new Vector3(dashSpeed * Time.fixedDeltaTime, 0) * playerController.facingOffset +
                                rb.transform.position);
                List<Collider> enemies = dashEnemyDetector._enemiesInRange;
                foreach (Collider enemy in enemies) // Push enemies back
                {
                    if (enemy == null) continue;
                    enemy.GetComponent<Enemy>().ForceMove(new Vector3(dashSpeed * Time.fixedDeltaTime, 0) * playerController.facingOffset);
                    if (!_tookDamagePhaseOne) // only take damage once in phase one
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(damage);
                        enemy.GetComponent<Enemy>().Stiff(stiffTimePhaseOne);
                        _tookDamagePhaseOne = true;
                    }
                }
            }
            else
            {
                AudioManager.instance.PlaySfx("Uppercut");
                _isPlaying = false;

                rb.velocity = Vector3.zero;
                // Activate the airborne indicator in child object
                List<Collider> enemies = uppercutEnemyDetector._enemiesInRange;
                foreach (var enemy in enemies)
                {
                    if(enemy==null)
                        continue;
                    enemyKnockDownForce = new Vector3(_originalEnemyKnockDownForce.x * playerController.facingOffset,
                        _originalEnemyKnockDownForce.y, _originalEnemyKnockDownForce.z);                    
                    enemy.GetComponent<Enemy>().KnockUp(enemyKnockDownForce);
                    if (!_tookDamagePhaseTwo)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(damage);
                        _tookDamagePhaseTwo = true;
                    }
                }

                StartCoroutine(PlayerCanControl(phaseTwoDuration));
            }
        }
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(dashPosition.position, dashRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(uppercutPosition.position, uppercutRadius);
    }

    public override void Play()
    {
        if (_skillNotOnCooldown) // Check if the skill is on cooldown
        {
            AudioManager.instance.PlaySfx("Dash");
            GameManager.Instance.animator.SetTrigger("Dash Uppercut");
            playerController.canControl = false;

            base.Play();
            _tookDamagePhaseOne = false;
            _tookDamagePhaseTwo = false;
            _skillNotOnCooldown = false;
        }
        else
        {
            print("Skill is on cooldown");
        }
    }
}