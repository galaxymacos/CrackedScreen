using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public float openHeight = 5f;
    public Vector3 destination;
    public float smoothValue = 0.5f;
    public bool isBossDie;

    [SerializeField] private GameObject smokeParticleEffect;
    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position + new Vector3(0, openHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBossDie)
        {
            transform.position = Vector3.Lerp(transform.position, destination, smoothValue);
            if (smokeParticleEffect != null)
            {
                smokeParticleEffect.SetActive(true);
                
            }
        }
    }

    public void BossDie()
    {
        isBossDie = true;
    }
}
