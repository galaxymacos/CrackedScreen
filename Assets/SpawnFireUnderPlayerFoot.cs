using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireUnderPlayerFoot : BossAbility
{
    [SerializeField] private GameObject preAttack;

    [SerializeField] private GameObject attack;

    [SerializeField] private float delayBeforeMainAttackLands = 1.5f;

    [SerializeField] private float explosionRadius = 2f;

    private Vector3 attackLocation;
    private float remainTimeBeforeMainAttackLands;
    private GameObject player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainTimeBeforeMainAttackLands > 0)
        {
            remainTimeBeforeMainAttackLands -= Time.deltaTime;
            if (remainTimeBeforeMainAttackLands <= 0)
            {
                Instantiate(attack, attackLocation, Quaternion.identity);
                AudioManager.instance.PlayBossSound("Explosion");
                if ((player.transform.position - attackLocation).magnitude < explosionRadius)
                {
                    player.GetComponent<Player>().TakeDamage(20);
                    player.GetComponent<Player>().GetKnockOff(attackLocation);
                }
                
            }
        }
    }

    public override void Play()
    {
        remainTimeBeforeMainAttackLands = delayBeforeMainAttackLands;
        attackLocation = player.transform.position;
        Instantiate(preAttack, attackLocation, Quaternion.identity);    // Indicate that an attack is going to land here soon
        AudioManager.instance.PlayBossSound("Warning");
    }
}
