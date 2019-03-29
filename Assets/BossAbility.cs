using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAbility : MonoBehaviour
{
    public abstract void Play();

    public void SkillIsOver()    // This method will be called in the event of each skill's animation
    {
        BossAbilitiesManager.Instance.ResetSpawnAbilityTime();
    }
}
