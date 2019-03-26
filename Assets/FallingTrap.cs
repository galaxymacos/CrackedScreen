using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FallingTrap : MonoBehaviour
{
    private Rigidbody rb;

    private Player playerScript;

    [SerializeField] private GameObject snowLotus;
    private bool hasInteracted;

    [SerializeField] private float delayBeforeFalling = 1f;
    private float crackingTimeRemains;

    [SerializeField] private AudioClip soundOfIceCracking;
    [SerializeField] private AudioClip soundOfIceFalling;
    private AudioSource audioSource;

    private void Start()
    {
        if (soundOfIceFalling == null || soundOfIceCracking == null)
        {
            print("There is no sound attached");
        }

        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.Instance.player.GetComponent<Player>();
    }

    private void Update()
    {
        if (crackingTimeRemains > 0)
        {
            crackingTimeRemains -= Time.deltaTime;
            if (crackingTimeRemains <= 0)
            {
                rb.useGravity = true;
            }
        }
    }

    public void Play()
    {
        AudioManager.instance.PlaySfx("IceCracking"); // TODO add sound to AudioManager
        crackingTimeRemains = delayBeforeFalling;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            var thisTransform = transform;
            Physics.Raycast(thisTransform.position, thisTransform.forward, out var hit);
            playerScript.TakeDamage(10);
            playerScript.GetKnockOff(hit.point);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && !hasInteracted)
        {
            Instantiate(snowLotus, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            hasInteracted = true;
        }
    }
}