using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHp = 200f;
    public float hp = 200;
    public float maxRage = 200f;
    public float rage = 0f;

    private PlayerMovement _playerMovement;
    private PlayerController _playerController;

    public Vector3 knockUpForce = new Vector3(200, 200, 0);

    private Rigidbody rb;

    public GameObject floatingText;


    private void Start()
    {
//        GameManager.Instance.gameObjects.Add(gameObject);

        rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        hp = maxHp;
        
        GameManager.Instance.onPlayerDieCallback += RestorePlayerHealth;
    }

    [SerializeField] private float invincibieTime = 2f;
    private float lastTimeTakeDamage;
    internal float lastTimeKnockOff;
    private static readonly int knockOff = Animator.StringToHash("Knock Off");

    public void TakeDamage(int damage)
    {
        if (lastTimeTakeDamage + invincibieTime > Time.time)
        {
            return;
        }
        lastTimeTakeDamage = Time.time;
        if (GameManager.Instance.PlayerDying)
        {
            return;
        }

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block)
        {
            print("block enemy attack");
            AudioManager.instance.PlaySfx("Defend");
        }
        else
        {
            AudioManager.instance.PlaySfx("PlayerHurt");

            ChangeHpTo(hp-damage);
            ChangeRageTo(rage+damage);
            var playerTransform = transform;
            var floatingDamage = Instantiate(floatingText, playerTransform.position+new Vector3(0,2,0), Quaternion.identity);
            floatingDamage.GetComponent<TextMesh>().text = damage.ToString();
            GameUi.Instance.hpBar.fillAmount = hp / maxHp;
            if (hp <= 0)
            {
<<<<<<< HEAD
                print("playing death animation");
=======
>>>>>>> parent of 679ed4cd... Merge branch 'master' of https://github.com/galaxymacos/CrackedScreen
                GameManager.Instance.PlayerAnimator.PlayerStartDying();
            }    
        }
    }

    private void ChangeHpTo(float newHp)
    {
        hp = newHp;
        GameUi.Instance.hpBar.fillAmount = hp / maxHp;
    }

    private void ChangeRageTo(float newRageLevel)
    {
        if (newRageLevel < 100)
        {
            rage = newRageLevel;
            GameUi.Instance.mpBar.fillAmount = rage / maxRage;    
        }
        else
        {
            // rage level is full
        }
        
    }

    // Player debuff

    public void GetStunned(float sec)
    {
        StartCoroutine(Stun(sec));
    }

    public void GetKnockOff(Vector3 attackPosition)
    {
        
        if (lastTimeKnockOff + invincibieTime > Time.time)
        {
            return;
        }

        lastTimeKnockOff = Time.time;
        if (hp <= 0)
        {
            return;
        }
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
        {
            rb.velocity = new Vector3(-knockUpForce.x, knockUpForce.y, knockUpForce.z);


        }
        else // If enemy attacks from the right
        {
            rb.velocity = knockUpForce;
        }
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
    }

    public void GetKnockOff(Vector3 attackPosition, Vector3 force)
    {
        if (lastTimeKnockOff + invincibieTime > Time.time)
        {
            return;
        }

        lastTimeKnockOff = Time.time;
        if (hp <= 0)
        {
            return;
        }
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
        {
            rb.velocity = new Vector3(-force.x, force.y, force.z);


        }
        else // If enemy attacks from the right
        {
            rb.velocity = force;
        }
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
    }
    
    

    private IEnumerator Stun(float sec)
    {
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Stunned);
        _playerController.canControl = false;
        yield return new WaitForSeconds(sec);
        _playerMovement.ChangePlayerState(_playerMovement.playerPreviousState);
        _playerController.canControl = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly") && !GameManager.Instance.PlayerDying)
        {
            print("Player dies to death zone");
            ChangeHpTo(0);
            GameManager.Instance.PlayerAnimator.PlayerStartDying();
        }
    }

    public void RestorePlayerHealth()
    {
        ChangeHpTo(maxHp);
    }
}