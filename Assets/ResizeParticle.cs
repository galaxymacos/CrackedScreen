using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeParticle : MonoBehaviour
{
    [SerializeField] private float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem m_system = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = m_system.main;
        main.startSizeMultiplier = scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
