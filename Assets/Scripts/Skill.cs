using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] private string skillName;
    [SerializeField] internal float cooldown;
    [SerializeField] private Image skillImage;


    internal float TimePlayed = 0f;
    internal PlayerController playerController;
    internal PlayerMovement playerMovement;
    internal Rigidbody rb;
    private Animator animator;
    internal bool _skillNotOnCooldown = true;

    internal bool _isPlaying;
    private bool isSkillImageNotNull;


    // Start is called before the first frame update
    public virtual void Start()
    {
        isSkillImageNotNull = skillImage != null;
        _skillNotOnCooldown = true; // No cooldown when started
        animator = GameManager.Instance.animator;
        rb = GameManager.Instance.player.GetComponent<Rigidbody>();
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        UpdateSkillIcon();
    }

    private void UpdateSkillIcon()
    {
        if (_skillNotOnCooldown) // skill cooldown
        {
            if (isSkillImageNotNull)
            {
                skillImage.fillAmount = 1;
            }
        }
        else
        {
            if (isSkillImageNotNull)
            {
                skillImage.fillAmount = Mathf.Clamp01((Time.time - TimePlayed) / cooldown);
            }
        }
    }


    public IEnumerator PlayerCanControl(float afterSec)
    {
        yield return new WaitForSeconds(afterSec);
        playerController.canControl = true;
    }

    public virtual void Play()
    {
        _isPlaying = true;
        TimePlayed = Time.time;
    }
}