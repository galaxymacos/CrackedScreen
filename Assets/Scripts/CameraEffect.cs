using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraEffect : MonoBehaviour
{
    private Coroutine shakeCoroutine;

    public bool isShaking;
    public float shakeIntensity = 0.4f;
    public float shakeTimeRemain;
    public Vector3 originalPos;
    [SerializeField] private float sizeSmoothValue = 0.5f;
    private Camera _camera;

    private void Start()
    {
        originalPos = transform.localPosition;
        _camera = GetComponent<Camera>();
        destinationCameraSize = _camera.orthographicSize;
    }


    private void Update()
    {
        if (shakeTimeRemain > 0f)
        {
            shakeTimeRemain -= Time.deltaTime;
            if (shakeTimeRemain <= 0f)
            {
                isShaking = false;
            }
        }

        if (isShaking)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            transform.localPosition = new Vector3(x, y, originalPos.z);
        }
        else
        {
            transform.localPosition = originalPos;
        }

        if (Camera.main != null && Math.Abs(Camera.main.orthographicSize - destinationCameraSize) > Mathf.Epsilon)
        {
            ChangeSizeToTargetSize();
        }
    }

    private void ChangeSizeToTargetSize()
    {
        Camera.main.orthographicSize =
            Mathf.Lerp(Camera.main.orthographicSize, destinationCameraSize, sizeSmoothValue);
    }

    public void StartShaking()
    {
        isShaking = true;
    }

    public void ShakeForSeconds(float shakeTimeInSecond)
    {
        shakeTimeRemain = shakeTimeInSecond;
        isShaking = true;
    }

    public void StopShaking()
    {
        isShaking = false;
    }

    private float destinationCameraSize;

    public void EnlargeCamera(float size)
    {
        destinationCameraSize = size;
    }
}