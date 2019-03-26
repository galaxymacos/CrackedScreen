using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fenceSystemScript : MonoBehaviour
{
   [SerializeField] private GameObject[] fences;
    [SerializeField] private float duration3s;
    [SerializeField] private float duration2s;
    [SerializeField] private float duration1p8s;
    private float durationRandom1;
    private GameObject obj0,obj1,obj2, obj3, obj4, obj5, obj6;
    private bool fenceRun1, fenceRun2, fenceRun3, fenceRun4, fenceRun5, fenceRun6;
    private float duration1s0=1, duration1s1=1, duration1s2=1, duration1s3=1, duration1s4=1, duration1s5=1, duration1s6=1;
    private float durationRandom;

    // Start is called before the first frame update
    void Start()
    {
        duration3s = 3;
        duration2s = 2;
        duration1p8s = 1.8f;
        durationRandom1 = GetRandomNumber();
    }

    // Update is called once per frame
    void Update()
    {
        Fence1();
        Fence2();
        Fence3();
        Fence4();
    }

   private int GetRandomNumber()
    {
        System.Random randomNb = new System.Random();
        return randomNb.Next(3, 7);
    }

    void Fence1()
    {
        duration3s -= Time.deltaTime;
        if (duration3s < 0)
        {
            obj0 = Instantiate(fences[0]);
            duration3s = 4f;
            fenceRun1 = true;
        }
        if (fenceRun1)
        {
            duration1s0 -= Time.deltaTime;
            if (duration1s0 < 0)
            {
                Destroy(obj0.gameObject);
                duration1s0 = 1;
                fenceRun1 = false;
            }
        }
    }

    void Fence2()
    {
        duration2s -= Time.deltaTime;
        if (duration2s < 0)
        {
            obj1 = Instantiate(fences[1]);
            duration2s = 3f;
            fenceRun2 = true;
        }
        if (fenceRun2)
        {
            duration1s1 -= Time.deltaTime;
            if (duration1s1 < 0)
            {
                Destroy(obj1.gameObject);
                duration1s1 = 1;
                fenceRun2 = false;
            }
        }
    }

    void Fence3()
    {
        duration1p8s -= Time.deltaTime;
        if (duration1p8s < 0)
        {
            obj2 = Instantiate(fences[2]);
            duration1p8s = 2.8f;
            fenceRun3 = true;
        }
        if (fenceRun3)
        {
            duration1s2 -= Time.deltaTime;
            if (duration1s2 < 0)
            {
                Destroy(obj2.gameObject);
                duration1s2 = 1;
                fenceRun3 = false;
            }
        }
    }

    void Fence4()
    {
        float temp = GetRandomNumber();
        Debug.Log(durationRandom1);
        durationRandom1 -= Time.deltaTime;
        if (durationRandom1 < 0)
        {
            obj3 = Instantiate(fences[3]);
            durationRandom1 = temp + 1;
            fenceRun4 = true;
        }
        if (fenceRun4)
        {
            duration1s3 -= Time.deltaTime;
            if (duration1s3 < 0)
            {
                Destroy(obj3.gameObject);
                duration1s3 = 1;
                fenceRun4 = false;
            }
        }
    }
}
