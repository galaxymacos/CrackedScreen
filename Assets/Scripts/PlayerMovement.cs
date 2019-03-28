using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        Jump,
        DoubleJump,
        Attack,
        Stand,
        Walk,
        Run,
        Block,
        Stunned,
        KnockUp,
        FallDown
    }

    public string[] AnimationVariables;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform cameraPosIn3D;


    // Player ability
    [SerializeField] private float doubleJumpForce = 500f;
    [SerializeField] private float dropdownSpeed = 0.05f;

    [SerializeField] private float gravity = 1000f;

    private bool hasKnockUp;
    private bool isFallingDown;

    public bool isGliding;

//    public bool isGrounded()
//    {
//        LayerMask ground = 1 << 11;
//        return Physics.Raycast(transform.position, Vector3.down, 1.3f, ground);
//    }

    public bool isGrounded;

    public bool isMoving;

    // Player State
    [SerializeField] private float jumpForce = 500f;

    private float lastFallDownTime;
    private float lastMoveTime;
    private bool lastWalkIsRight;

    // Player run property
    private float lastWalkTime;
    [SerializeField] private float moveSpeed = 10f;

    public PlayerState playerCurrentState;
    public PlayerState playerPreviousState;
    private Rigidbody rb;
    private float runDetectDelay = 0.5f; // 
    [SerializeField] private float runSpeed = 20f;

    private float startCountdown;
    [SerializeField] private float verticalMoveSpeed = 3f;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangePlayerState(PlayerState.Stand);
    }

    public void ChangePlayerState(PlayerState newPlayerState)
    {
        if (newPlayerState == playerCurrentState) // No need to change player state if player is already in that state
            return;

        if (newPlayerState == PlayerState.KnockUp) hasKnockUp = true;


        

        playerPreviousState = playerCurrentState;
        playerCurrentState = newPlayerState;
        AudioManager.instance.StopAllSfx();

        switch (newPlayerState)
        {
            case PlayerState.Jump:
                AudioManager.instance.PlaySfx("Jump");

                break;
            case PlayerState.DoubleJump:
                AudioManager.instance.PlaySfx("Double Jump");

                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Stand:
                break;
            case PlayerState.Walk:
                AudioManager.instance.PlaySfx("Walk");

                break;
            case PlayerState.Run:
                AudioManager.instance.PlaySfx("Run");

                break;
            case PlayerState.Block:
                AudioManager.instance.PlaySfx("Block");
                break;
            case PlayerState.Stunned:
                break;
            case PlayerState.KnockUp:
                break;
            case PlayerState.FallDown:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPlayerState), newPlayerState, null);
        }
        
        if (playerCurrentState == PlayerState.Jump)
        {
            var playerCollider = PlayerProperty.player.GetComponent<BoxCollider>();
            playerCollider.center = new Vector3(0, 1, 0);
        }
        else if (playerCurrentState == PlayerState.DoubleJump)
        {
            var playerCollider = PlayerProperty.player.GetComponent<BoxCollider>();
            playerCollider.center = new Vector3(0, 1.5f, 0);
        }
        else
        {
            var playerCollider = PlayerProperty.player.GetComponent<BoxCollider>();
            playerCollider.center = new Vector3(0, 0, 0);
        }
        ChangeAnimationAccordingToAction();
    }

    private void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.PlayerDying) return;
        if (CheckIfPlayerOnGround()) MovePlayerOnGround();

        animator.SetBool("Fall Down", VerticalVelocityIsNegative());
        if (VerticalVelocityIsNegative() && playerCurrentState != PlayerState.KnockUp)
        {
            ChangePlayerState(PlayerState.FallDown);
        }
        ApplyGravity();
    }

    public bool VerticalVelocityIsNegative()
    {
        if (CheckIfPlayerOnGround()) return false;
        return rb.velocity.y < -0.01f;
    }

    private void ApplyGravity()
    {
        rb.AddForce(0, -gravity * Time.fixedDeltaTime, 0);
    }


    public void Move(float horizontalMovement, float verticalMovement)
    {
        // Detect if player is walking or standing or running if it is on ground
        if (Math.Abs(horizontalMovement) < Mathf.Epsilon && Math.Abs(verticalMovement) < Mathf.Epsilon && isGrounded &&
            playerCurrentState != PlayerState.Jump && playerCurrentState != PlayerState.KnockUp)
        {
            if (playerCurrentState != PlayerState.Block)
            {
                // ChangePlayerState(PlayerState.Stand);
            }

            return;
        }
        Vector3 movement;
        if (GameManager.Instance.is3D)
        {
            
            if (playerCurrentState == PlayerState.Run)
            {
                movement = new Vector3(
                    horizontalMovement * runSpeed * Time.fixedDeltaTime,
                    0,
                    verticalMovement * verticalMoveSpeed * Time.fixedDeltaTime
                );
            }
            else
            {
                movement = new Vector3(
                    horizontalMovement * moveSpeed * Time.fixedDeltaTime,
                    0,
                    verticalMovement * verticalMoveSpeed * Time.fixedDeltaTime
                );
            }
        }

        else
        {
            if (playerCurrentState == PlayerState.Run)
            {
                movement = new Vector3(
                    horizontalMovement * runSpeed * Time.fixedDeltaTime,
                    0,
                    0
                );

            }
            else
            {
                
                movement = new Vector3(
                    horizontalMovement * moveSpeed * Time.fixedDeltaTime,
                    0,
                    0
                );
            }
        }


        if (movement.x > 0 && PlayerHasWallAtRight())
        {
            movement = new Vector3(0,movement.y,movement.z);
        }
        else if (movement.x < 0 && PlayerHasWallAtLeft())
        {
            movement = new Vector3(0,movement.y,movement.z);
        }
        transform.Translate(movement);

    }

    public void Jump()
    {
        if (isGrounded)
        {
            ResetVerticalVelocity();
            rb.AddForce(new Vector3(0, jumpForce, 0));
            ChangePlayerState(PlayerState.Jump);
        }
        else if (playerCurrentState == PlayerState.Jump ||
                 playerCurrentState == PlayerState.FallDown && playerPreviousState == PlayerState.Jump ||
                 playerCurrentState == PlayerState.FallDown &&
                 (playerPreviousState == PlayerState.Walk || playerPreviousState == PlayerState.Run))
        {
            ResetVerticalVelocity();
            rb.AddForce(new Vector3(0, doubleJumpForce));
            ChangePlayerState(PlayerState.DoubleJump);
        }
    }

    public void FallDown()
    {
        var playerVelocity = rb.velocity;
//        if (rb.velocity.y > Mathf.Epsilon)
//        {
            playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y - dropdownSpeed * Time.fixedDeltaTime,
                playerVelocity.z);
            rb.velocity = playerVelocity;
//        }
    }

    private void ResetVerticalVelocity()
    {
        var playerVelocity = rb.velocity;
        playerVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        rb.velocity = playerVelocity;
    }

    private bool CheckIfPlayerOnGround()
    {
        LayerMask groundLayer = 1 << 11;
        LayerMask slopeLayer = 1 << 15;
        var position = transform.position;
        var hasHitRightGround = Physics.Raycast(position+new Vector3(GetComponent<BoxCollider>().size.x/2,0), Vector3.down,
            GetComponent<BoxCollider>().size.y / 2 + 0.01f, groundLayer);
        var hasHitLeftGround = Physics.Raycast(position-new Vector3(GetComponent<BoxCollider>().size.x/2,0), Vector3.down,
            GetComponent<BoxCollider>().size.y / 2 + 0.01f, groundLayer);
        var hasHitSlope = Physics.Raycast(position, Vector3.down,
            GetComponent<BoxCollider>().size.y / 2 + 0.4f, slopeLayer);

        isGrounded = (hasHitRightGround || hasHitLeftGround) && rb.velocity.y <= 0 || hasHitSlope;
        if (isGrounded)
            if (hasKnockUp && rb.velocity.y <= 0) // hasKnockUp TODO is this variable really necessery?
            {
                hasKnockUp = false;
                MovePlayerOnGround();
                PlayerProperty.controller.canControl = true;
            }

        return isGrounded;
    }

    private bool PlayerHasWallAtRight()
    {
        LayerMask wallLayer = 1 << 14;
        var position = transform.position;
        var hasHitRightWall = Physics.Raycast(position, Vector3.right, GetComponent<BoxCollider>().size.x / 2 + 0.01f,
            wallLayer);
        return hasHitRightWall;
    }

    private bool PlayerHasWallAtLeft()
    {
        LayerMask wallLayer = 1 << 14;
        var position = transform.position;
        var hasHitRightWall = Physics.Raycast(position, Vector3.left, GetComponent<BoxCollider>().size.x / 2 + 0.01f,
            wallLayer);
        return hasHitRightWall;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Slope"))
            if (rb.velocity.y > 0)
                isGrounded = false;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = other.transform.Find("PlatformNode").transform;
        }
    }

    public void MovePlayerOnGround()
    {
        if (GameManager.Instance.player.GetComponent<PlayerController>().horizontalMovement > 0 ||
            GameManager.Instance.player.GetComponent<PlayerController>().horizontalMovement < 0||
            ( GameManager.Instance.player.GetComponent<PlayerController>().verticalMovement < 0|| GameManager.Instance.player.GetComponent<PlayerController>().verticalMovement > 0) && GameManager.Instance.is3D)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (playerCurrentState != PlayerState.Jump && playerCurrentState != PlayerState.DoubleJump)
                    ChangePlayerState(PlayerState.Run);
            }
            else
            {
                if (playerCurrentState != PlayerState.Jump && playerCurrentState != PlayerState.DoubleJump)
                    ChangePlayerState(PlayerState.Walk);
            }
        }
        else
        {
            if (playerCurrentState != PlayerState.Block && playerCurrentState != PlayerState.Jump)
                ChangePlayerState(PlayerState.Stand);
        }

    }

    public void ChangeAnimationAccordingToAction()
    {
        string variableStayOn;
        switch (playerCurrentState)
        {
            case PlayerState.Jump:
                variableStayOn = "Jump";
                break;
            case PlayerState.DoubleJump:
                variableStayOn = "Double Jump";
                break;
            case PlayerState.Attack:
                variableStayOn = "Attack";
                break;
            case PlayerState.Stand:
                variableStayOn = "Stand";
                break;
            case PlayerState.Walk:
                variableStayOn = "Walk";
                break;
            case PlayerState.Run:
                variableStayOn = "Run";
                break;
            case PlayerState.Block:
                variableStayOn = "Block";
                break;
            default:
                variableStayOn = "";
                break;
        }

        for (var i = 0; i < AnimationVariables.Length; i++)
            if (AnimationVariables[i] != variableStayOn)
                animator.SetBool(AnimationVariables[i], false);
            else
                animator.SetBool(AnimationVariables[i], true);
    }
}