using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumPhysics : MonoBehaviour
{
    public Rigidbody rb;

    public float leftPushRange;

    public float rightPushRange;

    public Vector3 velocityThreshold;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = velocityThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        Push();
    }

    private void Push()
    {
        if (transform.rotation.z > 0 &&
            transform.rotation.z < rightPushRange &&
            rb.angularVelocity.z < 0 &&
            rb.angularVelocity.z < velocityThreshold.z)
        {
            rb.angularVelocity = velocityThreshold;
        }
        else if (transform.rotation.z < 0 &&
                 transform.rotation.z > leftPushRange &&
                 rb.angularVelocity.z < 0 &&
                 rb.angularVelocity.z > velocityThreshold.z * -1)
        {
            rb.angularVelocity = velocityThreshold * -1;
        }
                
    }
}
