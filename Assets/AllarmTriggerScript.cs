using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllarmTriggerScript : MonoBehaviour
{

    private AudioSource souce;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            souce.Play();
        }
    }

    private void Start()
    {
        souce = GetComponent<AudioSource>();
    }
}
