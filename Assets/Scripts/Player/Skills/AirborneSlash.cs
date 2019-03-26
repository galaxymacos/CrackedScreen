using UnityEngine;

namespace Skills
{
    public class AirborneSlash : Skill
    {
        [SerializeField] private float damage = 10f;
        public EnemyDetector EnemyDetector;

        public Vector3 enemyKnockdownForce;
        private static readonly int airSlash = Animator.StringToHash("Air Slash");


        public override void Start()
        {
            base.Start();
//            playerController.onFacingChangeCallback += CreateAirborneSlashCollider;
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
                GameManager.Instance.animator.SetTrigger(airSlash);
                _skillNotOnCooldown = false;
                base.Play();

                AudioManager.instance.PlaySfx("Air Slash");
                var enemies = EnemyDetector._enemiesInRange;
                foreach (var enemy in enemies)
                {
                    if (enemy == null)
                         continue;
                    enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    if (enemy.transform.position.x>PlayerProperty.playerPosition.x)
                    {
                        print("slash enemy from left");
                        enemy.GetComponent<Enemy>().KnockUp(enemyKnockdownForce);
                    }
                    else
                    {
                        print("slash enemy from right");

                        enemy.GetComponent<Enemy>().KnockUp(new Vector3(-enemyKnockdownForce.x,enemyKnockdownForce.y,enemyKnockdownForce.z));
                    }
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            else
            {
                print("Skill is on cooldown");
            }
        }

//        public void CreateAirborneSlashCollider(bool isFacingRight)
//        {
//            enemyKnockdownForce = new Vector3(-enemyKnockdownForce.x, enemyKnockdownForce.y, enemyKnockdownForce.z);
//        }

        public bool PlayerisLeft()
        {
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            Vector3 pos = transform.position;

            return playerPos.x < pos.x;
        }
    }
}