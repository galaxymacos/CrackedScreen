using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cog : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private int damage = 10;
    [SerializeField] private float rotationSpeed = 500f;
    private EnemyDetector enemyDetector;

    private void Start()
    {
        playerScript = GameManager.Instance.player.GetComponent<Player>();
        enemyDetector = transform.GetComponent<EnemyDetector>();
    }

    private void Update()
    {
        transform.Rotate(0,0,rotationSpeed*Time.deltaTime);
        if (enemyDetector.playerInRange())
        {
            playerScript.TakeDamage(damage);
            playerScript.GetKnockOff(transform.position);
        }
    }
}
