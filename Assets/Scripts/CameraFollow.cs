using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 offsetIn3D;
    private Vector3 center;
    [SerializeField] private float horizontalLimit = 5f;
    [SerializeField] private float verticalLimit = 5f;
    [SerializeField] private GameObject player;

    private void Start()
    {
        center = player.transform.position;
        GameManager.Instance.OnSceneChangeCallback += RotateCamera;
    }

    private void LateUpdate()
    {
//        if (PlayerProperty.player.transform.position.x > center.x + horizontalLimit)
//        {
//            center = new Vector3(PlayerProperty.player.transform.position.x-horizontalLimit,center.y,center.z);
//        }
//        else if (PlayerProperty.player.transform.position.x < center.x - horizontalLimit)
//        {
//            center = new Vector3(PlayerProperty.player.transform.position.x+horizontalLimit,center.y,center.z);
//        }
//
//        transform.position = center;
//        if(!GameManager.Instance.is3D)
//        {
//            transform.rotation = Quaternion.identity;
//        }
    }

    private void RotateCamera(bool is3D)
    {
//        transform.Rotate(is3D?20:-20,0,0);
    }
}