using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnVolcanoAttack : BossAbility
{
    private float intervalBetweenPos = 5f;
    private float timeIntervalBetweenSpawn = 0.5f;
    

    [SerializeField] private Vector3[] attackPos;

    [SerializeField] private GameObject warningCircle;
    [SerializeField] private GameObject volcano;

    private GameObject player;

    private float abilityStartTime;
    private float delayBetteenWarningAndAttack = 1f;

    private bool warning1;
    private bool warning2;
    private bool warning3;

    private bool explosion1;
    private bool explosion2;
    private bool explosion3;

    private bool isPlaying;

    private GameObject volcano1;
    private GameObject volcano2;
    private GameObject volcano3;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        if (!warning1)
        {
            volcano1 = Instantiate(volcano, attackPos[0], Quaternion.identity);
            AudioManager.instance.PlayBossSound("Warning");
            warning1 = true;
            print("Warning one");
        }
        
        if (Time.time > abilityStartTime + delayBetteenWarningAndAttack && !explosion1)
        {
            AudioManager.instance.PlayBossSound("Explosion");
            volcano1.GetComponent<VolcanoFlameHitPlayer>().ActivateDamage();
            explosion1 = true;
            print("explosion one");


        }
        if (Time.time > abilityStartTime + timeIntervalBetweenSpawn && !warning2)
        {
            volcano2 = Instantiate(volcano, attackPos[1], Quaternion.identity);
            AudioManager.instance.PlayBossSound("Warning");
            warning2 = true;
            print("Warning two");


        }
        if (Time.time > abilityStartTime + timeIntervalBetweenSpawn + delayBetteenWarningAndAttack && !explosion2)
        {
            AudioManager.instance.PlayBossSound("Explosion");
            volcano2.GetComponent<VolcanoFlameHitPlayer>().ActivateDamage();
            explosion2 = true;
            print("explosion two");

        }

        if (Time.time > abilityStartTime + 2 * timeIntervalBetweenSpawn && !warning3)
        {
            volcano3 = Instantiate(volcano, attackPos[2], Quaternion.identity);
            AudioManager.instance.PlayBossSound("Warning");
            warning3 = true;
            print("Warning three");


        }
        
        if (Time.time > abilityStartTime + 2* timeIntervalBetweenSpawn + delayBetteenWarningAndAttack && !explosion3)
        {
            AudioManager.instance.PlayBossSound("Explosion");
            volcano3.GetComponent<VolcanoFlameHitPlayer>().ActivateDamage();
            explosion3 = true;
            print("explosion three");


        }

        
        
        
    }

    public override void Play()
    {
        print("play volcano attack");
        LayerMask groundLayer = 1 << 11;
        player = GameManager.Instance.player;
        RaycastHit hitInfo;
        Physics.Raycast(player.transform.position, Vector3.down, out hitInfo, groundLayer);
        Vector3 hitCenter = hitInfo.point;

        
        if(UnityEngine.Random.Range(0,2) == 0){
            attackPos[0] = new Vector3(hitCenter.x-intervalBetweenPos,hitCenter.y,hitCenter.z);
            attackPos[1] = hitCenter;
            attackPos[2] = new Vector3(hitCenter.x+intervalBetweenPos,hitCenter.y,hitCenter.z);
        }
        else
        {
            attackPos[2] = new Vector3(hitCenter.x-intervalBetweenPos,hitCenter.y,hitCenter.z);
            attackPos[1] = hitCenter;
            attackPos[0] = new Vector3(hitCenter.x+intervalBetweenPos,hitCenter.y,hitCenter.z);
        }

        abilityStartTime = Time.time;
        warning1 = false;
        warning2 = false;
        warning3 = false;
        explosion1 = false;
        explosion2 = false;
        explosion3 = false;

        isPlaying = true;
    }
}
