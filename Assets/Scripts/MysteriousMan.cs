using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousMan : MonoBehaviour
{
    public GameObject dialogueBox;
    private BossEnemy bossEnemy;

    private void Start()
    {
        bossEnemy = transform.GetComponent<BossEnemy>();
        bossEnemy.OnBossDieCallback += ActivateBossDieDialogue;
    }

    public void ActivateBossDieDialogue()
    {
        dialogueBox.SetActive(true);
    }
}