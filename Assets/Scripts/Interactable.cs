using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool playerInRange;
    [SerializeField] private bool autoInteract;
    public bool hasBeenInteracted;


    private void Update()
    {
        if (playerInRange)
        {
            if (autoInteract && !hasBeenInteracted)
            {
                hasBeenInteracted = true;
                Interact();
            }
            else if (!hasBeenInteracted && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
            else if (hasBeenInteracted && Input.anyKeyDown)
            {
                Interact();
            }
        }
    }

    public abstract void Interact();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = false;
        }
    }
}