using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Vector3 orientation;

    [SerializeField] private float speed;

    [SerializeField] private float numOfBounce;

    [SerializeField] private int damage;

    private EnemyDetector enemyDetector;

    
    

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = new Vector3(-1,0,1).normalized;
        rb.velocity = orientation * speed;
        enemyDetector = GetComponentInChildren<EnemyDetector>();
    }

    private bool hasExplode;

    // Update is called once per frame
    void Update()
    {
        if (enemyDetector.playerInRange() && !hasExplode)
        {
            Explode();
            hasExplode = true;
        }
    }

    private void Explode()
    {
        PlayerProperty.playerClass.TakeDamage(damage);
        PlayerProperty.playerClass.GetKnockOff(transform.position);
        AudioManager.instance.PlaySfx("CannonBallExplode");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
