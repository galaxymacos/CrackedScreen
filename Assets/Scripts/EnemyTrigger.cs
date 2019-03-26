using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player && !hasTriggered)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }

            hasTriggered = true;
        }
    }
}