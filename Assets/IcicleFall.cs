using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleFall : MonoBehaviour
{
    [SerializeField] private GameObject icicle;
    private float timeRemains;
    private bool hasInteracted;

    private void Update()
    {
        if (timeRemains > 0)
        {
            timeRemains -= Time.deltaTime;
            if (timeRemains <= 0f)
            {
                icicle.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerProperty.player && !hasInteracted)
        {
            icicle.GetComponent<FallingTrap>().Play();
            hasInteracted = true;
        }
    }
}
