using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAbilitiesManager : MonoBehaviour
{
    private float timeRemainsForSpawningBossAbility;
    [SerializeField] private float spawningBossAbilityInterval = 3f;
    [SerializeField] private BossAbility[] bossAbilities;

    public static BossAbilitiesManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        timeRemainsForSpawningBossAbility = spawningBossAbilityInterval;
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (timeRemainsForSpawningBossAbility > 0f)
        {
            timeRemainsForSpawningBossAbility -= Time.deltaTime;
            if (timeRemainsForSpawningBossAbility <= 0f)
            {
                PlayRandomAbility();
                timeRemainsForSpawningBossAbility = spawningBossAbilityInterval;
            }
        }
    }

    public void PlayRandomAbility()
    {
       bossAbilities[Random.Range(0,bossAbilities.Length)].Play();
    }

    public void ResetSpawnAbilityTime()
    {
        timeRemainsForSpawningBossAbility = spawningBossAbilityInterval;
    }
    
}
