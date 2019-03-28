using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DragonFist : BossAbility
{
    private Animator animator;

    private bool isDashingForward;

    [SerializeField] private float dashingSpeed = 30f;

    [SerializeField] private float dragonFistFlyKnockUpForce = 1000f;

    [SerializeField] private EnemyDetector dragonFistHitBox;

    [SerializeField] private float followHomeRunChance = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashingForward)
        {
            if (PlayerProperty.playerPosition.x - transform.position.x > 0)
            {
                transform.Translate(new Vector3(dashingSpeed*Time.deltaTime,0,0));
            }
            else
            {
                transform.Translate(new Vector3(-dashingSpeed*Time.deltaTime,0,0));
            }
        }
    }

    public override void Play()
    {
        animator.SetTrigger("DragonFist");
    }

    /// <summary>
    /// This method will be called in animator event
    /// </summary>
    public void DashForward()
    {
        isDashingForward = true;
    }

    public void DragonFistStrike()
    {
        isDashingForward = false;
        if (dragonFistHitBox.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(10);
            PlayerProperty.playerClass.GetKnockOff(transform.position,new Vector3(0,dragonFistFlyKnockUpForce,0));
            PlayerProperty.playerClass.ResetInvincibleTime();    // Player not invincible after it is knocked up by dragon fist
            animator.SetBool("DragonFistHitPlayer",true);
        }
    }


    public void decideIfFollowHomerun()
    {
        float RandomFloat = Random.Range(0, 100) / 100f;
        if (RandomFloat < followHomeRunChance)
        {
            
        }
        else
        {
            animator.enabled = false;
            animator.enabled = true;
        }
    }
}
