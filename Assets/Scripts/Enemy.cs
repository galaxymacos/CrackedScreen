using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // State
    public enum EnemyState
    {
        Standing,
        GotHitToAir,
        LayOnGround
    }

    public EnemyState _enemyCurrentState;

    public delegate void OnChangeEnemyState(EnemyState enemyState);

    public OnChangeEnemyState OnChangeEnemyStateCallback;

    public void ChangeEnemyState(EnemyState enemyState)
    {
        if (_enemyCurrentState != enemyState)
        {
            _enemyCurrentState = enemyState;
            OnChangeEnemyStateCallback?.Invoke(enemyState);
        }
    }


    // Private 
    public int atk = 5;
    public float attackRange = 2f;
    public float attackSpeed = 1f; // 1 hit in 1 sec



    [Header("Enemy ability")] 
    [SerializeField] private bool canStun;
    [SerializeField] private bool canStiff;
    [SerializeField] private bool canKnockUp;

    // Ability
    public int defense = 1;
    // enemy type
    public float maxHp = 200f;
    public float HP;

    // Floating damage text
    [SerializeField]private GameObject floatingText;

    // buff and 
    protected bool isStiffed;

    public float laySec = 2f; // How many seconds the enemy will stay on ground if it got hit to the air
    public float laySecLeft;
    public float moveSpeed = 5f;
    public float nextAttackTime;


    public Rigidbody rb;


    internal float StiffTimeRemain;
    [SerializeField] private float stunDuration = 0.5f;

    private Player playerScript;
    public SpriteRenderer spriteRenderer;

    private float delayBeforeDestroy;

    public bool isFacingRight;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        HP = maxHp;
        rb = GetComponent<Rigidbody>();


        GameManager.Instance.gameObjects.Add(gameObject);

        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        ChangeEnemyState(EnemyState.Standing);

        nextAttackTime = 0;
    }


    public virtual void TakeDamage(float damage)
    {
        HP -= Mathf.Clamp(damage - defense,0,Mathf.Infinity);
        if (HP <= 0)
        {
            Die();
        }

        if (floatingText != null)
        {
            ShowFloatingText();            
        }
        else
        {
//            print("Floating text is missing");
        }
        
    }

    /// <summary>
    /// This method will be implemented by children only
    /// </summary>
    protected abstract void Die();    
    public void Stiff(float time)
    {
        if (!canStiff)
            return;
        isStiffed = true;
        StiffTimeRemain = time;
    }

    public virtual void KnockUp(Vector3 force)
    {
        if (!canKnockUp)
        {
            return;
        }
//        print("Try to knock up the enemy");
        if (_enemyCurrentState == EnemyState.GotHitToAir)
        {
            GetComponent<Animator>().SetTrigger("HitToAir");
        }
        FaceBasedOnPlayerPosition();
        rb.AddForce(force);
        ChangeEnemyState(EnemyState.GotHitToAir);
    }
    
    public void FaceBasedOnPlayerPosition() {
        if (AnimationPlaying())    // Can't change facing when boss is using its ability
        {
            return;
        }
        if (PlayerIsAtRight()) {
            Flip(true);
        }
        else
        {
            Flip(false);
        }
    }
    
    public void FaceBasedOnMoveDirection()
    {
        
        if (rb.velocity.x > 0)
        {
            Flip(true);
        }
        else
        {
            Flip(false);
        }
    }
    
    public bool PlayerIsAtRight() {
        return PlayerProperty.playerPosition.x - transform.position.x > 0;
    }

    /// <summary>
    /// This method will be called when enemy takes damage
    /// </summary>
    private void ShowFloatingText( )
    {
        
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = Convert.ToInt32(HP).ToString();

    }

    public virtual void Update()
    {
        if (canStiff)
        {
            TryUnstiffing();
        }


        if (canKnockUp)
        {
            if (_enemyCurrentState == EnemyState.LayOnGround)
            {
                TryStandUp();
            }
        }

        
        if (GameManager.Instance.PlayerDying)
        {
            return;
        }

        InteractWithPlayer();
    }

    private void TryUnstiffing()
    {
        if (StiffTimeRemain > 0)
        {
            isStiffed = true;
            StiffTimeRemain -= Time.deltaTime;
        }
        else
        {
            isStiffed = false;
        }
    }

    private void TryStandUp()
    {
        laySecLeft -= Time.deltaTime;
        if (laySecLeft <= 0)
        {
            StandUp();
        }
    }

    public virtual void StandUp()
    {
        ChangeEnemyState(EnemyState.Standing);
    }


    public void Flip(bool facingRight)    // Dealing with face changing in children behavior
    {
        if (facingRight)
        {
            transform.localScale = new Vector3(1,transform.localScale.y,transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1,transform.localScale.y,transform.localScale.z);
        }
    }

    [SerializeField] private bool canAttack = true;

    /// <summary>
    /// This method is called in update method
    /// </summary>
    public abstract void InteractWithPlayer();    

    public virtual void ForceMove(Vector3 force)
    {
        rb.MovePosition(transform.position+force);
    }

    /// <summary>
    ///  This method is not required, generally, this method will be implemented in InteractWithPlayer method
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// This method return whether the player is current playing an ability animation
    /// </summary>
    /// <returns></returns>
    public abstract bool AnimationPlaying();

    public void Attack()
    {
        PlayerProperty.playerClass.TakeDamage(atk);
            if (canStun && PlayerProperty.movementClass.playerCurrentState != PlayerMovement.PlayerState.Block)
            {
                PlayerProperty.playerClass.GetStunned(stunDuration);
                Debug.Log("Stun player");
            }

            if (canKnockUp && PlayerProperty.movementClass.playerCurrentState != PlayerMovement.PlayerState.Block)
            {
                PlayerProperty.playerClass.GetKnockOff(transform.position);
            }
            
        
       
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if ( other.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Die();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            if (_enemyCurrentState == EnemyState.GotHitToAir)
            {
                laySecLeft = laySec;
                ChangeEnemyState(EnemyState.LayOnGround);
            }
    }
}