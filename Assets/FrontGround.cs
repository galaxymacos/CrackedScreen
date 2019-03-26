using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontGround : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GameManager.Instance.OnSceneChangeCallback += ChangeVisibility;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChangeVisibility(bool is3D)
    {
        if (is3D)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
