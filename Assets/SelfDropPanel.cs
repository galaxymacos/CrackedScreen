using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDropPanel : MonoBehaviour
{
    [Tooltip("Panel drops after touching player for ? seconds")]
    [SerializeField,Range(0,10)] private float dropDelay = 2f;
    private float dropTimeRemains;
    private bool hasInteracted;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (dropTimeRemains > 0)
        {
            dropTimeRemains -= Time.deltaTime;
            if (dropTimeRemains <= 0)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == PlayerProperty.player && !hasInteracted)
        {
            print("player in range");
            dropTimeRemains = dropDelay;
            hasInteracted = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Destroy(gameObject,2f);
        }
    }
}
