using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpLossText : MonoBehaviour
{
    public float duration = 1f;
    [SerializeField] private float floatingSpeed = 100f;

    private float _lastTime = 0f;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Translate(new Vector3(0, 100, 0) * Time.deltaTime);
        _lastTime += Time.deltaTime;
        if (_lastTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}