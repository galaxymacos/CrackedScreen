using Enemies;
using UnityEngine;

public class PatrolEnemyGroundDetector : MonoBehaviour
{
    private bool hasFloor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent.GetComponent<PatrolEnemy>().floorExistsInFront = true;
        }
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent.GetComponent<PatrolEnemy>().floorExistsInFront = false;
        }

    }
}