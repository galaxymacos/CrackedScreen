using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private bool hasInteracted;
    // Start is called before the first frame update

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player && !hasInteracted)
        {
            GetComponent<Animator>().SetTrigger("SpikePopUp");
            hasInteracted = true;
        }
    }
}
