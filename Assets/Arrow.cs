using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    internal Vector3 flyDirection;
    internal float flySpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(flyDirection*flySpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            HitPlayer();
        }
    }

    public void HitPlayer()
    {
        PlayerProperty.playerClass.TakeDamage(damage);
        PlayerProperty.playerClass.GetKnockOff(transform.position);
        Destroy(gameObject);
    }
}
