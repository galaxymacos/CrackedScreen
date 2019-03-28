using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraPosIn3D;


    // Player ability
    public bool canAwake;
    public bool canControl;
    public bool canControlDialogueBox;
    public float facingOffset = 1; // 1 if player is facing right and -1 if player is facing left
    private bool is3D;
    public bool isFacingRight;


    public delegate void OnFacingChange(bool isFacingRight);

    public OnFacingChange onFacingChangeCallback;

    // Player State
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float enter3DWorldDuration = 2f;
    public float powerAccumulateTime;
    public bool transferStoragePowerFull;

    private CameraEffect _cameraEffect;

    private PlayerMovement playerMovement;

    public float horizontalMovement;
    public float verticalMovement;


    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (Camera.main != null)
        {
            mainCamera = Camera.main;
            _cameraEffect = mainCamera.GetComponent<CameraEffect>();
        }

        isFacingRight = true;
        onFacingChangeCallback?.Invoke(true);
        canControl = true;
        canControlDialogueBox = true;
    }

    private void Update()
    {
        if (playerMovement.playerCurrentState == PlayerMovement.PlayerState.Stunned ||
            playerMovement.playerCurrentState == PlayerMovement.PlayerState.KnockUp ||
            playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block)
        {
            return; // Refuse all player input if player is being stunned or being knocked up
        }

        if (!canControl)
        {
            return;
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (canAwake) CheckIfPlayerAwakes();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
            playerMovement.isGliding = true;
        }


//        if (!Input.GetKey(KeyCode.Space))    // TODO to delete this to make the game easier by letting player to jump to the highest point once he/she presses the spacebar
//        {
//            playerMovement.FallDown();
//        }


//        // can't running while on the air
//        if (playerMovement.playerCurrentState != PlayerMovement.PlayerState.Jump &&
//            playerMovement.playerCurrentState != PlayerMovement.PlayerState.DoubleJump)
//        {
//            if (Input.GetKey(KeyCode.LeftControl))
//            {
//                playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Run);
//            }
//        }
    }


    private void CheckIfPlayerAwakes()
    {
        if (canControl)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Time.timeScale = 0.5f;
                _cameraEffect.StartShaking(); // Keep shaking
            }

            if (Input.GetKey(KeyCode.T))
                powerAccumulateTime += Time.deltaTime / Time.timeScale;
            if (powerAccumulateTime > enter3DWorldDuration)
            {
                transferStoragePowerFull = true;
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                powerAccumulateTime = 0;
                _cameraEffect.StopShaking();
                Time.timeScale = 1f;
                if (transferStoragePowerFull)
                {
                    GameManager.Instance.is3D = !GameManager.Instance.is3D;
                    if (GameManager.Instance.is3D)
                    {
                        GameManager.Instance.OnSceneChangeCallback?.Invoke(true);
                    }
                    else
                    {
                        GameManager.Instance.OnSceneChangeCallback?.Invoke(false);
                    }

                    transferStoragePowerFull = false; // reset storage power full
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!canControl)
        {
            return;
        }

        if (isFacingRight)
            facingOffset = 1;
        else
            facingOffset = -1;


        playerMovement.Move(horizontalMovement, verticalMovement);

        ChangeFaceDirection(horizontalMovement);
    }

    private void ChangeFaceDirection(float horizontalMovement) // Player Controller
    {
        {
            if (horizontalMovement < 0)
            {
                if (isFacingRight)
                {
                    var localScale = transform.localScale;
                    transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
                    isFacingRight = false;
                    onFacingChangeCallback?.Invoke(false);
                }
            }
            else if (horizontalMovement > 0)
            {
                if (!isFacingRight)
                {
                    var localScale = transform.localScale;
                    transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
                    isFacingRight = true;
                    onFacingChangeCallback?.Invoke(true);
                }
            }
        }
    }
}