using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private bool _tookDamage;
    [SerializeField] private float absorbSpeed = 5f;
    [SerializeField] private int damage = 20;
    public float duration = 3f;

    private float existTime;
    [SerializeField] private float explosionForce = 100f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float damagePerSecWhenAbsorb = 4f;

    [SerializeField] private float stiffTimeAfterExplosion = 1.5f;

    private GameObject[] enemiesInRange;
    private Rigidbody[] enemyRigidbodysInRange;
    private Camera mainCamera;

    private void Start()
    {
        LayerMask enemyLayer = 1 << 9;

        existTime = Time.time;
        mainCamera = Camera.main;
        IEnumerable<Collider> a = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        enemiesInRange = a.Select(col => col.gameObject).ToArray();
        enemyRigidbodysInRange = enemiesInRange.Select(enemy => enemy.GetComponent<Rigidbody>()).ToArray();
        AudioManager.instance.StopAllSfx();
        AudioManager.instance.PlaySfx("BlackHoleSoaking");
        print("Playing black hole soaking");
    }

    private void FixedUpdate()
    {
        LayerMask enemyLayer = 1 << 9;
        if (existTime + duration >= Time.time)
        {
            foreach (var enemy in enemyRigidbodysInRange)
            {
                if (enemy != null)
                {
                    var direction = (transform.position - enemy.transform.position).normalized;
                    enemy.AddForce(direction * Time.fixedDeltaTime * absorbSpeed);
                    enemy.GetComponent<Enemy>().TakeDamage(1 / damagePerSecWhenAbsorb);    
                    enemy.GetComponent<Enemy>().Stiff(Time.fixedDeltaTime);
                }
                
            }
        }


        else
        {
            foreach (var enemy in enemyRigidbodysInRange)
            {
                if (enemy != null)
                {
                    enemy.AddExplosionForce(explosionForce,
                        transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
                            Random.Range(-1f, 1f)), radius);
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                    enemy.GetComponent<Enemy>().Stiff(stiffTimeAfterExplosion);
                }
                
                mainCamera.GetComponent<CameraEffect>().ShakeForSeconds(0.15f);
            }
            AudioManager.instance.StopSfx("BlackHoleSoaking");
            AudioManager.instance.PlaySfx("BlackHoleExplosion");
            Destroy(gameObject);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);
    }
}