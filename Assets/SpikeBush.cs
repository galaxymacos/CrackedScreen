using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class SpikeBush : MonoBehaviour
{
    [SerializeField] private GameObject spikeGraphics;

    [SerializeField]private float leftLimit;
    [SerializeField]private float rightLimit;

    [SerializeField]
    private int numOfSpike;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfSpike; i++)
        {
            float x = Random.Range(leftLimit, rightLimit);
            Instantiate(spikeGraphics, transform.position + new Vector3(x,0,0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var objectPos = transform.position;
        Gizmos.DrawSphere(objectPos+new Vector3(leftLimit,0,0),1);
        Gizmos.DrawSphere(objectPos+new Vector3(rightLimit,0,0),1);
    }
}
