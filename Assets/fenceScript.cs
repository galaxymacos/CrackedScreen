using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fenceScript : MonoBehaviour
{
    static public bool death = false;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            fenceScript.death = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }
}
