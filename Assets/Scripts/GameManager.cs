﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // delegate methods
    public delegate void OnPlayerDie();

    internal OnPlayerDie onPlayerDieCallback;

    public delegate void OnSceneChange(bool is3D);

    internal OnSceneChange OnSceneChangeCallback;


    private const string _playerSaveCoordinate = "PlayerLastCoordinate";


    public bool gameIsOver;
    public bool gameIsPaused;
    public bool isPlayerDying;

    public List<GameObject> gameObjects; // Including enemy, player, and environment
    public GameObject gameOverPanel; // TODO
    [SerializeField] private Sprite healthImage;

    internal bool is3D;

    public List<Image> livesUI;

    public GameObject player;
    public int playerLives = 3;

    private float originalZ;    // The z value of every game object in 2D

    public Animator animator;
    public PlayerAnimator PlayerAnimator;

    public static GameManager Instance { get; private set; }

    internal List<LiveObject> liveObjects;    // use to change the collider of all the gameobjects in run time when switch between 2D and 3D

    private void Awake()
    {
        if (Instance == null) Instance = this;
        originalZ = player.transform.position.z;
        liveObjects = new List<LiveObject>();
    }

    public bool PlayerDying;

    // Start is called before the first frame update
    private void Start()
    {
        CreatePlayerSaveSpot();

        RefreshHeartUi();
        OnSceneChangeCallback += RearrangeObjectsBasedOnScene;
        OnSceneChangeCallback?.Invoke(false);

        onPlayerDieCallback += Die;
        onPlayerDieCallback += RefreshHeartUi;
        
        
    }

    public void CreatePlayerSaveSpot()
    {
        var playerPosition = player.transform.position;

        PlayerPrefs.SetString(_playerSaveCoordinate, $"{playerPosition.x},{playerPosition.y},{playerPosition.z}");
    }

    private void RearrangeObjectsBasedOnScene(bool changeTo3D)
    {
        if (changeTo3D)
        {
            is3D = true;
            foreach (LiveObject liveObject in liveObjects)
            {
                if (liveObject != null)
                {
                    if (liveObject.boxTriggerCollider != null)
                    {
                        liveObject.boxTriggerCollider.size = new Vector3(liveObject.boxTriggerCollider.size.x,liveObject.boxTriggerCollider.size.y,liveObject.originalBoxTriggerColliderSizeZ);
                    }

                    if (liveObject.boxCollider != null)
                    {
                        liveObject.boxCollider.size = new Vector3(liveObject.boxCollider.size.x,liveObject.boxCollider.size.y,liveObject.originalBoxColliderSizeZ);
                    }
                }
            }
//            foreach (var objectInGame in gameObjects)
//                if (objectInGame != null)
//                {
//                    var objPos = objectInGame.transform.position;
//                    objPos = new Vector3(objPos.x, objPos.y,
//                        originalZ);
//                    objectInGame.transform.position = objPos;
//                    if (objectInGame.layer == LayerMask.NameToLayer("Enemy"))
//                    {
//                        objectInGame.GetComponent<Rigidbody>().constraints =
//                            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
//                    }
//                    else if (objectInGame.layer == LayerMask.NameToLayer("Player"))
//                    {
//                        objectInGame.GetComponent<Rigidbody>().constraints =
//                            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
//                    }
//                    else if (objectInGame.layer == LayerMask.NameToLayer("Environment"))
//                    {
//                    }
//    
//                }
        }
        else
        {
            is3D = false;
            print("Change to 2D scene");
            foreach (LiveObject liveObject in liveObjects)
            {
                if (liveObject != null)
                {
                    if (liveObject.boxTriggerCollider != null)
                    {
                        liveObject.originalBoxTriggerColliderSizeZ = liveObject.boxTriggerCollider.size.z;
                        liveObject.boxTriggerCollider.size = new Vector3(liveObject.boxTriggerCollider.size.x,liveObject.boxTriggerCollider.size.y,1000);
                    }

                    if (liveObject.boxCollider != null)
                    {
                        liveObject.originalBoxColliderSizeZ = liveObject.boxCollider.size.z;
                        liveObject.boxCollider.size = new Vector3(liveObject.boxCollider.size.x,liveObject.boxCollider.size.y,1000);
                    }
                }
            }
//            originalZ = player.transform.position.z;
//            foreach (var objectInGame in gameObjects)
//            {
//                if (objectInGame != null)
//                {
//                    var objPos = objectInGame.transform.position;
//                    objPos = new Vector3(objPos.x, objPos.y,
//                        Random.Range(widthHigherPoint, widthLowerPoint));
//                    objectInGame.transform.position = objPos;
//                    objectInGame.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
//    
//                }
//            }
        }
    }

    private void Die()
    {
        playerLives -= 1;
        if (playerLives <= 0)
            Gameover();
        else
            RefreshHeartUi();
    }

    private void Gameover()
    {
        gameOverPanel.SetActive(true); // Turn on the game over menu
        PlayerPrefs.DeleteAll(); // Delete all player data when game is turned off
    }

    private void RefreshHeartUi()
    {
        for (var i = 0; i < playerLives; i++) livesUI[i].sprite = healthImage;

        for (var i = playerLives; i < livesUI.Count; i++) livesUI[i].sprite = null;
    }
    
    
}