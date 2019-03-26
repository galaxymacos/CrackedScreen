using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowLotus : MonoBehaviour
{
    private float scaleMultiplier = 1f;

    [SerializeField] private float scaleIncreaseSpeed = 1f;

    [SerializeField] private float existDuration = 1f;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySfx("SnowLotus");    // TODO find snow lotus sound
        Destroy(gameObject,existDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= scaleMultiplier + scaleIncreaseSpeed * Time.deltaTime;
    }
}
