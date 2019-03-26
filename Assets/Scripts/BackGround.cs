using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Vector3 playerLastPosition;
    private float playerXValue;
    [SerializeField] private float backgroundMoveSpeed = 1f;

    private void Start()
    {
        playerLastPosition = GetPlayerPosition();
    }

    public Vector3 GetPlayerPosition()
    {
        return GameManager.Instance.player.transform.position;
    }

    private void FixedUpdate()
    {
        float differenceInX = GetPlayerPosition().x - playerLastPosition.x;
        transform.Translate(new Vector3(-differenceInX * backgroundMoveSpeed * Time.fixedDeltaTime, 0, 0));
        playerLastPosition = GetPlayerPosition();
    }
}