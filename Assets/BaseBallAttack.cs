using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBallAttack : BossAbility
{
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private EnemyDetector StrikeHitBoxFront;
    [SerializeField] private EnemyDetector StrikeHitBoxBack;
    [SerializeField] private EnemyDetector ImpactWaveHitBox;
    [SerializeField] private int damageStrikeFront = 20;
    [SerializeField] private int damageStrikeBack = 10;
    [SerializeField] private int damageImpactWave = 40;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Play()
    {
        animator.SetTrigger("BaseballAttack");
    }

    private void MoveParticleEffect(float distance)
    {
        particleEffect.transform.Translate(new Vector3(distance,0,0));
    }

    public void DealDamageInStrikeHitBoxFront()
    {
        if (StrikeHitBoxFront.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(damageStrikeFront);
        }
    }

    public void DealDamageInStrikeHitBoxBack()
    {
        if (StrikeHitBoxBack.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(damageStrikeBack);
        }

    }

    public void DealDamageInImpactWaveHitBox()
    {
        if (ImpactWaveHitBox.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(damageImpactWave);
            PlayerProperty.playerClass.GetKnockOff(transform.position);
        }
    }
}
