using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fencesDeathCheck : MonoBehaviour
{
    [SerializeField] GameObject GameOverUI;
    // Update is called once per frame
    void Update()
    {
        if (fenceScript.death)
        {
            GameOverUI.SetActive(true);
        }
    }
}
