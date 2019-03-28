using System.Collections;
using UnityEngine;

public enum Position {
    Left, Right, Up, Down
}

public class Player : MonoBehaviour {
    private static readonly int knockOff = Animator.StringToHash("Knock Off");
    private PlayerController _playerController;

    private PlayerMovement _playerMovement;

    [SerializeField] private float defendRecoilForce = 10f;
    [SerializeField] private float defendRecoilTime = 0.5f;
    private Position defendRecoilDirection;
    
    private bool recoilDirection;
    private float defendRecoilTimeRemain = 0;

    public GameObject floatingText;
    public float hp = 200;

    [SerializeField] private float invincibieTime = 2f;

    public Vector3 knockUpForce = new Vector3(200, 200, 0);
    private float lastTimeKnockOff;
    private float lastTimeTakeDamage;
    public float maxHp = 200f;
    public float maxRage = 200f;
    public float rage;

    private Rigidbody rb;
    
    


    private void Start() {
//        GameManager.Instance.gameObjects.Add(gameObject);

        rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        hp = maxHp;

        GameManager.Instance.onPlayerDieCallback += RestorePlayerHealth;
    }

    private void Update() {
        if (defendRecoilTimeRemain > 0) {
            PlayerProperty.animator.SetBool("DefendRecoiling", true);
        }
        else {
            PlayerProperty.animator.SetBool("DefendRecoiling", false);
        }
        if (defendRecoilTimeRemain > 0) {
            defendRecoilTimeRemain -= Time.deltaTime;
            if (defendRecoilDirection == Position.Left) {
                transform.Translate(new Vector3(-defendRecoilForce*Time.deltaTime,0,0));
            }
            else {
                transform.Translate(new Vector3(defendRecoilForce*Time.deltaTime,0,0));
            }
        }
    }

    public void ResetInvincibleTime() {
        lastTimeKnockOff = 0;
        lastTimeTakeDamage = 0;
    }

    public void TakeDamage(int damage) {
        if (lastTimeTakeDamage + invincibieTime > Time.time) return;
        lastTimeTakeDamage = Time.time;
        if (GameManager.Instance.PlayerDying) return;

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block) {
            print("block enemy attack");
            AudioManager.instance.PlaySfx("Defend");
        }
        else {
            AudioManager.instance.PlaySfx("PlayerHurt");

            ChangeHpTo(hp - damage);
            ChangeRageTo(rage + damage);
            var playerTransform = transform;
            var floatingDamage = Instantiate(floatingText, playerTransform.position + new Vector3(0, 2, 0),
                Quaternion.identity);
            floatingDamage.GetComponent<TextMesh>().text = damage.ToString();
            GameUi.Instance.hpBar.fillAmount = hp / maxHp;
            if (hp <= 0) GameManager.Instance.PlayerAnimator.PlayerStartDying();
        }
    }

    private void ChangeHpTo(float newHp) {
        hp = newHp;
        GameUi.Instance.hpBar.fillAmount = hp / maxHp;
    }

    private void ChangeRageTo(float newRageLevel) {
        if (newRageLevel < 100) {
            rage = newRageLevel;
            GameUi.Instance.mpBar.fillAmount = rage / maxRage;
        }
    }

    // Player debuff

    public void GetStunned(float sec) {
        StartCoroutine(Stun(sec));
    }

    public void GetKnockOff(Vector3 attackPosition) {
        if (lastTimeKnockOff + invincibieTime > Time.time) return;

        if (_playerMovement.playerCurrentState == PlayerMovement.PlayerState.Block) {
            print("block enemy attack");
            AudioManager.instance.PlaySfx("Defend");
            defendRecoilTimeRemain = defendRecoilTime;
            if (attackPosition.x > transform.position.x) {
                defendRecoilDirection = Position.Left;
            }
            else {
                defendRecoilDirection = Position.Right;
            }
            return;
            
        }
        
        lastTimeKnockOff = Time.time;
        if (hp <= 0) return;
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
            rb.velocity = new Vector3(-knockUpForce.x, knockUpForce.y, knockUpForce.z);
        else // If enemy attacks from the right
            rb.velocity = knockUpForce;
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
    }

    public void GetKnockOff(Vector3 attackPosition, Vector3 force) {
        if (lastTimeKnockOff + invincibieTime > Time.time) return;

        lastTimeKnockOff = Time.time;
        if (hp <= 0) return;
        rb.velocity = Vector3.zero;
        PlayerProperty.controller.canControl = false;
        if (attackPosition.x < transform.position.x)
            rb.velocity = new Vector3(-force.x, force.y, force.z);
        else // If enemy attacks from the right
            rb.velocity = force;
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.KnockUp);
        GameManager.Instance.animator.SetTrigger(knockOff);
    }


    private IEnumerator Stun(float sec) {
        _playerMovement.ChangePlayerState(PlayerMovement.PlayerState.Stunned);
        _playerController.canControl = false;
        yield return new WaitForSeconds(sec);
        _playerMovement.ChangePlayerState(_playerMovement.playerPreviousState);
        _playerController.canControl = true;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly") && !GameManager.Instance.PlayerDying) {
            print("Player dies to death zone");
            ChangeHpTo(0);
            GameManager.Instance.PlayerAnimator.PlayerStartDying();
        }
    }

    public void RestorePlayerHealth() {
        ChangeHpTo(maxHp);
    }
}