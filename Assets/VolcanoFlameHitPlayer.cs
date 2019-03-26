using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoFlameHitPlayer : MonoBehaviour
{
    private bool playerInRange;

    private bool hasDealtDamage;

    private bool canDealDamage;

    [SerializeField] private float countDownBeforeDestroy = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !hasDealtDamage && canDealDamage)
        {
            GameManager.Instance.player.GetComponent<Player>().TakeDamage(10);
            GameManager.Instance.player.GetComponent<Player>().GetKnockOff(transform.position);
            hasDealtDamage = true;
        }

        countDownBeforeDestroy -= Time.deltaTime;
        if (countDownBeforeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = true;
            
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = false;
            
        }
    }



    public void ActivateDamage()
    {
        canDealDamage = true;
    }

}
