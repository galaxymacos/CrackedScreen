using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill
{
            [SerializeField] private float damage = 10f;
        public EnemyDetector EnemyDetector;

        public Vector3 enemyKnockdownForce;

        [SerializeField] private float stiffTimeWhenHit = 1f;

        public override void Start()
        {
            base.Start();
            playerController.onFacingChangeCallback += CreateAirborneSlashCollider;
        }

        private void Update()
        {
            if (!_skillNotOnCooldown)
            {
                if (TimePlayed + cooldown <= Time.time) _skillNotOnCooldown = true;
            }
        }

        public override void Play()
        {
            if (_skillNotOnCooldown)
            {
                GameManager.Instance.animator.SetTrigger("Basic Attack");
                _skillNotOnCooldown = false;
                base.Play();

                AudioManager.instance.PlaySfx("Basic Attack");
                var enemies = EnemyDetector._enemiesInRange;
                bool HasAttackedOneEnemy = false;
                foreach (var enemy in enemies)
                {
                    if (enemy == null)
                         continue;
                    // Basic attack can only attack one enemy
                    if (HasAttackedOneEnemy)
                        break;
                    HasAttackedOneEnemy = true;
                    enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    if (enemy.transform.position.x>transform.position.x)
                    {
                        enemy.GetComponent<Rigidbody>().AddForce(enemyKnockdownForce);                        
                    }
                    else
                    {
                        enemy.GetComponent<Rigidbody>().AddForce(new Vector3(-enemyKnockdownForce.x,enemyKnockdownForce.y,enemyKnockdownForce.z));                        

                    }
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                    enemy.GetComponent<Enemy>().Stiff(stiffTimeWhenHit);
                }
            }
            else
            {
                print("Skill is on cooldown");
            }
        }

        public void CreateAirborneSlashCollider(bool isFacingRight)
        {
            enemyKnockdownForce = new Vector3(-enemyKnockdownForce.x, enemyKnockdownForce.y, enemyKnockdownForce.z);
        }

        public bool PlayerisLeft()
        {
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            Vector3 pos = transform.position;

            return playerPos.x < pos.x;
        }

}
