using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveObject : MonoBehaviour
{
    internal BoxCollider boxTriggerCollider;
    internal float originalBoxTriggerColliderSizeZ;

    internal BoxCollider boxCollider;
    internal float originalBoxColliderSizeZ;
    

    // Start is called before the first frame update
    void Awake()
    {
        var boxColliders = GetComponents(typeof(BoxCollider));
        if (boxColliders.Length > 2 || boxColliders.Length == 0)
        {
            throw new InvalidOperationException(gameObject.name+" has more than two colliders");
        }
        foreach (Component component in boxColliders)
        {
            var bc = component as BoxCollider;
            if (bc != null && bc.isTrigger)
            {
                boxTriggerCollider = bc;
                originalBoxTriggerColliderSizeZ = bc.size.z;
            }
            else if (bc != null && !bc.isTrigger)
            {
                boxCollider = bc;
                originalBoxColliderSizeZ = bc.size.z;

            }
        }

    }

    private void Start()
    {
        GameManager.Instance.liveObjects.Add(this);

    }
}
