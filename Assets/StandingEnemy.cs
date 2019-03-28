using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Die()
    {
        AudioManager.instance.PlaySfx("RockBreak");    // TODO find rock break sound
        Destroy(gameObject);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    public override void Move()
    {
        return;
    }

    public override bool AnimationPlaying()
    {
        return false;
    }

    public override void InteractWithPlayer()
    {
        
    }

    public override void ForceMove(Vector3 force)
    {
        return;
    }
}
