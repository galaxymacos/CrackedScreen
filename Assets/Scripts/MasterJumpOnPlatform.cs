using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterJumpOnPlatform : MonoBehaviour
{
    [SerializeField] private GameObject Master;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            Master.GetComponent<Animator>().SetTrigger("JumpOnPlatform");
        }
    }
}