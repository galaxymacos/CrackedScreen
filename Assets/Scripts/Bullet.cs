using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage;

    [Tooltip("How much force will be applied to player if player gets hit")]
    [SerializeField] private Vector3 pushForce;

    [SerializeField] private float flyingSpeed;

    [SerializeField] internal Vector3 flyingDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(flyingDirection.normalized*flyingSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            
            var playerProperty = GameManager.Instance.player.GetComponent<Player>();
            playerProperty.TakeDamage(damage);
            Destroy(gameObject);

            if (GameManager.Instance.isPlayerDying)
            {
                return;
            }
            playerProperty.GetKnockOff(transform.position);
        }
    }

}
