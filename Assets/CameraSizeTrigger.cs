using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeTrigger : MonoBehaviour
{
    [SerializeField] private float multiplier = 1.5f;
    private bool hasInteracted;
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasInteracted && other.gameObject == PlayerProperty.player)
        {
            Camera.main.GetComponent<CameraEffect>().EnlargeCamera(Camera.main.orthographicSize*multiplier);
            hasInteracted = true;
        }
    }
}
