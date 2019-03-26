using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    
    
}
