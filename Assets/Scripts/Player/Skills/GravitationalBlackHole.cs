using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBlackHole : Skill
{
    [SerializeField] private GameObject gravitationalBlackHole;
    [SerializeField] private Transform place;


    private bool _tookDamage;

    private void Update()
    {
        if (!_skillNotOnCooldown)
        {
            if (TimePlayed + cooldown <= Time.time)
            {
                _skillNotOnCooldown = true;
            }
        }
    }


    public override void Play()
    {
        if (_skillNotOnCooldown) // Check if the skill is on cooldown
        {
            GameManager.Instance.animator.SetTrigger("Black Hole");

            _skillNotOnCooldown = false;
            print("playing skill");
            base.Play();
            Instantiate(gravitationalBlackHole, place.position, Quaternion.identity);
        }
        else
        {
            print("Black hole attack is on cooldown");
        }
    }
}