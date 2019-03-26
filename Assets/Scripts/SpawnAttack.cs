using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnAttack : MonoBehaviour
{
    [SerializeField] private GameObject objToSpawn;

    [Tooltip("How often should the player attack be spawned")]
    [SerializeField] private float spawnInterval = 5f;

    [SerializeField] private Transform spawnPosition;
    private float timeRemainsBeforeSpawning;
    // Start is called before the first frame update
    void Start()
    {
        if (spawnPosition == null)
        {
            spawnPosition = transform;
        }
        timeRemainsBeforeSpawning = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemainsBeforeSpawning -= Time.deltaTime;
        if (timeRemainsBeforeSpawning <= 0)
        {
            GameObject objSpawned = Instantiate(objToSpawn, spawnPosition.position, Quaternion.identity);
            SpawnObjInteraction(objSpawned);
            timeRemainsBeforeSpawning = spawnInterval;
        }
    }

    public abstract void SpawnObjInteraction(GameObject obj);
}
