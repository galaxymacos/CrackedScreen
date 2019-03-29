using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveObject : MonoBehaviour
{
    internal Dictionary<BoxCollider, float> boxTriggerColliderSizeDictionary;
    internal Dictionary<BoxCollider, float> boxColliderSizeDictionary;

    void Awake()
    {
        boxTriggerColliderSizeDictionary = new Dictionary<BoxCollider, float>();
        boxColliderSizeDictionary = new Dictionary<BoxCollider, float>();
        var boxColliders = GetComponents(typeof(BoxCollider));
        foreach (Component component in boxColliders)
        {
            var bc = component as BoxCollider;
            if (bc != null && bc.isTrigger)
            {
                boxTriggerColliderSizeDictionary.Add(bc,bc.size.z);
            }
            else if (bc != null && !bc.isTrigger)
            {
                boxColliderSizeDictionary.Add(bc,bc.size.z);
            }
        }

    }

    private void Start()
    {
        GameManager.Instance.liveObjects.Add(this);

    }
}
