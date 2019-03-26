using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : Interactable
{
    private PlayerMovement playerMovement;

    private const string _playerSaveCoordinate = "PlayerLastCoordinate";

    private void Start()
    {
        playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();
        GameManager.Instance.onPlayerDieCallback += RespawnPlayer;
    }

    public override void Interact()
    {
        print("Saving player progress");
        GameManager.Instance.CreatePlayerSaveSpot();
    }

    public void RespawnPlayer()
    {
        string encryptedCoordinate = PlayerPrefs.GetString(_playerSaveCoordinate);
        string[] encryptedConponent = encryptedCoordinate.Split(',');
        float x = float.Parse(encryptedConponent[0]);
        float y = float.Parse(encryptedConponent[1]);
        float z = float.Parse(encryptedConponent[2]);
        Vector3 playerLastCoordinate = new Vector3(x, y, z);
        GameManager.Instance.player.transform.position = playerLastCoordinate;
        playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Stand);
        print("Change player state when respawning");
    }
}