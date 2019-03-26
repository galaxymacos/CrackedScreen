using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCushion : MonoBehaviour
{
    private EnemyDetector enemyDetector;

    [SerializeField] private float airForce;
    // Start is called before the first frame update
    void Start()
    {
        enemyDetector = GetComponent<EnemyDetector>();
        
    }


    private void FixedUpdate()
    {
        if (enemyDetector.playerInRange())
        {
            PlayerProperty.player.GetComponent<Rigidbody>().AddForce(new Vector3(0,airForce*Time.fixedDeltaTime,0));
            PlayerProperty.movementClass.Jump();
        }
    }
}
