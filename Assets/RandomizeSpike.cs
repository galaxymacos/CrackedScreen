using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpike : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, 2);
        if(random == 0)
        {
        GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
