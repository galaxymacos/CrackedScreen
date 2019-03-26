using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class screenCrushScript : MonoBehaviour
{
    private float durationNb = 4;
    private float durationCracked = 5;
    [SerializeField] AudioClip TVNoise;
    [SerializeField] AudioClip CrackedSound;
    [SerializeField] Transform crackPosition;
    [SerializeField] GameObject crackedScreen;
    private bool haveCrached = false;

    private AudioSource souce;


    private void Start()
    {
        souce = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        souce.PlayOneShot(TVNoise,0.1f);
        durationNb -= Time.deltaTime;
        if (durationNb < 0)
        {
            if (!haveCrached)
            {
                Instantiate(crackedScreen, crackPosition.position, crackPosition.rotation);
                haveCrached=true;
                AudioSource.PlayClipAtPoint(CrackedSound, crackPosition.position);
            }
            souce.Stop();
            durationCracked -= Time.deltaTime;
            if (durationCracked < 0)
            { 
                SceneManager.LoadScene("schoolScene");
            }     
        }
    

    }
}
