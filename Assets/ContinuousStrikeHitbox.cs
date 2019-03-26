using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class ContinuousStrikeHitbox : MonoBehaviour
{

    public bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            playerInRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerProperty.player)
        {
            playerInRange = false;
        }
    }
}
