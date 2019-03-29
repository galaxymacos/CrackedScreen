using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 offsetIn3D;
    public float smoothSpeed = 0.125f;
    private Transform target;

    private Camera mainCamera;

    private void Start()
    {
        target = PlayerProperty.player.transform;
        mainCamera = Camera.main;
        GameManager.Instance.player.GetComponent<PlayerController>().onFacingChangeCallback += FlipCamera;
        GameManager.Instance.OnSceneChangeCallback += RotateCamera;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition;
        if (GameManager.Instance.is3D)
        {
//            mainCamera.orthographic = false;

            desiredPosition = target.position + offsetIn3D;
        }
        else
        {
//            mainCamera.orthographic = true;
            desiredPosition = target.position + offset;
        }

        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (GameManager.Instance.is3D)
        {
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private void FlipCamera(bool isFacingRight)
    {
        offset = new Vector3(-offset.x, offset.y, offset.z);
        offsetIn3D = new Vector3(-offsetIn3D.x, offsetIn3D.y, offsetIn3D.z);
    }

    private void RotateCamera(bool is3D)
    {
        transform.Rotate(is3D?20:-20,0,0);
    }
}