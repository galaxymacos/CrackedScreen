using System.Collections.Generic;
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

    internal readonly List<LiveObject> liveObjects = new List<LiveObject>();    // use to change the collider of all the gameobjects in run time when switch between 2D and 3D

    private void Awake()
    {
        if (Instance == null) Instance = this;
        originalZ = player.transform.position.z;
        
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
                    if (liveObject.boxTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxTriggerColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,pair.Value);
                        }
                    }

                    if (liveObject.boxColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,pair.Value);
                        }
                    }
                }
            }

        }
        else
        {
            is3D = false;
            print("Change to 2D scene");
            foreach (LiveObject liveObject in liveObjects)
            {
                if (liveObject != null)
                {
                    if (liveObject.boxTriggerColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxTriggerColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,1000);
                        }
                    }
                    
                    if (liveObject.boxColliderSizeDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<BoxCollider, float> pair in liveObject.boxColliderSizeDictionary)
                        {
                            pair.Key.size = new Vector3(pair.Key.size.x, pair.Key.size.y,1000);
                        }
                    }
                }
            }
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